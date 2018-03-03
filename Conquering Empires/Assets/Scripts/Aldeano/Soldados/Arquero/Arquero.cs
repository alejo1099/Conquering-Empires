using UnityEngine;
using UnityEngine.AI;
using GameManager;
using InteligenciaArtificialNPC;

public class Arquero : Soldado
{
    private struct VariablesArquero
    {
        public Transform posicionFlechas;
        public Transform referenciaWayPoints;
        public int rangoDeTiempo;
        public float intervaloFlechas;
    }
    private VariablesArquero variablesArquero = new VariablesArquero();

    private void Awake()
    {
        ObtenerComponentesPrincipales();
        variablesArquero.posicionFlechas = transform.GetChild(3);
        RangoDeAtaque = 20;
        variablesArquero.rangoDeTiempo = 2;
    }

    private void Start()
    {
        PrimeraAsignacion();
        IrPorArma();
        AsignacionEventos.VerificarAsignar(IrAlmacenArmas);
    }

    private void PrimeraAsignacion()
    {
        Capas = GameManagement.gameManagement[0];
        ConseguirWayPoints();
        MaquinaDeEstados = new MaquinaDeEstados(Agente, AgenteObstaculo, OjosAgente, Capas, WayPoints, RangoDeAtaque);
        MaquinaDeEstados.OnPersecutionActivate += ActivarAtaque;
        MaquinaDeEstados.OnPatrolActivate += DesactivarAtaque;

        TransformAgente.tag = "Aldeano/Soldado/Arquero";
        if (AgenteObstaculo.enabled) AgenteObstaculo.enabled = false;
        if (!Agente.enabled) Agente.enabled = true;
        variablesArquero.intervaloFlechas = Time.time + variablesArquero.rangoDeTiempo;
    }

    private void ConseguirWayPoints()
    {
        variablesArquero.referenciaWayPoints = SpawnerMilitar.spawnerMilitar.WayPoints[Random.Range(0, SpawnerMilitar.spawnerMilitar.WayPoints.Length)];
        WayPoints = new Transform[variablesArquero.referenciaWayPoints.childCount];

        for (int i = 0; i < variablesArquero.referenciaWayPoints.childCount; i++)
        {
            WayPoints[i] = variablesArquero.referenciaWayPoints.GetChild(i);
        }
    }

    private void IrAlmacenArmas()
    {
        if (AlLlegarAlDestino())
        {
            SpawnerMilitar.spawnerMilitar.PoolingObjeto(PosicionArma, SpawnerMilitar.spawnerMilitar.Arcos, PosicionArma);
            AsignacionEventos.VerificarRemover(IrAlmacenArmas);
            MaquinaDeEstados.ActivarPatrulla();
        }
    }

    private void ActivarAtaque()
    {
        AsignacionEventos.VerificarAsignar(VerificarDisparoFlechas);
    }

    private void DesactivarAtaque()
    {
        AsignacionEventos.VerificarRemover(VerificarDisparoFlechas);
    }

    private void VerificarDisparoFlechas()
    {
        //if(AlLlegarAlDestino(TransformAgente.position, objetivoActualAgente.position, RangoDeAtaque))
        if (AlLlegarAlDestino())
        {
            DispararHaciaObjetivo();
        }
    }

    private void DispararHaciaObjetivo()
    {
        TransformAgente.LookAt(Agente.destination);
        if (Time.time > variablesArquero.intervaloFlechas)
        {
            variablesArquero.intervaloFlechas = Time.time + variablesArquero.rangoDeTiempo;
            SpawnerMilitar.spawnerMilitar.PoolingObjeto(variablesArquero.posicionFlechas, SpawnerMilitar.spawnerMilitar.Flechas);
        }
    }

    public override void EliminarDelegate()
    {
        base.EliminarDelegate();
        AsignacionEventos.VerificarRemover(IrAlmacenArmas);
        AsignacionEventos.VerificarRemover(VerificarDisparoFlechas);
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