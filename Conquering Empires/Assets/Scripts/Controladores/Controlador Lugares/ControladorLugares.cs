using System;
using UnityEngine;

public class ControladorLugares : MonoBehaviour
{
    [Serializable]
    public struct VariablesControladorLugares
    {
        [HeaderAttribute("Lugares Militares")]
        public Transform almacenArmas;

        [HeaderAttribute("Lugares Económicos")]
        public Transform almacenAlimentos;

        [HeaderAttribute("Lugares Recursos")]
        public Transform[] bosques;
        public Transform[] canteras;
        public Transform[] granjas;
    }

    [SerializeField] private VariablesControladorLugares variablesControladorLugares = new VariablesControladorLugares();
    public static ControladorLugares controladorLugares;

    public Transform[] Canteras { get { return variablesControladorLugares.canteras; } }
    public Transform[] Bosques { get { return variablesControladorLugares.bosques; } }
    public Transform[] Granjas { get { return variablesControladorLugares.granjas; } }

    private void Awake()
    {
        AsignacionEventos.ConvertirSingleton<ControladorLugares>(ref controladorLugares, this, gameObject);
    }
}