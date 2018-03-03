using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InteligenciaArtificialNPC;

public class Trabajador : Aldeano<Transform>
{
    protected bool ObtenerRecurso(Transform recursoExtraer, Transform recursoAgregar)
    {
        RecursoNatural recursoNatural = recursoExtraer.GetComponent<RecursoNatural>();
        if (recursoNatural.RecursoDisponible >= 1)
        {
            recursoNatural.RecursoDisponible--;
        }
        else
        {
            return false;
        }
        recursoAgregar.GetComponent<ValorRecurso>().CantidadRecurso++;
        return true;
    }
}