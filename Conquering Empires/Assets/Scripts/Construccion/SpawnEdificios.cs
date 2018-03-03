using System;
using UnityEngine;
using Spawner;

namespace Emperor
{
    public class SpawnEdificios : Spawn
    {
        [Serializable]
        public class EdificiosMilitares
        {
            public GameObject establo;
            public GameObject arqueria;
            public GameObject cuartel;

            [HideInInspector] public GameObject[] establos;
            [HideInInspector] public GameObject[] arquerias;
            [HideInInspector] public GameObject[] cuarteles;
        }

        [Serializable]
        public class EdificiosEconomicos
        {
            public GameObject casa;
            public GameObject granja;
            public GameObject puestoLenador;
            public GameObject cantera;

            [HideInInspector] public GameObject[] casas;
            [HideInInspector] public GameObject[] granjas;
            [HideInInspector] public GameObject[] puestoLenadores;
            [HideInInspector] public GameObject[] canteras;
        }
        public static SpawnEdificios SingletonSpawnEdificios;
        [SerializeField] private EdificiosEconomicos edificiosEconomicos;
        [SerializeField] private EdificiosMilitares edificiosMilitares;

        private Transform esteTransform;

        private void Awake()
        {
            if (SingletonSpawnEdificios == null)
                SingletonSpawnEdificios = this;
            else
                Destroy(this);

            esteTransform = transform.GetChild(0);
            cantidadInstanciar = 50;
            InstanciarEdificiosEconomicos();
            InstanciarEdificiosMilitares();
        }

        private void InstanciarEdificiosEconomicos()
        {
            InstanciarObjeto(ref edificiosEconomicos.canteras, edificiosEconomicos.cantera, esteTransform);
            InstanciarObjeto(ref edificiosEconomicos.casas, edificiosEconomicos.casa, esteTransform);
            InstanciarObjeto(ref edificiosEconomicos.puestoLenadores, edificiosEconomicos.puestoLenador, esteTransform);
            InstanciarObjeto(ref edificiosEconomicos.granjas, edificiosEconomicos.granja, esteTransform);
        }

        private void InstanciarEdificiosMilitares()
        {
            InstanciarObjeto(ref edificiosMilitares.arquerias, edificiosMilitares.arqueria, esteTransform);
            InstanciarObjeto(ref edificiosMilitares.cuarteles, edificiosMilitares.cuartel, esteTransform);
            InstanciarObjeto(ref edificiosMilitares.establos, edificiosMilitares.establo, esteTransform);
        }

        public void InstanciarCasa(ref Transform transformCasa)
        {
            PoolingObjeto(transformCasa, ref transformCasa, edificiosEconomicos.casas);
        }

        public void InstanciarGranja(ref Transform transformGranja)
        {
            PoolingObjeto(transformGranja, ref transformGranja, edificiosEconomicos.granjas);
        }

        public void InstanciarPuestoLenador(ref Transform transformPuestoLenador)
        {
            PoolingObjeto(transformPuestoLenador, ref transformPuestoLenador, edificiosEconomicos.puestoLenadores);
        }

        public void InstanciarCantera(ref Transform transformCantera)
        {
            PoolingObjeto(transformCantera, ref transformCantera, edificiosEconomicos.canteras);
        }

        public void InstanciarCuartel(ref Transform transformCuartel)
        {
            PoolingObjeto(transformCuartel, ref transformCuartel, edificiosMilitares.cuarteles);
        }

        public void InstanciarEstablo(ref Transform transformEstablo)
        {
            PoolingObjeto(transformEstablo, ref transformEstablo, edificiosMilitares.establos);
        }

        public void InstanciarArqueria(ref Transform transformArqueria)
        {
            PoolingObjeto(transformArqueria, ref transformArqueria, edificiosMilitares.arquerias);
        }
    }
}