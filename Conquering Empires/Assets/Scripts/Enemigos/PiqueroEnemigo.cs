using UnityEngine;
using UnityEngine.AI;
using GameManager;
using InteligenciaArtificialNPC;

public class PiqueroEnemigo : Soldado
{
    struct VariablesPiqueroEnemigo
    {
        internal DestruirEdificio destruirEdificio;
        internal AtacarNPC atacarNPC;

        internal LayerMask capasEdificios;
        internal LayerMask capasNPC;

        internal Vector3 posicionOriginalPica;

        internal float tiempoGiroOjos;
        internal float tiempoLerpPica;
        internal float tiempoRestaurarPica;

        internal bool restauracionCompleta;
    }

    VariablesPiqueroEnemigo variablesPiqueroEnemigo = new VariablesPiqueroEnemigo();

    void Awake()
    {
        Agente = GetComponent<NavMeshAgent>();
        AgenteObstaculo = GetComponent<NavMeshObstacle>();
        TransformAgente = transform;
        OjosAgente = transform.GetChild(0);
        variablesSoldado.posicionArma = transform.GetChild(1);
        variablesSoldado.rangoDeAtaque = 3.75f;
    }

    void Start()
    {
        variablesPiqueroEnemigo.capasNPC = GameManagement.gameManagement[2];
        variablesPiqueroEnemigo.capasEdificios = GameManagement.gameManagement[1];
        if (AgenteObstaculo.enabled) AgenteObstaculo.enabled = false;
        if (!Agente.enabled) Agente.enabled = true;
        variablesPiqueroEnemigo.posicionOriginalPica = variablesSoldado.posicionArma.localPosition;
        variablesPiqueroEnemigo.destruirEdificio = new DestruirEdificio(OjosAgente, variablesPiqueroEnemigo.capasEdificios, variablesSoldado.rangoDeAtaque, Agente, AgenteObstaculo, this);
        variablesPiqueroEnemigo.atacarNPC = new AtacarNPC(OjosAgente, variablesPiqueroEnemigo.capasNPC, variablesSoldado.rangoDeAtaque, Agente, AgenteObstaculo, this);

        Invoke("PrimerDestino", 1);
        Invoke("SacarArma", 1);
    }

    private void PrimerDestino()
    {
        posicionObjetivo = GameManagement.gameManagement.variablesGameManagement.objetivoEnemigos.position;
        ControladorEventos.controladorEventos.FuncionesActuales += ActualizarObjetivo;
        ControladorEventos.controladorEventos.FuncionesActuales += VerificarPresenciaEnemiga;
        ControladorEventos.controladorEventos.FuncionesActuales += Atacar;
        ControladorEventos.controladorEventos.FuncionesActuales += variablesPiqueroEnemigo.destruirEdificio.AtacarEdificio;
        ControladorEventos.controladorEventos.FuncionesActuales += variablesPiqueroEnemigo.destruirEdificio.BuscarCast;
        ControladorEventos.controladorEventos.FuncionesActuales += variablesPiqueroEnemigo.atacarNPC.AtacarNPCs;
        ControladorEventos.controladorEventos.FuncionesActuales += variablesPiqueroEnemigo.atacarNPC.BuscarCast;
    }

    private void SacarArma()
    {
        SpawnerMilitar.spawnerMilitar.PoolingObjeto(variablesSoldado.posicionArma, SpawnerMilitar.spawnerMilitar.Picas, variablesSoldado.posicionArma, "ArmasEnemigo/Pica");
    }


    void ActualizarObjetivo()
    {
        if (Agente.enabled)
        {
            Agente.destination = posicionObjetivo + DistanciaRelativaAlObjetivo;
        }
    }

    void VerificarPresenciaEnemiga()
    {
        variablesPiqueroEnemigo.tiempoGiroOjos += Time.fixedDeltaTime;
        OjosAgente.localRotation = Quaternion.Lerp(Quaternion.Euler(0, -60, 0), Quaternion.Euler(0, 60, 0), Mathf.PingPong(variablesPiqueroEnemigo.tiempoGiroOjos * 8, 1));
    }

