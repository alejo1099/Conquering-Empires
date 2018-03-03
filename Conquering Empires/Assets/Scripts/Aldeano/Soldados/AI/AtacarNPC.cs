using UnityEngine;
using UnityEngine.AI;

namespace InteligenciaArtificialNPC
{
    public class AtacarNPC
    {
        public struct VariablesAtacarNPC
        {
            internal ControladorEstados controladorEstados;
            internal Soldado agenteSoldado;

            internal NavMeshAgent agente;
            internal NavMeshObstacle agenteObstaculo;

            internal Transform ojos;
            internal Transform transformAgente;

            internal LayerMask capasNPC;

            internal float rangoDeAtaque;
            internal float tiempoObstaculo;

            internal bool nPCLocalizado;
            internal bool seguirPlayer;
            internal bool obstaculoActivado;
            internal bool seguirObjetivo;
            internal bool atacarObjetivo;
        }

        public VariablesAtacarNPC variablesAtacarNPC = new VariablesAtacarNPC();

        public AtacarNPC(Transform ojosAgente, LayerMask capasNPC, float rangoDeAtaque, NavMeshAgent agente, NavMeshObstacle agenteObstaculo, Soldado tipoSoldado)
        {
            variablesAtacarNPC.ojos = ojosAgente;
            this.variablesAtacarNPC.capasNPC = capasNPC;
            this.variablesAtacarNPC.agente = agente;
            variablesAtacarNPC.transformAgente = agente.transform;
            this.variablesAtacarNPC.rangoDeAtaque = rangoDeAtaque;
            this.variablesAtacarNPC.agenteObstaculo = agenteObstaculo;
            variablesAtacarNPC.agenteSoldado = tipoSoldado;

        }

        public void BuscarCast()
        {
            if (!variablesAtacarNPC.agenteSoldado.Atacando && !variablesAtacarNPC.nPCLocalizado)
            {
                RaycastHit infoGolpe;
                if (Physics.SphereCast(variablesAtacarNPC.ojos.position, 0.4f, variablesAtacarNPC.ojos.forward, out infoGolpe, 20, variablesAtacarNPC.capasNPC, QueryTriggerInteraction.Ignore))
                {
                    variablesAtacarNPC.controladorEstados = infoGolpe.transform.GetComponent<ControladorEstados>();
                    variablesAtacarNPC.agenteSoldado.posicionObjetivo = infoGolpe.transform.position;
                    variablesAtacarNPC.nPCLocalizado = true;
                    variablesAtacarNPC.agenteSoldado.Atacando = true;
                }
            }
        }

        public void AtacarNPCs()
        {
            if (variablesAtacarNPC.nPCLocalizado)
            {
                if (!variablesAtacarNPC.obstaculoActivado && Vector3.Distance(variablesAtacarNPC.transformAgente.position, variablesAtacarNPC.agenteSoldado.posicionObjetivo) <= variablesAtacarNPC.rangoDeAtaque)
                {
                    variablesAtacarNPC.agente.speed = 0;
                    variablesAtacarNPC.agente.enabled = false;
                    variablesAtacarNPC.agenteObstaculo.enabled = true;
                    variablesAtacarNPC.obstaculoActivado = true;
                    variablesAtacarNPC.atacarObjetivo = true;
                }
                else if (variablesAtacarNPC.obstaculoActivado && Vector3.Distance(variablesAtacarNPC.transformAgente.position, variablesAtacarNPC.agenteSoldado.posicionObjetivo) > variablesAtacarNPC.rangoDeAtaque)
                {
                    variablesAtacarNPC.agenteObstaculo.enabled = false;
                    variablesAtacarNPC.tiempoObstaculo = Time.time;
                    variablesAtacarNPC.seguirObjetivo = true;
                    variablesAtacarNPC.obstaculoActivado = false;
                    variablesAtacarNPC.agenteSoldado.Atacando = false;
                    variablesAtacarNPC.atacarObjetivo = false;
                }
                else if (variablesAtacarNPC.seguirObjetivo && Time.time > variablesAtacarNPC.tiempoObstaculo + 0.2f)
                {
                    variablesAtacarNPC.seguirObjetivo = false;
                    variablesAtacarNPC.agente.enabled = true;
                    variablesAtacarNPC.agente.speed = 1.5f;
                }
                VerificarEstadoNPC();
                VerificarPosicionEnemigo();
            }
            else if (variablesAtacarNPC.seguirPlayer && Time.time > variablesAtacarNPC.tiempoObstaculo + 0.2f)
            {
                variablesAtacarNPC.agente.enabled = true;
                variablesAtacarNPC.agente.speed = 1.5f;
                variablesAtacarNPC.agenteSoldado.posicionObjetivo = GameManagement.gameManagement.variablesGameManagement.objetivoEnemigos.position;
                variablesAtacarNPC.agenteSoldado.DistanciaRelativaAlObjetivo = Vector3.zero;
                variablesAtacarNPC.seguirPlayer = false;
                variablesAtacarNPC.obstaculoActivado = false;
            }
        }

        private void VerificarPosicionEnemigo()
        {
            variablesAtacarNPC.agenteSoldado.DistanciaRelativaAlObjetivo = (variablesAtacarNPC.transformAgente.position - variablesAtacarNPC.agenteSoldado.posicionObjetivo).normalized * 2;
        }

        private void VerificarEstadoNPC()
        {
            if (variablesAtacarNPC.controladorEstados && variablesAtacarNPC.controladorEstados.variableSalud.cantidadVida <= 0)
            {
                variablesAtacarNPC.atacarObjetivo = false;
                variablesAtacarNPC.agenteObstaculo.enabled = false;
                variablesAtacarNPC.nPCLocalizado = false;
                variablesAtacarNPC.agenteSoldado.Atacando = false;
                variablesAtacarNPC.seguirPlayer = true;
                variablesAtacarNPC.tiempoObstaculo = Time.time;
            }
        }
    }
}