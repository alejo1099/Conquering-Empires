using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorMapa : MonoBehaviour
{
    public struct VariablesControladorMapa
    {
        public Transform[] informacionBosques;
        public Transform[] informacionCanteras;
        public Transform[] informacionGranjas;
    }

    public VariablesControladorMapa variablesControladorMapa = new VariablesControladorMapa();
    public static ControladorMapa controladorMapa;

    void Awake()
    {
        if (controladorMapa == null)
        {
            controladorMapa = this;
        }
    }
}