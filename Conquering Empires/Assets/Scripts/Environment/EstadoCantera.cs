using UnityEngine;
using UnityEngine.AI;

public class EstadoCantera : RecursoNatural
{
    private struct VariablesEstadoCantera
    {
        public int cantidadPiedra;
    }
    private VariablesEstadoCantera variablesEstadoCantera = new VariablesEstadoCantera();

    public override int RecursoDisponible
    {
        get
        {
            return variablesEstadoCantera.cantidadPiedra;
        }
        set
        {
            variablesEstadoCantera.cantidadPiedra = value;
            variablesEstadoCantera.cantidadPiedra++;
        }
    }

    private void Awake()
    {
        RecursoDisponible = 150;
    }
}