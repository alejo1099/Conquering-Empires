using UnityEngine;
using UnityEngine.UI;

public class IndicesEconomicos : MonoBehaviour
{
    [System.Serializable]
    struct VariablesEconomicas
    {
        public Text textoRecursos;
        internal int cantidadMadera;
        internal int cantidadPiedra;
        internal int cantidadAlimento;
    }

    [SerializeField] VariablesEconomicas variablesEconomicas = new VariablesEconomicas();

    public static IndicesEconomicos indicesEconomicos;

    public int CantidadMadera { get { return variablesEconomicas.cantidadMadera; } set { variablesEconomicas.cantidadMadera = value; } }
    public int CantidadPiedra { get { return variablesEconomicas.cantidadPiedra; } set { variablesEconomicas.cantidadPiedra = value; } }
    public int CantidadAlimento { get { return variablesEconomicas.cantidadAlimento; } set { variablesEconomicas.cantidadAlimento = value; } }

    private void Awake()
    {
        AsignacionEventos.ConvertirSingleton<IndicesEconomicos>(ref indicesEconomicos, this, gameObject);
        ActualizarRecursos();
    }

    public void ActualizarRecursos()
    {
        variablesEconomicas.textoRecursos.text = "Madera: " + variablesEconomicas.cantidadMadera + "\nPiedra: " + variablesEconomicas.cantidadPiedra;
    }
}