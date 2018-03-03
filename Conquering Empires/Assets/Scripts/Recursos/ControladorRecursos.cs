using System.Collections;
using UnityEngine;

namespace Emperor
{
    public class ControladorRecursos : MonoBehaviour
    {
        public static ControladorRecursos SingletonControladorRecursos;
        private int _cantidadMadera;
        private int _cantidadPiedra;
        private int _cantidadAlimento;
        private int _cantidadAldeanos;
        private int _capacidadAldeanos;
        private int _cantidadHierro;

        public int cantidadMadera
        {
            get
            {
                return _cantidadMadera;
            }
            set
            {
                _cantidadMadera = value;
                ActualizarRecursos();
            }
        }
        public int cantidadPiedra
        {
            get
            {
                return _cantidadPiedra;
            }
            set
            {
                _cantidadPiedra = value;
                ActualizarRecursos();
            }
        }
        public int cantidadAlimento
        {
            get
            {
                return _cantidadAlimento;
            }
            set
            {
                _cantidadAlimento = value;
                ActualizarRecursos();
            }
        }

        public int cantidadHierro
        {
            get
            {
                return _cantidadHierro;
            }
            set
            {
                _cantidadHierro = value;
                ActualizarRecursos();
            }
        }

        public int cantidadAldeanos
        {
            get
            {
                return _cantidadAldeanos;
            }
            set
            {
                _cantidadAldeanos = value;
                UIManager.SingletonUIManager.ActualizarAldeanos(_cantidadAldeanos, _capacidadAldeanos);
            }
        }

        public int capacidadAldeanos
        {
            get
            {
                return _capacidadAldeanos;
            }
            set
            {
                _capacidadAldeanos = value;
                UIManager.SingletonUIManager.ActualizarAldeanos(_cantidadAldeanos, _capacidadAldeanos);
            }
        }

        private void Awake()
        {
            AsignacionEventos.ConvertirSingleton(ref SingletonControladorRecursos, this, gameObject);
        }
        private void Start()
        {
            cantidadMadera = 500;
            cantidadPiedra = 500;
            cantidadAlimento = 500;
            cantidadHierro = 500;
            cantidadAldeanos = 0;
            capacidadAldeanos = 5;
        }

        private void ActualizarRecursos()
        {
            UIManager.SingletonUIManager.ActualizarRecursos(_cantidadMadera,
               _cantidadAlimento, _cantidadPiedra, _cantidadHierro);
        }
    }
}