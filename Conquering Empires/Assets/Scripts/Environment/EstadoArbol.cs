using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EstadoArbol : RecursoNatural
{
    struct VariablesEstadoArbol
    {
        public Rigidbody rigidbody;
        public NavMeshObstacle navMeshObstacle;
        public int troncosDisponibles;
    }
    private VariablesEstadoArbol variablesEstadoArbol;
    public Action OnTreeCut;

    public override int RecursoDisponible
    {
        get
        {
            return variablesEstadoArbol.troncosDisponibles;
        }
        set
        {
            variablesEstadoArbol.troncosDisponibles = value;
            if (variablesEstadoArbol.troncosDisponibles < 1)
                StartCoroutine(TumbarArbol());
        }
    }

    public bool ArbolCortado { get; set; }

    private void Awake()
    {
        variablesEstadoArbol.rigidbody = GetComponent<Rigidbody>();
        variablesEstadoArbol.navMeshObstacle = GetComponent<NavMeshObstacle>();
        RecursoDisponible = 100;
    }

    private IEnumerator TumbarArbol()
    {
        if (OnTreeCut != null)
            OnTreeCut();
        DesactivarVariables();
        yield return new WaitForSeconds(10);
        ResetearVariables();
    }

    private void DesactivarVariables()
    {
        variablesEstadoArbol.navMeshObstacle.carving = false;
        variablesEstadoArbol.rigidbody.isKinematic = false;
        ArbolCortado = true;
    }

    private void ResetearVariables()
    {
        variablesEstadoArbol.rigidbody.isKinematic = true;
        RecursoDisponible = 100;
        gameObject.SetActive(false);
        OnTreeCut = null;
    }
}