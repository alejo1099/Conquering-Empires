using System;
using UnityEngine;
using UnityEngine.AI;
using GameManager;

namespace InteligenciaArtificialNPC
{
    public class Persecusion
    {
        private struct VariablesPersecusion
        {
            public Action OnLostTarget;
            public Transform transformAgente;
            public Transform objetivoActual;
            public Transform ojos;
            public NavMeshAgent agente;
            public NavMeshObstacle agenteObstaculo;

            public LayerMask capasRayCast;
            public Vector3 posicionObjetivo;
            public Vector3 distanciaRelativaObjetivo;

            public float tiempoAcumulado;
            public float tiempoEsperaEncontrarObjetivo;
            public float rangoDeAtaque;

            public bool persiguiendoObjetivo;
            public bool viendoAlObjetivo;
            public bool obstaculoActivado;
            public bool agenteActivado;
        }

        private VariablesPersecusion variablesPersecusion = new VariablesPersecusion();
        private readonly float rangoDeAtaque;
        public Action OnLostTarget { get { return variablesPersecusion.OnLostTarget; } set { variablesPersecusion.OnLostTarget = value; } }
        public Transform ObjetivoActual { get { return variablesPersecusion.objetivoActual; } set { variablesPersecusion.objetivoActual = value; } }

        public Persecusion(NavMeshAgent agente, NavMeshObstacle agenteObstaculo, Transform ojos, LayerMask capasRayCast, float rangoDeAtaque)
        {
            variablesPersecusion.agente = agente;
            variablesPersecusion.ojos = ojos;
            variablesPersecusion.capasRayCast = capasRayCast;
            variablesPersecusion.rangoDeAtaque = rangoDeAtaque;
            this.rangoDeAtaque = rangoDeAtaque;
            variablesPersecusion.transformAgente = agente.transform;
        }

        public void IniciarPersecusion()
        {
            variablesPersecusion.agente.stoppingDistance = 0;
            variablesPersecusion.persiguiendoObjetivo = true;
            variablesPersecusion.viendoAlObjetivo = true;

            AsignacionEventos.VerificarAsignar(ActualizarDistanciaRelativa);
            AsignacionEventos.VerificarAsignar(PerseguirObjetivo);

            AsignacionEventos.VerificarAsignar(VerificarDistanciaObjetivo);
            AsignacionEventos.VerificarAsignar(VerificaRotacionOjos);
        }

        private void ActualizarDistanciaRelativa()
        {
            Vector3 direccion = variablesPersecusion.transformAgente.position;
            Vector3 centro = variablesPersecusion.objetivoActual.position;
            direccion.y = 0;
            centro.y = 0;
            variablesPersecusion.distanciaRelativaObjetivo = (direccion - centro).normalized * 1.75f;
        }

        private void PerseguirObjetivo()
        {
            if (variablesPersecusion.persiguiendoObjetivo && variablesPersecusion.agente.enabled == true)
            {
                variablesPersecusion.posicionObjetivo = variablesPersecusion.objetivoActual.position + variablesPersecusion.distanciaRelativaObjetivo;
                variablesPersecusion.posicionObjetivo.y = 0;
                ActualizarDestinoAgente(variablesPersecusion.posicionObjetivo);
            }
        }

        private void VerificarDistanciaObjetivo()
        {
            if (variablesPersecusion.viendoAlObjetivo && AlLlegarAlDestino(variablesPersecusion.rangoDeAtaque))
            {
                ActivarAgenteObstaculo();
            }
            else if (variablesPersecusion.viendoAlObjetivo && !AlLlegarAlDestino(variablesPersecusion.rangoDeAtaque))
            {
                DesactivarAgenteObstaculo();
            }
            else if (!variablesPersecusion.viendoAlObjetivo && !AlLlegarAlDestino(variablesPersecusion.rangoDeAtaque))
            {
                DesactivarAgenteObstaculo();
            }
            else
            {
                //ActivarAgenteObstaculo();
            }
        }

        private void ActivarAgenteObstaculo()
        {
            if (!variablesPersecusion.obstaculoActivado)
            {
                variablesPersecusion.obstaculoActivado = true;
                variablesPersecusion.agenteActivado = false;

                variablesPersecusion.agente.speed = 0;
                variablesPersecusion.agente.enabled = false;
                variablesPersecusion.agenteObstaculo.enabled = true;
                variablesPersecusion.agenteObstaculo.carving = true;
            }
        }

