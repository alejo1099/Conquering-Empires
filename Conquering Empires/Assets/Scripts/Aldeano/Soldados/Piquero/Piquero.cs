using UnityEngine;
using UnityEngine.AI;
using GameManager;
using InteligenciaArtificialNPC;

public class Piquero : Soldado
{
    struct VariablesPiquero
    {
        internal Transform referenciaWayPoints;

        internal LayerMask capasDeseadas;

        internal Quaternion rotacionX;

        internal Vector3 posicionOriginalPica;

        internal float tiempoLerpPica;
        internal float lerpRestaurar;

        internal bool restaurado;
    }

    VariablesPiquero variablesPiquero = new VariablesPiquero();

    void Awake()
    {
        Agente = GetComponent<NavMeshAgent>();
        AgenteObstaculo = GetComponent<NavMeshObstacle>();
        OjosAgente = transform.GetChild(0);
        TransformAgente = transform;
        variablesSoldado.posicionArma = transform.GetChild(1);
        variablesSoldado.rangoDeAtaque = 3.75f;
        variablesPiquero.capasDeseadas = GameManagement.gameManagement[0];
    }

    void Start()
    {
        Invoke("PrimeraAsignacion", 0.5f);
        Invoke("PrimerDestino", 1);
    }

    private void PrimeraAsignacion()
    {
        transform.tag = "Aldeano/Soldado";
        if (AgenteObstaculo.enabled) AgenteObstaculo.enabled = false;
        if (!Agente.enabled) Agente.enabled = true;
        variablesPiquero.rotacionX = Quaternion.Euler(80, 0, 5);
        variablesPiquero.posicionOriginalPica = variablesSoldado.posicionArma.localPosition;
        ObtenerWayPoints();
    }

    void ObtenerWayPoints()
    {
        variablesPiquero.referenciaWayPoints = SpawnerMilitar.spawnerMilitar.WayPoints[Random.Range(0, SpawnerMilitar.spawnerMilitar.WayPoints.Length)];
        WayPoints = new Transform[variablesPiquero.referenciaWayPoints.childCount];
        for (int i = 0; i < variablesPiquero.referenciaWayPoints.childCount; i++)
        {
            WayPoints[i] = variablesPiquero.referenciaWayPoints.GetChild(i);
        }
    }

    private void PrimerDestino()
    {
        ControladorEventos.controladorEventos.FuncionesActuales += IrPorArma;
    }

    public override void IrPorArma()
    {
        base.IrPorArma();
        ControladorEventos.controladorEventos.FuncionesActuales -= IrPorArma;
        ControladorEventos.controladorEventos.FuncionesActuales += AlLlegarAlAlmacenDeArmas;
    }

    void AlLlegarAlAlmacenDeArmas()
    {
        if (AlLlegarAlDestino())
        {
            // SpawnerMilitar.spawnerMilitar.PoolingObjeto(variablesSoldado.posicionArma, SpawnerMilitar.spawnerMilitar.variablesSpawnerMilitar.picas, variablesSoldado.posicionArma);
            // if (!AgenteReactivado || variablesSoldado.maquinaDeEstados.variablesMaquinaDeEstados.patrulla == null)
            // {
            //     //variablesSoldado.maquinaDeEstados.ActivarPatrulla(Agente, OjosAgente, variablesPiquero.capasDeseadas, variablesPiquero.wayPoints, this);
            // }
            // else
            // {
            //     variablesSoldado.maquinaDeEstados.ActivarPatrulla();
            // }
            ControladorEventos.controladorEventos.FuncionesActuales -= AlLlegarAlAlmacenDeArmas;
            ControladorEventos.controladorEventos.FuncionesActuales += VerificarAtaque;
        }
    }

    public void VerificarAtaque()
    {
        // if (variablesSoldado.maquinaDeEstados.variablesMaquinaDeEstados.persecusion != null && variablesSoldado.maquinaDeEstados.variablesMaquinaDeEstados.persecusion.PersecucionActivada)
        // {
        //     VerificarObjetivoExistente();
        // }
        // else
        // {
        //     Atacando = false;
        //     variablesPiquero.restaurado = false;
        // }
        // AtaquePica();
        // ReiniciarValoresPica();
    }

    void VerificarObjetivoExistente()
    {
        //if (!objetivoActualAgente) objetivoActualAgente = variablesSoldado.maquinaDeEstados.variablesMaquinaDeEstados.persecusion.variablesPersecusion.objetivoActual;

        if (Vector3.Distance(TransformAgente.position, objetivoActualAgente.position) <= variablesSoldado.rangoDeAtaque)
        {
            Atacando = true;
        }
        else
        {
            Atacando = false;
            variablesPiquero.restaurado = false;
        };
    }

    public void AtaquePica()
    {
        if (Atacando)
        {
            variablesPiquero.tiempoLerpPica += Time.fixedDeltaTime;
            variablesSoldado.posicionArma.localRotation = Quaternion.Slerp(variablesSoldado.posicionArma.localRotation, variablesPiquero.rotacionX, variablesPiquero.tiempoLerpPica * 2);
            if (variablesSoldado.posicionArma.localRotation.eulerAngles.x >= 70)
            {
                TransformAgente.LookAt(objetivoActualAgente);
                variablesSoldado.posicionArma.localPosition = Vector3.Lerp(variablesPiquero.posicionOriginalPica, new Vector3(0.3f, 0, 1.3f), Mathf.PingPong(variablesPiquero.tiempoLerpPica * 5, 1));
                variablesPiquero.lerpRestaurar = 0;
            }
        }
    }

    public void ReiniciarValoresPica()
    {
        if (!Atacando && !variablesPiquero.restaurado)
        {
            variablesPiquero.lerpRestaurar += Time.fixedDeltaTime;
            variablesSoldado.posicionArma.localRotation = Quaternion.Slerp(variablesSoldado.posicionArma.localRotation, Quaternion.identity, variablesPiquero.lerpRestaurar * 0.5f);
            variablesSoldado.posicionArma.localPosition = Vector3.MoveTowards(variablesSoldado.posicionArma.localPosition, variablesPiquero.posicionOriginalPica, Time.deltaTime);
            if (variablesSoldado.posicionArma.localPosition == variablesPiquero.posicionOriginalPica)
            {
                variablesPiquero.tiempoLerpPica = 0;
                variablesPiquero.restaurado = true;
            }
        }
    }

    public override void EliminarDelegate()
    {
        base.EliminarDelegate();

        ControladorEventos.controladorEventos.FuncionesActuales -= IrPorArma;
        ControladorEventos.controladorEventos.FuncionesActuales -= AlLlegarAlAlmacenDeArmas;
        ControladorEventos.controladorEventos.FuncionesActuales -= VerificarAtaque;
    }

    void OnDisable()
    {
        EliminarDelegate();
        if (AgenteObstaculo.enabled) AgenteObstaculo.enabled = false;
        if (Agente.enabled) Agente.enabled = false;
        DesactivarArma();
        Destroy(this);
    }
}