using UnityEngine;

public class ControladorSoldado : MonoBehaviour
{
    struct VariableControladorSoldado
    {
        internal bool componenteAgregado;
    }
    VariableControladorSoldado variableControladorSoldado = new VariableControladorSoldado();

    private void Awake()
    {
        if (GetComponent<ControladorTrabajo>())
            Destroy(GetComponent<ControladorTrabajo>());
    }
    
    public void SoldadoArquero()
    {
        if (!variableControladorSoldado.componenteAgregado)
        {
            gameObject.AddComponent<Arquero>();
            GetComponent<ControladorEstados>().variableSalud.cantidadVida = 150;
            variableControladorSoldado.componenteAgregado = true;
        }
    }

    public void SoldadoPiquero()
    {
        if (!variableControladorSoldado.componenteAgregado)
        {
            gameObject.AddComponent<Piquero>();
            GetComponent<ControladorEstados>().variableSalud.cantidadVida = 200;
            variableControladorSoldado.componenteAgregado = true;
        }
    }

    public void SoldadoEspadachin()
    {
        if (!variableControladorSoldado.componenteAgregado)
        {
            variableControladorSoldado.componenteAgregado = true;
            gameObject.AddComponent<Espadachin>();
            GetComponent<ControladorEstados>().variableSalud.cantidadVida = 250;
        }
    }

    void OnDisable()
    {
        variableControladorSoldado.componenteAgregado = false;
    }
}