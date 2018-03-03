using UnityEngine;
using UnityEngine.UI;

namespace GameManager
{
    public class ControladorTextos : MonoBehaviour
    {
        [System.Serializable]
        struct VariablesControladorTextos
        {
            [SerializeField] internal Text textoEmplearAldeano;
            internal string[] textosEmplearAldeano;

            [SerializeField] internal Text textoEdificios;
            //internal string[] textosElegirEdificios;
        }

        [SerializeField] VariablesControladorTextos variablesControladorTextos = new VariablesControladorTextos();
        public static ControladorTextos controladorTextos;

        public int CambiarTextoEmplearAldeano
        {
            set
            {
                switch (value)
                {
                    case 0:
                        variablesControladorTextos.textoEmplearAldeano.text = variablesControladorTextos.textosEmplearAldeano[value];
                        break;
                    case 1:
                        variablesControladorTextos.textoEmplearAldeano.text = variablesControladorTextos.textosEmplearAldeano[value];
                        break;
                    case 2:
                        variablesControladorTextos.textoEmplearAldeano.text = variablesControladorTextos.textosEmplearAldeano[value];
                        break;
                    case 3:
                        variablesControladorTextos.textoEmplearAldeano.text = variablesControladorTextos.textosEmplearAldeano[value];
                        break;
                    default:
                        variablesControladorTextos.textoEmplearAldeano.text = variablesControladorTextos.textosEmplearAldeano[0];
                        break;
                }
            }
        }

        private void Awake()
        {
            AsignacionEventos.ConvertirSingleton<ControladorTextos>(ref controladorTextos,this,gameObject);
            AsignarTextosEmpleoAldeano();
        }

        private void Start()
        {
            ActualizarTextoEdificio();
        }

        private void AsignarTextosEmpleoAldeano()
        {
            variablesControladorTextos.textosEmplearAldeano = new string[4];
            variablesControladorTextos.textosEmplearAldeano[0] = " ";
            variablesControladorTextos.textosEmplearAldeano[1] = "Por favor presiona T \n para trabajar o presiona L \n para luchar";
            variablesControladorTextos.textosEmplearAldeano[2] = /*"Presione F para ser Agricultor, o */"Presione \n  M para ser Minero, o presione \n L para ser Leñador";
            variablesControladorTextos.textosEmplearAldeano[3] = "Presione E para \n ser Espadachin, o presione P para ser\n Piquero, o presione Q para ser Arquero";
        }

        public void ActualizarTextoEdificio()
        {
            variablesControladorTextos.textoEdificios.text = "Aldeanos " + GameManagement.gameManagement.
            variablesGameManagement.cantidadAldeanos + "/" + GameManagement.gameManagement.variablesGameManagement.aldeanosDisponibles;
        }
    }
}