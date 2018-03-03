using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GameManager;
using InteligenciaArtificialNPC;

public class EspadachinEnemigo : Soldado
{
    struct VariablesEspadachinEnemigo
    {
        internal DestruirEdificio destruirEdificio;
        internal AtacarNPC atacarNPC;

        internal LayerMask capasEdificios;
        internal LayerMask capasNPC;

        internal float tiempoGiroOjos;
        internal float tiempoLerpEspada;
        internal float tiempoRestaurarEspada;

        internal bool restauracionCompleta;
    }

    VariablesEspadachinEnemigo variablesEspadachinEnemigo = new VariablesEspadachinEnemigo();

    void Awake()
    {
        Agente = GetComponent<NavMeshAgent>();
        AgenteObstaculo = GetComponent<NavMeshObstacle>();
        TransformAgente = transform;
        OjosAgente = transform.GetChild(0);
        variablesSoldado.posicionArma = transform.GetChild(1);
        variablesSoldado.rangoDeAtaque = 2;
    }

    void Start()
    {
        variablesEspadachinEnemigo.capasNPC = GameManagement.gameManagement[2];
        variablesEspadachinEnemigo.capasEdificios = GameManagement.gameManagement[1];
        if (AgenteObstaculo.enabled) AgenteObstaculo.enabled = false;
        if (!Agente.enabled) Agente.enabled = true;
        variablesEspadachinEnemigo.destruirEdificio = new DestruirEdificio(OjosAgente, variablesEspadachinEnemigo.capasEdificios, variablesSoldado.rangoDeAtaque, Agente, AgenteObstaculo, this);
        variablesEspadachinEnemigo.atacarNPC = new AtacarNPC(OjosAgente, variablesEspadachinEnemigo.capasNPC, variablesSoldado.rangoDeAtaque, Agente, AgenteObstaculo, this);

        Invoke("PrimerDestino", 1);
        Invoke("SacarArma", 1);
    }

    private void PrimerDestino()
    {
        posicionObjetivo = GameManagement.gameManagement.variablesGameManagement.objetivoEnemigos.position;
        ControladorEventos.controladorEventos.FuncionesActuales += ActualizarDestino;
        ControladorEventos.controladorEventos.FuncionesActuales += VerificarPresenciaEnemiga;
        ControladorEventos.controladorEventos.FuncionesActuales += Atacar;
        ControladorEventos.controladorEventos.FuncionesActuales += variablesEspadachinEnemigo.destruirEdificio.AtacarEdificio;
        ControladorEventos.controladorEventos.FuncionesActuales += variablesEspadachinEnemigo.destruirEdificio.BuscarCast;
        ControladorEventos.controladorEventos.FuncionesActuales += variablesEspadachinEnemigo.atacarNPC.AtacarNPCs;
        ControladorEventos.controladorEventos.FuncionesActuales += variablesEspadachinEnemigo.atacarNPC.BuscarCast;
    }

    private void SacarArma()
    {
        SpawnerMilitar.spawnerMilitar.PoolingObjeto(variablesSoldado.posicionArma, SpawnerMilitar.spawnerMilitar.Espadas, variablesSoldado.posicionArma, "ArmasEnemigo/Espada");
    }

    void ActualizarDestino()
    {
        if (Agente.enabled == true)
        {
            Agente.destination = posicionObjetivo + DistanciaRelativaAlObjetivo;
        }
    }

    void VerificarPresenciaEnemiga()
    {
        variablesEspadachinEnemigo.tiempoGiroOjos += Time.fixedDeltaTime;
        OjosAgente.localRotation = Quaternion.Lerp(Quaternion.Euler(0, -60, 0), Quaternion.Euler(0, 60, 0), Mathf.PingPong(variablesEspadachinEnemigo.tiempoGiroOjos * 8, 1));
    }

    void Atacar()
    {
        if (variablesEspadachinEnemigo.atacarNPC.variablesAtacarNPC.atacarObjetivo || variablesEspadachinEnemigo.destruirEdificio.variablesDestruirEdificio.atacarObjetivo)
        {
            if (Atacando)
            {
                TransformAgente.LookAt(posicionObjetivo);
                AtaqueEspada();
            }
            else if (!variablesEspadachinEnemigo.restauracionCompleta)
            {
                RestaurarEspada();
            }
        }
        else
        {
            RestaurarEspada();
        }
    }

    private void AtaqueEspada()
    {
        variablesEspadachinEnemigo.tiempoLerpEspada += Time.fixedDeltaTime;
        variablesSoldado.posicionArma.localRotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(90, 0, 45), Mathf.PingPong(variablesEspadachinEnemigo.tiempoLerpEspada * 4, 1));
    }

    private void RestaurarEspada()
    {
        variablesEspadachinEnemigo.tiempoRestaurarEspada += Time.fixedDeltaTime;
        variablesSoldado.posicionArma.localRotation = Quaternion.Lerp(variablesSoldado.posicionArma.localRotation, Quaternion.identity, variablesEspadachinEnemigo.tiempoRestaurarEspada);
        if (variablesSoldado.posicionArma.localRotation == Quaternion.identity)
        {
            variablesEspadachinEnemigo.restauracionCompleta = true;
            variablesEspadachinEnemigo.tiempoLerpEspada = 0;
            variablesEspadachinEnemigo.tiempoRestaurarEspada = 0;
        }
    }

    private void RestaurarRotaciones()
    {
        variablesSoldado.posicionArma.localRotation = Quaternion.identity;
        OjosAgente.localRotation = Quaternion.identity;
        for (int i = 0; i < variablesSoldado.posicionArma.childCount; i++)
        {
            variablesSoldado.posicionArma.GetChild(i).gameObject.SetActive(false);
        }
    }

    public override void EliminarDelegate()
    {
        ControladorEventos.controladorEventos.FuncionesActuales -= ActualizarDestino;
        ControladorEventos.controladorEventos.FuncionesActuales -= VerificarPresenciaEnemiga;
        ControladorEventos.controladorEventos.FuncionesActuales -= Atacar;
        if (variablesEspadachinEnemigo.destruirEdificio != null && variablesEspadachinEnemigo.atacarNPC != null)
        {
            ControladorEventos.controladorEventos.FuncionesActuales -= variablesEspadachinEnemigo.destruirEdificio.AtacarEdificio;
            ControladorEventos.controladorEventos.FuncionesActuales -= variablesEspadachinEnemigo.destruirEdificio.BuscarCast;
            ControladorEventos.controladorEventos.FuncionesActuales -= variablesEspadachinEnemigo.atacarNPC.AtacarNPCs;
            ControladorEventos.controladorEventos.FuncionesActuales -= variablesEspadachinEnemigo.atacarNPC.BuscarCast;
        }
    }

    void OnDisable()
    {
        EliminarDelegate();
        RestaurarRotaciones();
        if (AgenteObstaculo.enabled) AgenteObstaculo.enabled = false;
        if (Agente.enabled) Agente.enabled = false;
        Destroy(this);
    }
}
