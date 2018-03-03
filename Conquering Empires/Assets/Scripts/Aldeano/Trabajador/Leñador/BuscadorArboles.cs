using UnityEngine;

public class BuscadorArboles
{
    public struct VariablesBuscadorArboles
    {
        internal Transform transformAgente;
        internal Transform ojos;
        internal Transform arbolElegido;
        internal Transform objetivoActualAgente;

        internal Vector3 rotarOjos;
        internal Vector3 posicionArbolAnterior;
        internal Vector3 distanciaRelativaArbol;
        internal Vector3 posicionObjetivo;

        internal LayerMask capasEnvironmentArbol;

        internal bool arbolEncontrado;
    }

    public VariablesBuscadorArboles variablesBuscadorArboles = new VariablesBuscadorArboles();

    public BuscadorArboles(Transform transformAgente, Transform arbolElegido, Transform objetivoActualAgente, LayerMask capasEnvironmentArbol)
    {
        variablesBuscadorArboles.transformAgente = transformAgente;
        variablesBuscadorArboles.ojos = transformAgente.GetChild(0);
        variablesBuscadorArboles.arbolElegido = arbolElegido;
        variablesBuscadorArboles.objetivoActualAgente = objetivoActualAgente;
        variablesBuscadorArboles.capasEnvironmentArbol = capasEnvironmentArbol;
    }

    public void BuscarArbol()
    {
        variablesBuscadorArboles.ojos.localRotation = Quaternion.Slerp(Quaternion.Euler(
            variablesBuscadorArboles.rotarOjos), Quaternion.Euler(-variablesBuscadorArboles.rotarOjos), Mathf.PingPong(Time.time * 2f, 1f));

        if (BuscarObjetoSphereCast(ref variablesBuscadorArboles.arbolElegido))
        {
            variablesBuscadorArboles.arbolEncontrado = true;
            variablesBuscadorArboles.posicionArbolAnterior = variablesBuscadorArboles.arbolElegido.position;
            variablesBuscadorArboles.posicionArbolAnterior.y = 0;
            AlEncontrarElArbol();
        }
    }

    private void AlEncontrarElArbol()
    {
        variablesBuscadorArboles.distanciaRelativaArbol = (variablesBuscadorArboles.transformAgente.position - variablesBuscadorArboles.arbolElegido.position).normalized * 2f;
        variablesBuscadorArboles.distanciaRelativaArbol.y = 0;
        variablesBuscadorArboles.objetivoActualAgente = variablesBuscadorArboles.arbolElegido;
        variablesBuscadorArboles.posicionObjetivo = variablesBuscadorArboles.objetivoActualAgente.position + variablesBuscadorArboles.distanciaRelativaArbol;
        variablesBuscadorArboles.posicionObjetivo.y = 0;
        //ActualizarDestinoAgente(posicionObjetivo, 1.5f, 2);
        //IndiceDelegate++;
        //ControladorEventos.controladorEventos.FuncionesActuales -= BuscarArbol;
        //ControladorEventos.controladorEventos.FuncionesActuales += IrHaciaElArbol;
    }

    private bool BuscarObjetoSphereCast(ref Transform receptorDePosicion)
    {
        RaycastHit InformacionGolpe;
        if (Physics.SphereCast(variablesBuscadorArboles.ojos.position, 0.4f, variablesBuscadorArboles.ojos.forward,
        out InformacionGolpe, 20, variablesBuscadorArboles.capasEnvironmentArbol, QueryTriggerInteraction.Ignore))
        {
            if (InformacionGolpe.transform.CompareTag("Environment/Arbol"))
            {
                receptorDePosicion = InformacionGolpe.transform;
                return true;
            }
        }
        return false;
    }
}