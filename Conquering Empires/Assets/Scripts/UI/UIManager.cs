using UnityEngine;
using UnityEngine.UI;

namespace Emperor
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager SingletonUIManager;

        [SerializeField] private Text textoRecursos;
        [SerializeField] private Text textoAldeanos;

        private void Awake()
        {
            AsignacionEventos.ConvertirSingleton(ref SingletonUIManager, this, gameObject);
        }

        public void ActualizarRecursos(int madera, int alimento, int piedra, int hierro)
        {
            textoRecursos.text = "Madera: " + madera + " Alimento: " + alimento + " Piedra: " + piedra + " Hierro: " + hierro;
        }

        public void ActualizarAldeanos(int cantidadAldeanos, int capacidadAldeanos)
        {
            textoAldeanos.text = "Aldenanos: " + cantidadAldeanos + "/" + capacidadAldeanos;
        }
    }
}