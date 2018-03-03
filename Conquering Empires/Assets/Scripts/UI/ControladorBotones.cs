using System;
using UnityEngine;

namespace Emperor
{
    public class ControladorBotones : MonoBehaviour
    {
        [Serializable]
        public class ElementosEconomia
        {
            public GameObject edificiosEconomicos;
            public GameObject botonEconomicos;
        }

        [Serializable]
        public class ElementosMilitares
        {
            public GameObject edificiosMilitares;
            public GameObject botonMilitares;

            public GameObject arqueria;
            public GameObject establo;
            public GameObject infanteria;
        }

        public static ControladorBotones SingletonControladorBotones;

        [SerializeField] private ElementosEconomia elementosEconomia;
        [SerializeField] private ElementosMilitares elementosMilitares;

        private void Awake()
        {
            if (SingletonControladorBotones == null)
                SingletonControladorBotones = this;
            else
                Destroy(this);
        }

        public void ActivarEdificiosMilitares()
        {
            elementosEconomia.botonEconomicos.SetActive(false);
            elementosMilitares.botonMilitares.SetActive(false);

            elementosEconomia.edificiosEconomicos.SetActive(false);
            elementosMilitares.edificiosMilitares.SetActive(true);
        }

        public void ActivarEdificiosEconomicos()
        {
            elementosEconomia.botonEconomicos.SetActive(false);
            elementosMilitares.botonMilitares.SetActive(false);

            elementosMilitares.edificiosMilitares.SetActive(false);
            elementosEconomia.edificiosEconomicos.SetActive(true);
        }

        public void RegresarEconomicos()
        {
            elementosEconomia.edificiosEconomicos.SetActive(false);

            elementosEconomia.botonEconomicos.SetActive(true);
            elementosMilitares.botonMilitares.SetActive(true);
        }

        public void RegresarMilitares()
        {
            elementosMilitares.edificiosMilitares.SetActive(false);

            elementosEconomia.botonEconomicos.SetActive(true);
            elementosMilitares.botonMilitares.SetActive(true);
        }

        private void DesactivarConstruccion()
        {
            elementosMilitares.edificiosMilitares.SetActive(false);
            elementosEconomia.edificiosEconomicos.SetActive(false);

            elementosEconomia.botonEconomicos.SetActive(false);
            elementosMilitares.botonMilitares.SetActive(false);
        }

        private void ActivarConstruccion()
        {
            elementosEconomia.botonEconomicos.SetActive(true);
            elementosMilitares.botonMilitares.SetActive(true);
        }

        public void ActivarEstablo()
        {
            DesactivarConstruccion();
            elementosMilitares.establo.SetActive(true);
        }

        public void DesactivarEstablo()
        {
            elementosMilitares.establo.SetActive(false);
            ActivarConstruccion();
        }

        public void ActivarArqueria()
        {
            DesactivarConstruccion();
            elementosMilitares.arqueria.SetActive(true);
        }

        public void DesactivarArqueria()
        {
            elementosMilitares.arqueria.SetActive(false);
            ActivarConstruccion();
        }

        public void ActivarInfanteria()
        {
            DesactivarConstruccion();
            elementosMilitares.infanteria.SetActive(true);
        }

        public void DesactivarInfanteria()
        {
            elementosMilitares.infanteria.SetActive(false);
            ActivarConstruccion();
        }

        public void ActivarAlmacen()
        {

        }
    }
}