    void Atacar()
    {
        if (variablesPiqueroEnemigo.atacarNPC.variablesAtacarNPC.atacarObjetivo || variablesPiqueroEnemigo.destruirEdificio.variablesDestruirEdificio.atacarObjetivo)
        {
            if (Atacando)
            {
                TransformAgente.LookAt(posicionObjetivo);
                AtaquePica();
            }
            else if (!variablesPiqueroEnemigo.restauracionCompleta)
            {
                RestaurarPica();
            }
        }
        else
        {
            RestaurarPica();
        }
    }

    private void AtaquePica()
    {
        variablesPiqueroEnemigo.tiempoLerpPica += Time.fixedDeltaTime;
        variablesSoldado.posicionArma.localRotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(90, 0, 5), Mathf.PingPong(variablesPiqueroEnemigo.tiempoLerpPica * 4, 1));
        if (variablesSoldado.posicionArma.localRotation.eulerAngles.x >= 70)
        {
            TransformAgente.LookAt(posicionObjetivo);
            variablesSoldado.posicionArma.localPosition = Vector3.Lerp(variablesPiqueroEnemigo.posicionOriginalPica, new Vector3(0.3f, 0, 1.3f), Mathf.PingPong(variablesPiqueroEnemigo.tiempoLerpPica * 5, 1));
            variablesPiqueroEnemigo.tiempoRestaurarPica = 0;
        }
    }

    private void RestaurarPica()
    {
        variablesPiqueroEnemigo.tiempoRestaurarPica += Time.fixedDeltaTime;
        variablesSoldado.posicionArma.localRotation = Quaternion.Lerp(variablesSoldado.posicionArma.localRotation, Quaternion.identity, variablesPiqueroEnemigo.tiempoRestaurarPica);
        variablesSoldado.posicionArma.localPosition = Vector3.MoveTowards(variablesSoldado.posicionArma.localPosition, variablesPiqueroEnemigo.posicionOriginalPica, Time.deltaTime);
        if (variablesSoldado.posicionArma.localRotation == Quaternion.identity)
        {
            variablesPiqueroEnemigo.restauracionCompleta = true;
            variablesPiqueroEnemigo.tiempoLerpPica = 0;
            variablesPiqueroEnemigo.tiempoRestaurarPica = 0;
        }
    }

    public override void EliminarDelegate()
    {
        ControladorEventos.controladorEventos.FuncionesActuales -= ActualizarObjetivo;
        ControladorEventos.controladorEventos.FuncionesActuales -= VerificarPresenciaEnemiga;
        ControladorEventos.controladorEventos.FuncionesActuales -= Atacar;
        if (variablesPiqueroEnemigo.destruirEdificio != null && variablesPiqueroEnemigo.atacarNPC != null)
        {
            ControladorEventos.controladorEventos.FuncionesActuales -= variablesPiqueroEnemigo.destruirEdificio.AtacarEdificio;
            ControladorEventos.controladorEventos.FuncionesActuales -= variablesPiqueroEnemigo.destruirEdificio.BuscarCast;
            ControladorEventos.controladorEventos.FuncionesActuales -= variablesPiqueroEnemigo.atacarNPC.AtacarNPCs;
            ControladorEventos.controladorEventos.FuncionesActuales -= variablesPiqueroEnemigo.atacarNPC.BuscarCast;
        }
    }

    private void RestaurarRotaciones()
    {
        variablesSoldado.posicionArma.localRotation = Quaternion.identity;
        OjosAgente.localRotation = Quaternion.identity;
        variablesSoldado.posicionArma.localPosition = variablesPiqueroEnemigo.posicionOriginalPica;
        for (int i = 0; i < variablesSoldado.posicionArma.childCount; i++)
        {
            variablesSoldado.posicionArma.GetChild(i).gameObject.SetActive(false);
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