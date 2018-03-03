using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameManager
{
    public class MercadoAldeanos : MonoBehaviour
    {
        [Serializable]
        struct VariablesMercadoAldeanos
        {
            public Text interfazMercado;
            internal Transform posicionInstanciaAldeano;
        }

        [SerializeField] VariablesMercadoAldeanos variablesMercadoAldeanos = new VariablesMercadoAldeanos();

        void Start()
        {
            variablesMercadoAldeanos.interfazMercado.text = "Aldeano: 5 madera\n y 3 piedra";
            ControladorEventos.controladorEventos.FuncionesActuales += ComprarAldeano;
        }

        void ComprarAldeano()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (GameManagement.gameManagement.AldeanosDisponibles > GameManagement.gameManagement.CantidadAldeanos)
                {
                    if (IndicesEconomicos.indicesEconomicos.CantidadMadera >= 5 && IndicesEconomicos.indicesEconomicos.CantidadPiedra >= 3)
                    {
                        IndicesEconomicos.indicesEconomicos.CantidadMadera -= 5;
                        IndicesEconomicos.indicesEconomicos.CantidadPiedra -= 3;
                        SpawnerMilitar.spawnerMilitar.PoolingObjeto(variablesMercadoAldeanos.posicionInstanciaAldeano.position, SpawnerMilitar.spawnerMilitar.Aldeanos);
                        GameManagement.gameManagement.CantidadAldeanos++;
                    }
                }
            }
        }
    }
}