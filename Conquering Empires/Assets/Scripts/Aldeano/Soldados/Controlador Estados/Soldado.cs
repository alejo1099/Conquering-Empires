using UnityEngine;
using UnityEngine.AI;
using GameManager;

namespace InteligenciaArtificialNPC
{
    [RequireComponent(typeof(NavMeshObstacle))]
    public class Soldado : Aldeano<Transform>
    {
        protected struct VariablesSoldado
        {
            public MaquinaDeEstados maquinaDeEstados;
            public NavMeshObstacle agenteObstaculo;
            public Transform posicionArma;
            public Transform[] wayPoints;

            public Vector3 distanciaRelativaAlObjetivo;
            public float rangoDeAtaque;
            public bool atacando;
        }
        protected VariablesSoldado variablesSoldado = new VariablesSoldado();

        public MaquinaDeEstados MaquinaDeEstados { get { return variablesSoldado.maquinaDeEstados; } set { variablesSoldado.maquinaDeEstados = value; } }
        public Transform PosicionArma { get { return variablesSoldado.posicionArma; } set { variablesSoldado.posicionArma = value; } }
        protected NavMeshObstacle AgenteObstaculo { get { return variablesSoldado.agenteObstaculo; } set { variablesSoldado.agenteObstaculo = value; } }
        public Transform[] WayPoints { get { return variablesSoldado.wayPoints; } set { variablesSoldado.wayPoints = value; } }

        public Vector3 DistanciaRelativaAlObjetivo { get { return variablesSoldado.distanciaRelativaAlObjetivo; } set { variablesSoldado.distanciaRelativaAlObjetivo = value; } }
        public float RangoDeAtaque { get { return variablesSoldado.rangoDeAtaque; } set { variablesSoldado.rangoDeAtaque = value; } }
        public bool Atacando { get { return variablesSoldado.atacando; } set { variablesSoldado.atacando = value; } }

        public override void ObtenerComponentesPrincipales()
        {
            base.ObtenerComponentesPrincipales();
            AgenteObstaculo = GetComponent<NavMeshObstacle>();
            AgenteObstaculo.enabled = false;
            PosicionArma = TransformAgente.GetChild(2);
        }

        public virtual void IrPorArma()
        {
            ActualizarDestinoAgente(GameManagement.gameManagement.ElegirAlmacenCercano(GameManagement.gameManagement.AlmacenesArmas, TransformAgente.position, ref posicionObjetivo), 1.5f, 3);
            posicionObjetivo.y = 0;
            ActualizarDestinoAgente(posicionObjetivo, 1.5f, 3);
        }

        public override void EliminarDelegate()
        {
            variablesSoldado.maquinaDeEstados.EliminarDelegate();
        }

        public void DesactivarArma()
        {
            for (int i = 0; i < variablesSoldado.posicionArma.childCount; i++)
            {
                variablesSoldado.posicionArma.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}