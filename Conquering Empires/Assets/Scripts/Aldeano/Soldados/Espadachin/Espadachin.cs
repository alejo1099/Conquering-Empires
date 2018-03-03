using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GameManager;
using InteligenciaArtificialNPC;

public class Espadachin : Soldado
{
    struct VariablesEspadachin
    {
        internal Transform referenciaWayPoints;

        internal LayerMask capasDeseadas;
        internal float tiempoLerpEspada;
        internal float tiempoLerpReinicio;
        internal bool restauracionCompleta;
    }

    VariablesEspadachin variablesEspadachin = new VariablesEspadachin();

    private void Awake()
    {
        ObtenerComponentesPrincipales();
        RangoDeAtaque = 2;
        Capas = GameManagement.gameManagement[0];
    }

    private void Start()
    {
        PrimeraAsignacion();
        IrPorArma();
        AsignacionEventos.VerificarAsignar(IrAlmacenArmas);
    }

    private void PrimeraAsignacion()
    {
        ObtenerWayPoints();
        MaquinaDeEstados = new MaquinaDeEstados(Agente, AgenteObstaculo, OjosAgente, Capas, WayPoints, RangoDeAtaque);
        TransformAgente.tag = "Aldeano/Soldado/Espadachin";
        if (AgenteObstaculo.enabled) AgenteObstaculo.enabled = false;
        if (!Agente.enabled) Agente.enabled = true;
    }

    private void ObtenerWayPoints()
    {
        variablesEspadachin.referenciaWayPoints = SpawnerMilitar.spawnerMilitar.WayPoints[Random.Range(0, SpawnerMilitar.spawnerMilitar.WayPoints.Length)];
        WayPoints = new Transform[variablesEspadachin.referenciaWayPoints.childCount];
        for (int i = 0; i < variablesEspadachin.referenciaWayPoints.childCount; i++)
        {
            WayPoints[i] = variablesEspadachin.referenciaWayPoints.GetChild(i);
        }
    }

    private void IrAlmacenArmas()
    {
        if (AlLlegarAlDestino(TransformAgente.position, posicionObjetivo, 3))
        {
            SpawnerMilitar.spawnerMilitar.PoolingObjeto(PosicionArma, SpawnerMilitar.spawnerMilitar.Espadas, PosicionArma);
            // if (variablesSoldado.maquinaDeEstados.variablesMaquinaDeEstados.patrulla == null)
            // {
            //     //variablesSoldado.maquinaDeEstados.ActivarPatrulla(Agente, OjosAgente, variablesEspadachin.capasDeseadas, variablesEspadachin.wayPoints, this);
            // }
            // else
            // {
            //     variablesSoldado.maquinaDeEstados.ActivarPatrulla();
            // }
            // ControladorEventos.controladorEventos.FuncionesActuales -= IrAlmacenArmas;
            // ControladorEventos.controladorEventos.FuncionesActuales += VerificarPersecucion;
        }
    }

    private void VerificarPersecucion()
    {
        // if (variablesSoldado.maquinaDeEstados.variablesMaquinaDeEstados.persecusion != null && variablesSoldado.maquinaDeEstados.variablesMaquinaDeEstados.persecusion.PersecucionActivada)
        // {
        //     //if (!objetivoActualAgente) objetivoActualAgente = variablesSoldado.maquinaDeEstados.variablesMaquinaDeEstados.persecusion.variablesPersecusion.objetivoActual;

        //     if (Vector3.Distance(TransformAgente.position, objetivoActualAgente.position) <= variablesSoldado.rangoDeAtaque)
        //     {
        //         Atacando = true;
        //     }
        //     else
        //     {
        //         Atacando = false;
        //         variablesEspadachin.restauracionCompleta = false;
        //     }
        // }
        // else
        // {
        //     Atacando = false;
        //     variablesEspadachin.restauracionCompleta = false;
        // }
        // AtaqueEspadacin();
        // ReiniciarRotacion();
    }

    private void AtaqueEspadacin()
    {
        if (Atacando)
        {
            variablesEspadachin.tiempoLerpEspada += Time.fixedDeltaTime;
            TransformAgente.LookAt(objetivoActualAgente);
            variablesSoldado.posicionArma.localRotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(90, 0, 60), Mathf.PingPong(variablesEspadachin.tiempoLerpEspada * 4, 1));
            if (variablesSoldado.posicionArma.localEulerAngles.x > 9)
            {
                variablesEspadachin.tiempoLerpReinicio = 0;
            }
        }
    }

    private void ReiniciarRotacion()
    {
        if (!Atacando && !variablesEspadachin.restauracionCompleta)
        {
            variablesEspadachin.tiempoLerpReinicio += Time.fixedDeltaTime;
            variablesSoldado.posicionArma.localRotation = Quaternion.Lerp(variablesSoldado.posicionArma.localRotation, Quaternion.identity, variablesEspadachin.tiempoLerpEspada);
            if (variablesSoldado.posicionArma.localRotation == Quaternion.identity)
            {
                variablesEspadachin.tiempoLerpEspada = 0;
                variablesEspadachin.restauracionCompleta = true;
            }
        }
    }

    public override void EliminarDelegate()
    {
        base.EliminarDelegate();

        ControladorEventos.controladorEventos.FuncionesActuales -= IrPorArma;
        ControladorEventos.controladorEventos.FuncionesActuales -= IrAlmacenArmas;
        ControladorEventos.controladorEventos.FuncionesActuales -= VerificarPersecucion;
    }

    private void OnDisable()
    {
        EliminarDelegate();
        if (AgenteObstaculo.enabled) AgenteObstaculo.enabled = false;
        if (Agente.enabled) Agente.enabled = false;
        DesactivarArma();
        Destroy(this);
    }
}
