using System.Collections;
using UnityEngine;
using Spawner;

namespace GameManager
{
    public class PoolingEnemigos : Spawn
    {
        [System.Serializable]
        struct VariablesPoolingEnemigos
        {
            [HideInInspector] public GameObject[] arrayEnemigos;
            public GameObject prefabEnemigo;
            public Transform[] puntosSpawn;
            internal int numeroDeInstanciasEnemigos;
        }

        [SerializeField] VariablesPoolingEnemigos variablesPoolingEnemigos = new VariablesPoolingEnemigos();

        void Start()
        {
            cantidadInstanciar = 50;
            InstanciarObjeto(ref variablesPoolingEnemigos.arrayEnemigos, variablesPoolingEnemigos.prefabEnemigo, transform.GetChild(0));
            InvokeRepeating("SpawnerEnemigos", 120, 60);
        }

        void SpawnerEnemigos()
        {
            StartCoroutine(CrearEnemigos());
        }

        private IEnumerator CrearEnemigos()
        {
            variablesPoolingEnemigos.numeroDeInstanciasEnemigos++;
            for (int i = 0; i < variablesPoolingEnemigos.numeroDeInstanciasEnemigos; i++)
            {
                yield return new WaitForSeconds(Random.Range(2, 5));
                PoolingObjeto(variablesPoolingEnemigos.puntosSpawn[Random.Range(0, variablesPoolingEnemigos.puntosSpawn.Length)], variablesPoolingEnemigos.arrayEnemigos);
            }
        }
    }
}