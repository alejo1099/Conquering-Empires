using System;
using UnityEngine;
using UnityEngine.AI;

namespace InteligenciaArtificialNPC
{
    public class MaquinaDeEstados
    {
        private struct VariablesMaquinaDeEstados
        {
            public Action OnPersecutionActivate;
            public Action OnPatrolActivate;
            public Patrulla patrulla;
            public Persecusion persecusion;
        }

        private VariablesMaquinaDeEstados variablesMaquinaDeEstados = new VariablesMaquinaDeEstados();
        public Action OnPersecutionActivate { get { return variablesMaquinaDeEstados.OnPersecutionActivate; } set { variablesMaquinaDeEstados.OnPersecutionActivate = value; } }
        public Action OnPatrolActivate { get { return variablesMaquinaDeEstados.OnPatrolActivate; } set { variablesMaquinaDeEstados.OnPatrolActivate = value; } }

        public MaquinaDeEstados(NavMeshAgent agenteSoldado, NavMeshObstacle agenteObstaculo, Transform ojos, LayerMask capasRaycast, Transform[] wayPoints, float rangoDeAtaque)
        {
            variablesMaquinaDeEstados.patrulla = new Patrulla(agenteSoldado, ojos, wayPoints, capasRaycast);
            variablesMaquinaDeEstados.patrulla.OnFoundObject += variablesMaquinaDeEstados.patrulla.EliminarDelegate;
            variablesMaquinaDeEstados.patrulla.OnFoundObject += ActivarPersecucion;

            variablesMaquinaDeEstados.persecusion = new Persecusion(agenteSoldado, agenteObstaculo, ojos, capasRaycast, rangoDeAtaque);
            variablesMaquinaDeEstados.persecusion.OnLostTarget += ActivarPatrulla;
        }

        public void ActivarPatrulla()
        {
            variablesMaquinaDeEstados.patrulla.IniciarPatrulla();
        }

        public void ActivarPersecucion()
        {
            variablesMaquinaDeEstados.persecusion.ObjetivoActual = variablesMaquinaDeEstados.patrulla.EnemigoDetectado;
            variablesMaquinaDeEstados.persecusion.IniciarPersecusion();
            if (OnPersecutionActivate != null)
                OnPersecutionActivate();
        }

        public void EliminarDelegate()
        {
            variablesMaquinaDeEstados.patrulla.EliminarDelegate();
            variablesMaquinaDeEstados.persecusion.EliminarDelegate();
        }
    }
}