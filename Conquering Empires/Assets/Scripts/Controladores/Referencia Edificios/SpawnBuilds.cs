using System;
using UnityEngine;
using Spawner;

namespace GameManager
{
    public class SpawnBuilds : Spawn
    {
        [System.Serializable]
        public struct VariablesSpawnBuilds
        {
            [Header("Edificios Economicos")]
            public GameObject prefabCasa;
            public GameObject prefabAlmacenAlimento;
            public GameObject prefabAlmacenHierro;
            public GameObject prefabAlmacenMadera;
            public GameObject prefabAlmacenPiedra;
            public GameObject prefabMercado;

            [Header("Edificios Militares")]
            public GameObject prefabAlmacenArmas;
            public GameObject prefabArqueria;
            public GameObject prefabCastillo;
            public GameObject prefabCuartel;
            public GameObject prefabEstablo;
            public GameObject prefabMuro;
            public GameObject prefabTorre;

            [HideInInspector] public GameObject[] casa;
            [HideInInspector] public GameObject[] almacenAlimento;
            [HideInInspector] public GameObject[] almacenHierro;
            [HideInInspector] public GameObject[] almacenMadera;
            [HideInInspector] public GameObject[] almacenPiedra;
            [HideInInspector] public GameObject[] mercado;

            [HideInInspector] public GameObject[] almacenArmas;
            [HideInInspector] public GameObject[] arqueria;
            [HideInInspector] public GameObject[] castillo;
            [HideInInspector] public GameObject[] cuartel;
            [HideInInspector] public GameObject[] establo;
            [HideInInspector] public GameObject[] muro;
            [HideInInspector] public GameObject[] torre;
        }

        [SerializeField] public VariablesSpawnBuilds variablesSpawnBuilds;
        public static SpawnBuilds spawnBuilds;
        //Tamaño generico (11,5,12)
        //Tamaño muro (10,10,5)
        //Tamaño torre (10,11,10)
        //Tamaño castillo (30,11,30)

        private void Awake()
        {
            AsignacionEventos.ConvertirSingleton<SpawnBuilds>(ref spawnBuilds, this, gameObject);
            cantidadInstanciar = 50;
        }

        void Start()
        {
            InstanciarEdificios();
        }

        private void InstanciarEdificios()
        {
            InstanciarObjeto(ref variablesSpawnBuilds.casa, variablesSpawnBuilds.prefabCasa, transform.GetChild(0));
            // InstanciarObjeto(ref variablesSpawnBuilds.almacenHierro, variablesSpawnBuilds.prefabAlmacenHierro, transform.GetChild(0));
            // InstanciarObjeto(ref variablesSpawnBuilds.almacenAlimento, variablesSpawnBuilds.prefabAlmacenAlimento, transform.GetChild(0));
            // InstanciarObjeto(ref variablesSpawnBuilds.almacenMadera, variablesSpawnBuilds.prefabAlmacenMadera, transform.GetChild(0));
            // InstanciarObjeto(ref variablesSpawnBuilds.almacenPiedra, variablesSpawnBuilds.prefabAlmacenPiedra, transform.GetChild(0));
            //InstanciarObjeto(ref variablesSpawnBuilds.mercado, variablesSpawnBuilds.prefabMercado, transform.GetChild(0));

            //InstanciarObjeto(ref variablesSpawnBuilds.almacenArmas, variablesSpawnBuilds.prefabAlmacenArmas, transform.GetChild(0));
            //InstanciarObjeto(ref variablesSpawnBuilds.arqueria, variablesSpawnBuilds.prefabArqueria, transform.GetChild(0));
            //InstanciarObjeto(ref variablesSpawnBuilds.castillo, variablesSpawnBuilds.prefabCastillo, transform.GetChild(0));
            //InstanciarObjeto(ref variablesSpawnBuilds.cuartel, variablesSpawnBuilds.prefabCuartel, transform.GetChild(0));
            //InstanciarObjeto(ref variablesSpawnBuilds.establo, variablesSpawnBuilds.prefabCuartel, transform.GetChild(0));
            InstanciarObjeto(ref variablesSpawnBuilds.muro, variablesSpawnBuilds.prefabMuro, transform.GetChild(0));
            InstanciarObjeto(ref variablesSpawnBuilds.torre, variablesSpawnBuilds.prefabTorre, transform.GetChild(0));
        }
    }
}