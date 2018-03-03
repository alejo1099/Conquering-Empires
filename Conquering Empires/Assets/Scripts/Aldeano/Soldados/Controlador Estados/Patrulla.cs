using System;
using UnityEngine;
using UnityEngine.AI;

namespace InteligenciaArtificialNPC
{
    public class Patrulla
    {
        private struct VariablesPatrulla
        {
            public Action OnFoundObject;
            public NavMeshAgent agente;
            public Transform[] wayPoints;
            public Transform ojos;
            public Transform enemigoDetectado;

            public LayerMask capasDeseadasRaycast;
            public Vector3 anguloRotacionOjos;
            public int wayPointActual;
        }
        private VariablesPatrulla variablesPatrulla = new VariablesPatrulla();
        
        public Action OnFoundObject { get { return variablesPatrulla.OnFoundObject; } set { variablesPatrulla.OnFoundObject = value; } }
        public Transform EnemigoDetectado { get { return variablesPatrulla.enemigoDetectado; } set { variablesPatrulla.enemigoDetectado = value; } }

        public Patrulla(NavMeshAgent agente, Transform ojos, Transform[] wayPoints, LayerMask capasDeseadasRaycast)
        {
            variablesPatrulla.agente = agente;
            variablesPatrulla.ojos = ojos;
            variablesPatrulla.wayPoints = wayPoints;
            variablesPatrulla.capasDeseadasRaycast = capasDeseadasRaycast;
            variablesPatrulla.anguloRotacionOjos = new Vector3(0, 60, 0);
        }

        public void IniciarPatrulla()
        {
            variablesPatrulla.agente.stoppingDistance = 3;
            ActualizarDestinoAgente(variablesPatrulla.wayPoints[variablesPatrulla.wayPointActual].position);
            AsignacionEventos.VerificarAsignar(RecorrerWayPoints);
            AsignacionEventos.VerificarAsignar(BuscarEnemigo);
        }

        private void RecorrerWayPoints()
        {
            if (!variablesPatrulla.agente.pathPending && variablesPatrulla.agente.remainingDistance < variablesPatrulla.agente.stoppingDistance)
            {
                variablesPatrulla.wayPointActual++;
                if (variablesPatrulla.wayPointActual == variablesPatrulla.wayPoints.Length)
                {
                    variablesPatrulla.wayPointActual = 0;
                }
                ActualizarDestinoAgente(variablesPatrulla.wayPoints[variablesPatrulla.wayPointActual].position);
            }
        }

        private void BuscarEnemigo()
        {
            variablesPatrulla.ojos.localRotation = Quaternion.Lerp(Quaternion.Euler(variablesPatrulla.anguloRotacionOjos), Quaternion.Euler(-variablesPatrulla.anguloRotacionOjos), Mathf.PingPong(Time.time * 0.5f, 1));
            RaycastHit infoGolpe;
            if (Physics.SphereCast(variablesPatrulla.ojos.position, 0.4f, variablesPatrulla.ojos.forward, out infoGolpe, 20, variablesPatrulla.capasDeseadasRaycast, QueryTriggerInteraction.Ignore))
            {
                EnemigoDetectado = infoGolpe.transform;
                if (OnFoundObject != null)
                    OnFoundObject();
            }
        }

        private void ActualizarDestinoAgente(Vector3 destino)
        {
            variablesPatrulla.agente.destination = destino;
        }

        public void EliminarDelegate()
        {
            AsignacionEventos.VerificarRemover(RecorrerWayPoints);
            AsignacionEventos.VerificarRemover(BuscarEnemigo);
        }
    }
}