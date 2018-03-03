using System;
using System.Collections;
using UnityEngine;

namespace GameManager
{
    public class ControladorEventos : MonoBehaviour
    {
        private struct VariablesControladorEventos
        {
            public Action funcionesActuales;
        }
        private VariablesControladorEventos variablesControladorEventos = new VariablesControladorEventos();
        public static ControladorEventos controladorEventos;
        public Action FuncionesActuales { set { variablesControladorEventos.funcionesActuales = value; } get { return variablesControladorEventos.funcionesActuales; } }

        private void Awake()
        {
            AsignacionEventos.ConvertirSingleton<ControladorEventos>(ref controladorEventos, this, gameObject);
        }

        void Update()
        {
            if (variablesControladorEventos.funcionesActuales != null)
            {
                variablesControladorEventos.funcionesActuales();
            }
        }

        public void EjecutadorCorutinasTimer(Action OnEndCoroutine, float tiempoEspera)
        {
            StartCoroutine(CorutinaTmer(OnEndCoroutine, tiempoEspera));
        }

        private IEnumerator CorutinaTmer(Action OnEndCoroutine, float tiempoEspera)
        {
            yield return new WaitForSeconds(tiempoEspera);
            OnEndCoroutine();
        }
    }
}