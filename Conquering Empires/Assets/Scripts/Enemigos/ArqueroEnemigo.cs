using UnityEngine;
using UnityEngine.AI;
using GameManager;
using InteligenciaArtificialNPC;

public class ArqueroEnemigo : Soldado
{
    struct VariablesArqueroEnemigo
    {
        internal DestruirEdificio destruirEdificio;
        internal AtacarNPC atacarNPC;

        internal Transform posicionFlecha;

        internal LayerMask capasNPC;
        internal LayerMask capasEdificios;

        internal float tiempoGiroOjos;
        public float tiempoEntreDisparo;
        internal float sumaTime;
    }

    VariablesArqueroEnemigo variablesArqueroEnemigo = new VariablesArqueroEnemigo();

    void Awake()
    {
        Agente = GetComponent<NavMeshAgent>();
        AgenteObstaculo = GetComponent<NavMeshObstacle>();
        OjosAgente = transform.GetChild(0);
        variablesSoldado.posicionArma = transform.GetChild(1);
        variablesArqueroEnemigo.posicionFlecha = transform.GetChild(2);
        TransformAgente = transform;
        variablesSoldado.rangoDeAtaque = 20;
        variablesArqueroEnemigo.tiempoEntreDisparo = 0.75f;
    }

    void Start()
    {
        variablesArqueroEnemigo.capasNPC = GameManagement.gameManagement[2];
        variablesArqueroEnemigo.capasEdificios = GameManagement.gameManagement[1];
        if (AgenteObstaculo.enabled) AgenteObstaculo.enabled = false;
        if (!Agente.enabled) Agente.enabled = true;
        variablesArqueroEnemigo.destruirEdificio = new DestruirEdificio(OjosAgente, variablesArqueroEnemigo.capasEdificios, variablesSoldado.rangoDeAtaque, Agente, AgenteObstaculo, this);
        variablesArqueroEnemigo.atacarNPC = new AtacarNPC(OjosAgente, variablesArqueroEnemigo.capasNPC, variablesSoldado.rangoDeAtaque, Agente, AgenteObstaculo, this);

        Invoke("PrimerDestino", 1);
        Invoke("SacarArma", 1);
    }

    private void PrimerDestino()
    {
        posicionObjetivo = GameManagement.gameManagement.variablesGameManagement.objetivoEnemigos.position;
        ControladorEventos.controladorEventos.FuncionesActuales += ActualizarObjetivo;
        ControladorEventos.controladorEventos.FuncionesActuales += VerificarPresenciaEnemiga;
        ControladorEventos.controladorEventos.FuncionesActuales += Atacar;
        ControladorEventos.controladorEventos.FuncionesActuales += variablesArqueroEnemigo.destruirEdificio.AtacarEdificio;
        ControladorEventos.controladorEventos.FuncionesActuales += variablesArqueroEnemigo.destruirEdificio.BuscarCast;
        ControladorEventos.controladorEventos.FuncionesActuales += variablesArqueroEnemigo.atacarNPC.AtacarNPCs;
        ControladorEventos.controladorEventos.FuncionesActuales += variablesArqueroEnemigo.atacarNPC.BuscarCast;
    }

    private void SacarArma()
    {
        SpawnerMilitar.spawnerMilitar.PoolingObjeto(variablesSoldado.posicionArma, SpawnerMilitar.spawnerMilitar.Arcos, variablesSoldado.posicionArma, "ArmasEnemigo/Arco");
    }

    void ActualizarObjetivo()
    {
        if (Agente.enabled == true)
        {
            Agente.destination = posicionObjetivo + DistanciaRelativaAlObjetivo;
        }
    }

    void VerificarPresenciaEnemiga()
    {
        variablesArqueroEnemigo.tiempoGiroOjos += Time.fixedDeltaTime;
        OjosAgente.localRotation = Quaternion.Lerp(Quaternion.Euler(0, -60, 0), Quaternion.Euler(0, 60, 0), Mathf.PingPong(variablesArqueroEnemigo.tiempoGiroOjos * 8, 1));
    }

    void Atacar()
    {
        if (variablesArqueroEnemigo.atacarNPC.variablesAtacarNPC.atacarObjetivo || variablesArqueroEnemigo.destruirEdificio.variablesDestruirEdificio.atacarObjetivo)
        {
            if (Atacando && Time.time > variablesArqueroEnemigo.sumaTime)
            {
                TransformAgente.LookAt(posicionObjetivo);
                variablesArqueroEnemigo.sumaTime = Time.time + variablesArqueroEnemigo.tiempoEntreDisparo;
                SpawnerMilitar.spawnerMilitar.PoolingObjeto(variablesArqueroEnemigo.posicionFlecha, SpawnerMilitar.spawnerMilitar.Flechas, "ArmasEnemigo/Flecha");
            }
        }
    }

    public override void EliminarDelegate()
    {
        ControladorEventos.controladorEventos.FuncionesActuales -= ActualizarObjetivo;
        ControladorEventos.controladorEventos.FuncionesActuales -= VerificarPresenciaEnemiga;
        ControladorEventos.controladorEventos.FuncionesActuales -= Atacar;
        if (variablesArqueroEnemigo.destruirEdificio != null && variablesArqueroEnemigo.atacarNPC != null)
        {
            ControladorEventos.controladorEventos.FuncionesActuales -= variablesArqueroEnemigo.destruirEdificio.AtacarEdificio;
            ControladorEventos.controladorEventos.FuncionesActuales -= variablesArqueroEnemigo.destruirEdificio.BuscarCast;
            ControladorEventos.controladorEventos.FuncionesActuales -= variablesArqueroEnemigo.atacarNPC.AtacarNPCs;
            ControladorEventos.controladorEventos.FuncionesActuales -= variablesArqueroEnemigo.atacarNPC.BuscarCast;
        }
    }

    private void RestaurarArma()
    {
        for (int i = 0; i < variablesSoldado.posicionArma.childCount; i++)
        {
            variablesSoldado.posicionArma.GetChild(i).gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        EliminarDelegate();
        RestaurarArma();
        if (AgenteObstaculo.enabled) AgenteObstaculo.enabled = false;
        if (Agente.enabled) Agente.enabled = false;
        Destroy(this);
    }
}