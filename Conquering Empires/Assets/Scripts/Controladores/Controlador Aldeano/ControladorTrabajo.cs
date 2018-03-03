using UnityEngine;

public class ControladorTrabajo : MonoBehaviour
{
    struct VariablesControladorTrabajo
    {
        internal ControladorEstados controladorEstados;
        internal bool componenteAgregado;
    }

    VariablesControladorTrabajo variablesControladorTrabajo = new VariablesControladorTrabajo();

    void Awake()
    {
        variablesControladorTrabajo.controladorEstados = GetComponent<ControladorEstados>();
        if (GetComponent<ControladorSoldado>())
            Destroy(GetComponent<ControladorSoldado>());
    }

    public void TrabajoAgricultor()
    {
        if (!variablesControladorTrabajo.componenteAgregado)
        {
            gameObject.AddComponent<Agricultor>();
            variablesControladorTrabajo.controladorEstados.variableSalud.cantidadVida = 100;
            variablesControladorTrabajo.componenteAgregado = true;
        }
    }

    public void TrabajoLeñador()
    {
        if (!variablesControladorTrabajo.componenteAgregado)
        {
            gameObject.AddComponent<Leñador>();
            variablesControladorTrabajo.controladorEstados.variableSalud.cantidadVida = 100;
            variablesControladorTrabajo.componenteAgregado = true;
        }
    }

    public void TrabajoMinero()
    {
        if (!variablesControladorTrabajo.componenteAgregado)
        {
            gameObject.AddComponent<Minero>();
            variablesControladorTrabajo.controladorEstados.variableSalud.cantidadVida = 100;
            variablesControladorTrabajo.componenteAgregado = true;
        }
    }

    void OnDisable()
    {
        variablesControladorTrabajo.componenteAgregado = false;
    }
}