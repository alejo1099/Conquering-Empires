using UnityEngine;
using UnityEngine.UI;
using Spawner;

namespace GameManager
{
    public class ControladorSpawn : Spawn
    {
        [System.Serializable]
        public struct VariablesControladorSpawn
        {
            internal GameObject[] alimento;
            internal GameObject[] tronco;
            internal GameObject[] piedra;

            public GameObject prefabAlimento;
            public GameObject prefabPiedra;
            public GameObject prefabTronco;

            internal Transform esteTransform;
        }

        [SerializeField] public VariablesControladorSpawn variablesControladorSpawn = new VariablesControladorSpawn();
        public static ControladorSpawn controladorSpawn;

        private void Awake()
        {
            AsignacionEventos.ConvertirSingleton<ControladorSpawn>(ref controladorSpawn, this, gameObject);
            variablesControladorSpawn.esteTransform = transform;
            cantidadInstanciar = 50;
        }

        void Start()
        {
            InstanciarObjeto(ref variablesControladorSpawn.alimento, variablesControladorSpawn.prefabAlimento, variablesControladorSpawn.esteTransform.GetChild(0));
            InstanciarObjeto(ref variablesControladorSpawn.tronco, variablesControladorSpawn.prefabTronco, variablesControladorSpawn.esteTransform.GetChild(0));
            InstanciarObjeto(ref variablesControladorSpawn.piedra, variablesControladorSpawn.prefabPiedra, variablesControladorSpawn.esteTransform.GetChild(0));
        }
    }
}