using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Emperor
{
    public class ControladorPoblacion : MonoBehaviour
    {
        public static ControladorPoblacion SingletonControladorPoblacion;

        private void Awake()
        {
            AsignacionEventos.ConvertirSingleton(ref SingletonControladorPoblacion, this, gameObject);
        }

        public void AgregarAldeanos(int cantidadAgregar)
        {
            ControladorRecursos.SingletonControladorRecursos.cantidadAldeanos += cantidadAgregar;
        }

        public void AgregarCapacidadAldeanos(int cantidadAgregar)
        {
            ControladorRecursos.SingletonControladorRecursos.capacidadAldeanos += cantidadAgregar;
        }
    }
}