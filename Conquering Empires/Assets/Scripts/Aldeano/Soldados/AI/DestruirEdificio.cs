using UnityEngine;
using UnityEngine.AI;

namespace InteligenciaArtificialNPC
{
    public class DestruirEdificio
    {
        public struct VariablesDestruirEdificio
        {
            internal BuildsState buildsState;
            internal Soldado agenteSoldado;

            internal NavMeshAgent agente;
            internal NavMeshObstacle agenteObstaculo;

            internal Transform ojos;
            internal Transform transformAgente;

            internal LayerMask capasEdificios;

            internal float rangoDeAtaque;
            internal float tiempoObstaculo;

            internal bool edificioLocalizado;
            internal bool seguirPlayer;
            internal bool obstaculoActivado;
            internal bool atacarObjetivo;
        }

        public VariablesDestruirEdificio variablesDestruirEdificio = new VariablesDestruirEdificio();

        public DestruirEdificio(Transform ojosAgente, LayerMask capasEdificios, float rangoDeAtaque, NavMeshAgent agente, NavMeshObstacle agenteObstaculo, Soldado tipoSoldado)
        {
            variablesDestruirEdificio.ojos = ojosAgente;
            this.variablesDestruirEdificio.capasEdificios = capasEdificios;
            this.variablesDestruirEdificio.agente = agente;
            variablesDestruirEdificio.transformAgente = agente.transform;
            this.variablesDestruirEdificio.rangoDeAtaque = rangoDeAtaque;
            this.variablesDestruirEdificio.agenteObstaculo = agenteObstaculo;
            variablesDestruirEdificio.agenteSoldado = tipoSoldado;
        }

        public void BuscarCast()
        {
            if (!variablesDestruirEdificio.agenteSoldado.Atacando && !variablesDestruirEdificio.edificioLocalizado)
            {
                RaycastHit infoGolpe;
                if (Physics.SphereCast(variablesDestruirEdificio.ojos.position, 0.4f, variablesDestruirEdificio.ojos.forward, out infoGolpe, 20, variablesDestruirEdificio.capasEdificios, QueryTriggerInteraction.Ignore))
                {
                    Vector3 puntoDeImpacto = infoGolpe.point;
                    puntoDeImpacto.y = 0;
                    Vector3 distanciaHaciaObjetivo = (variablesDestruirEdificio.transformAgente.position - puntoDeImpacto).normalized * 1.75f;
                    variablesDestruirEdificio.agenteSoldado.posicionObjetivo = infoGolpe.point;
                    variablesDestruirEdificio.agenteSoldado.DistanciaRelativaAlObjetivo = (variablesDestruirEdificio.transformAgente.position - distanciaHaciaObjetivo).normalized * variablesDestruirEdificio.rangoDeAtaque;

                    variablesDestruirEdificio.buildsState = infoGolpe.transform.GetComponent<BuildsState>();
                    variablesDestruirEdificio.edificioLocalizado = true;
                    variablesDestruirEdificio.agenteSoldado.Atacando = true;
                }
            }
        }

        public void AtacarEdificio()
        {
            if (variablesDestruirEdificio.edificioLocalizado)
            {
                if (!variablesDestruirEdificio.obstaculoActivado && Vector3.Distance(variablesDestruirEdificio.transformAgente.position, variablesDestruirEdificio.agenteSoldado.posicionObjetivo) <= variablesDestruirEdificio.rangoDeAtaque)
                {
                    variablesDestruirEdificio.agente.speed = 0;
                    variablesDestruirEdificio.agente.enabled = false;
                    variablesDestruirEdificio.agenteObstaculo.enabled = true;
                    variablesDestruirEdificio.obstaculoActivado = true;
                    variablesDestruirEdificio.atacarObjetivo = true;
                }
                VerificarEstadoEdificio();
            }
            else if (variablesDestruirEdificio.seguirPlayer && Time.time > variablesDestruirEdificio.tiempoObstaculo + 0.2f)
            {
                variablesDestruirEdificio.agente.enabled = true;
                variablesDestruirEdificio.agente.speed = 1.5f;
                variablesDestruirEdificio.agenteSoldado.posicionObjetivo = GameManagement.gameManagement.variablesGameManagement.objetivoEnemigos.position;
                variablesDestruirEdificio.agenteSoldado.DistanciaRelativaAlObjetivo = Vector3.zero;
                variablesDestruirEdificio.seguirPlayer = false;
                variablesDestruirEdificio.obstaculoActivado = false;
            }
        }

        private void VerificarEstadoEdificio()
        {
            if (variablesDestruirEdificio.buildsState && variablesDestruirEdificio.buildsState.variableSalud.cantidadVida <= 0)
            {
                variablesDestruirEdificio.atacarObjetivo = false;
                variablesDestruirEdificio.agenteObstaculo.enabled = false;
                variablesDestruirEdificio.edificioLocalizado = false;
                variablesDestruirEdificio.agenteSoldado.Atacando = false;
                variablesDestruirEdificio.seguirPlayer = true;
                variablesDestruirEdificio.tiempoObstaculo = Time.time;
            }
        }
    }
}