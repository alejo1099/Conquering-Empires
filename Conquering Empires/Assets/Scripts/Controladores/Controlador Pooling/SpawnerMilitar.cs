using UnityEngine;
using Spawner;

namespace GameManager
{
    public class SpawnerMilitar : Spawn
    {
        [System.Serializable]
        private struct VariablesSpawnerMilitar
        {
            [HideInInspector] public GameObject[] flechas;
            [HideInInspector] public GameObject[] arcos;
            [HideInInspector] public GameObject[] picas;
            [HideInInspector] public GameObject[] espadas;
            [HideInInspector] public GameObject[] aldeano;
            public GameObject prefabFlecha;
            public GameObject prefabArco;
            public GameObject prefabPica;
            public GameObject prefabEspada;
            public GameObject prefabAldeano;

            public Transform[] wayPoints;
            internal Transform esteTransform;
            public Transform almacenArmas;
        }

        [SerializeField] private VariablesSpawnerMilitar variablesSpawnerMilitar = new VariablesSpawnerMilitar();
        public static SpawnerMilitar spawnerMilitar;

        public GameObject[] Arcos { get { return variablesSpawnerMilitar.arcos; } }
        public GameObject[] Flechas { get { return variablesSpawnerMilitar.flechas; } }
        public GameObject[] Espadas { get { return variablesSpawnerMilitar.espadas; } }
        public GameObject[] Picas { get { return variablesSpawnerMilitar.picas; } }
        public GameObject[] Aldeanos { get { return variablesSpawnerMilitar.aldeano; } }

        public Transform[] WayPoints { get { return variablesSpawnerMilitar.wayPoints; } }

        private void Awake()
        {
            AsignacionEventos.ConvertirSingleton<SpawnerMilitar>(ref spawnerMilitar, this, gameObject);
            variablesSpawnerMilitar.esteTransform = transform;
            cantidadInstanciar = 50;
        }

        void Start()
        {
            InstanciarObjeto(ref variablesSpawnerMilitar.flechas, variablesSpawnerMilitar.prefabFlecha, variablesSpawnerMilitar.esteTransform.GetChild(0));
            InstanciarObjeto(ref variablesSpawnerMilitar.arcos, variablesSpawnerMilitar.prefabArco, variablesSpawnerMilitar.esteTransform.GetChild(0));
            InstanciarObjeto(ref variablesSpawnerMilitar.picas, variablesSpawnerMilitar.prefabPica, variablesSpawnerMilitar.esteTransform.GetChild(0));
            InstanciarObjeto(ref variablesSpawnerMilitar.espadas, variablesSpawnerMilitar.prefabEspada, variablesSpawnerMilitar.esteTransform.GetChild(0));
            //InstanciarObjeto(ref variablesSpawnerMilitar.aldeano, variablesSpawnerMilitar.prefabAldeano, variablesSpawnerMilitar.festeTransform.GetChild(0));
        }
    }
}