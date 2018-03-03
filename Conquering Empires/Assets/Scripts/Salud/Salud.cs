using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManager;

public class Salud : MonoBehaviour
{
    internal struct VariableSalud
    {
        internal int cantidadVida;
        [HideInInspector] internal GameObject esteGameObject;
    }

    internal VariableSalud variableSalud = new VariableSalud();

    public void VerificarVida()
    {
        if (variableSalud.cantidadVida <= 0)
        {
            variableSalud.esteGameObject.SetActive(false);
            ControladorEventos.controladorEventos.FuncionesActuales -= VerificarVida;
        }
    }

    public void VerificarDaño()
    {
        
    }
}