        private void DesactivarAgenteObstaculo()
        {
            if (variablesPersecusion.obstaculoActivado)
            {
                variablesPersecusion.obstaculoActivado = false;
                variablesPersecusion.agenteObstaculo.carving = false;
                variablesPersecusion.agenteObstaculo.enabled = false;
                ControladorEventos.controladorEventos.EjecutadorCorutinasTimer(ActivarAgente, 0.1f);
            }
        }

        private void ActivarAgente()
        {
            variablesPersecusion.agenteActivado = true;
            variablesPersecusion.agente.enabled = true;
            variablesPersecusion.agente.speed = 1.5f;
        }

        private void VerificaRotacionOjos()
        {
            variablesPersecusion.ojos.LookAt(variablesPersecusion.objetivoActual);
            if (variablesPersecusion.ojos.localRotation.eulerAngles.x < 60 && variablesPersecusion.ojos.localRotation.eulerAngles.x > -60)
            {
                VigilarPosicionObjetivo();
            }
        }

        private void VigilarPosicionObjetivo()
        {
            RaycastHit infoRayo;
            if (Physics.SphereCast(variablesPersecusion.ojos.position, 0.4f, variablesPersecusion.ojos.forward, out infoRayo, 20, variablesPersecusion.capasRayCast, QueryTriggerInteraction.Ignore))
            {
                if (infoRayo.transform == variablesPersecusion.objetivoActual)
                {
                    SeguirActualizandoObjetivo();
                }
                else
                {
                    if (variablesPersecusion.viendoAlObjetivo)
                    {
                        variablesPersecusion.viendoAlObjetivo = false;
                        variablesPersecusion.rangoDeAtaque = 1;
                    }
                }
            }
            else
            {
                VerificarPerdidaObjetivo();
            }
        }

        private void SeguirActualizandoObjetivo()
        {
            variablesPersecusion.tiempoEsperaEncontrarObjetivo = 0;
            if (!variablesPersecusion.viendoAlObjetivo)
            {
                variablesPersecusion.viendoAlObjetivo = true;
                variablesPersecusion.rangoDeAtaque = rangoDeAtaque;
            }

            if (!variablesPersecusion.persiguiendoObjetivo)
            {
                variablesPersecusion.persiguiendoObjetivo = true;
            }
        }

        private void VerificarPerdidaObjetivo()
        {
            variablesPersecusion.tiempoEsperaEncontrarObjetivo += Time.fixedDeltaTime;
            if (variablesPersecusion.viendoAlObjetivo)
            {
                variablesPersecusion.viendoAlObjetivo = false;
                variablesPersecusion.rangoDeAtaque = 1;
            }

            if (variablesPersecusion.persiguiendoObjetivo)
            {
                variablesPersecusion.persiguiendoObjetivo = false;
            }
            if (variablesPersecusion.tiempoEsperaEncontrarObjetivo > 8)
            {
                LimpiarVariables();
                OnLostTarget();
            }
        }

        public void LimpiarVariables()
        {
            variablesPersecusion.viendoAlObjetivo = false;
            variablesPersecusion.persiguiendoObjetivo = true;
            variablesPersecusion.tiempoEsperaEncontrarObjetivo = 0;

            AsignacionEventos.VerificarRemover(PerseguirObjetivo);
            AsignacionEventos.VerificarRemover(VerificarDistanciaObjetivo);
            AsignacionEventos.VerificarRemover(VerificaRotacionOjos);
            AsignacionEventos.VerificarRemover(ActualizarDistanciaRelativa);
        }

        private bool AlLlegarAlDestino(float distanciaADetenerse)
        {
            if (Time.time > variablesPersecusion.tiempoAcumulado + 0.15f)
            {
                variablesPersecusion.tiempoAcumulado = Time.time;
                if (Vector3.Distance(variablesPersecusion.transformAgente.position, variablesPersecusion.posicionObjetivo) <= distanciaADetenerse)
                {
                    return true;
                }
            }
            return false;
        }

        private void ActualizarDestinoAgente(Vector3 destino)
        {
            variablesPersecusion.agente.destination = destino;
        }

        public void EliminarDelegate()
        {
            AsignacionEventos.VerificarRemover(PerseguirObjetivo);
            AsignacionEventos.VerificarRemover(VerificarDistanciaObjetivo);
            AsignacionEventos.VerificarRemover(VerificaRotacionOjos);
            AsignacionEventos.VerificarRemover(ActualizarDistanciaRelativa);
        }
    }
}