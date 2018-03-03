using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace InteligenciaArtificialNPC
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Aldeano<T> : Salud
    {
        private struct VariablesAldeano
        {
            public NavMeshAgent agente;
            public Transform transformAgente;
            public Transform ojosAgente;
            public Transform transformDeReferencia;
            public Transform[] arrayDevolver;
            public ArrayList listaTemporalTransforms;

            public LayerMask capasAldeano;
            public float tiempoAcumulado;
            public float tiempoTranscurrido;
        }

        private VariablesAldeano variablesAldeano = new VariablesAldeano();

        public Transform objetivoActualAgente;
        public Vector3 posicionObjetivo;
        private const float tiempoEntreActualizacion = 0.15f;

        public NavMeshAgent Agente { get { return variablesAldeano.agente; } set { variablesAldeano.agente = value; } }
        public Transform OjosAgente { get { return variablesAldeano.ojosAgente; } set { variablesAldeano.ojosAgente = value; } }
        public Transform TransformAgente { get { return variablesAldeano.transformAgente; } set { variablesAldeano.transformAgente = value; } }
        
        public LayerMask Capas { get { return variablesAldeano.capasAldeano; } set { variablesAldeano.capasAldeano = value; } }

        public virtual void ObtenerComponentesPrincipales()
        {
            TransformAgente = transform;
            Agente = GetComponent<NavMeshAgent>();
            OjosAgente = transform.GetChild(0);
        }

        public void ActualizarDestinoAgente(Vector3 destinoAgente, float velocidadAgente, float distanciaADetenerAgente)
        {
            variablesAldeano.agente.destination = destinoAgente;
            variablesAldeano.agente.speed = velocidadAgente;
            variablesAldeano.agente.stoppingDistance = distanciaADetenerAgente;
            variablesAldeano.agente.isStopped = false;
        }

        public void ActualizarDestinoAgente(Vector3 destinoAgente)
        {
            if (Time.time > variablesAldeano.tiempoTranscurrido + tiempoEntreActualizacion)
            {
                variablesAldeano.tiempoTranscurrido = Time.time;
                variablesAldeano.agente.destination = destinoAgente;
            }
        }

        public void DetenerAgente()
        {
            variablesAldeano.agente.isStopped = true;
            variablesAldeano.agente.speed = 0f;
        }

        public bool BuscarObjetoSphereCast(float radioEsfera, float rangoDeAlcanceCast, LayerMask capasVerificar, ref Transform receptorDePosicion, string tagVerificar, Vector3 anguloDeRotacion)
        {
            OjosAgente.localRotation = Quaternion.Slerp(Quaternion.Euler(anguloDeRotacion), Quaternion.Euler(-anguloDeRotacion), Mathf.PingPong(Time.time * 2f, 1f));
            RaycastHit InformacionGolpe;
            if (Physics.SphereCast(OjosAgente.position, radioEsfera, OjosAgente.forward, out InformacionGolpe, rangoDeAlcanceCast, capasVerificar, QueryTriggerInteraction.Ignore))
            {
                if (InformacionGolpe.transform.CompareTag(tagVerificar))
                {
                    receptorDePosicion = InformacionGolpe.transform;
                    return true;
                }
            }
            return false;
        }

        public bool BuscarObjetoSphereCast(Vector3 centroEsferaCast, float radioEsfera,
             Vector3 direccionSphereCast, float rangoDeAlcanceCast, LayerMask capasVerificar, ref Transform receptorDePosicion)
        {
            RaycastHit InformacionGolpe;
            if (Physics.SphereCast(centroEsferaCast, radioEsfera, direccionSphereCast, out InformacionGolpe, rangoDeAlcanceCast, capasVerificar, QueryTriggerInteraction.Ignore))
            {
                receptorDePosicion = InformacionGolpe.transform;
                return true;
            }
            return false;
        }

        public bool AlLlegarAlDestino()
        {
            if (Time.time > variablesAldeano.tiempoAcumulado + tiempoEntreActualizacion)
            {
                variablesAldeano.tiempoAcumulado = Time.time;
                if (!variablesAldeano.agente.pathPending && variablesAldeano.agente.remainingDistance <= variablesAldeano.agente.stoppingDistance)
                {
                    return true;
                }
            }
            return false;
        }

        public bool AlLlegarAlDestino(Vector3 posicionAgente, Vector3 posicionDestino, float distanciaADetenerse)
        {
            if (Time.time > variablesAldeano.tiempoAcumulado + tiempoEntreActualizacion)
            {
                variablesAldeano.tiempoAcumulado = Time.time;
                if (Vector3.Distance(posicionAgente, posicionDestino) <= distanciaADetenerse)
                {
                    return true;
                }
            }
            return false;
        }

        public void VerificarDistanciaMinimaObjeto(Transform[] arrayParaRevisar, ref Transform transformDeAlmacenamiento, Vector3 transformAVerificarDistancia)
        {
            float distanciaMinima = Mathf.Infinity;
            for (int i = 0; i < arrayParaRevisar.Length; i++)
            {
                if (Vector3.Distance(arrayParaRevisar[i].position, variablesAldeano.transformAgente.position) < distanciaMinima)
                {
                    distanciaMinima = Vector3.Distance(arrayParaRevisar[i].position, transformAVerificarDistancia);
                    transformDeAlmacenamiento = arrayParaRevisar[i];
                }
            }
        }

        public void VerificarDistanciaMinimaObjeto(List<Transform> arrayParaRevisar, ref Transform transformDeAlmacenamiento, Vector3 posicionAVerificarDistancia)
        {
            float distanciaMinima = Mathf.Infinity;
            for (int i = 0; i < arrayParaRevisar.Count; i++)
            {
                if (Vector3.Distance(arrayParaRevisar[i].position, variablesAldeano.transformAgente.position) < distanciaMinima)
                {
                    distanciaMinima = Vector3.Distance(arrayParaRevisar[i].position, posicionAVerificarDistancia);
                    transformDeAlmacenamiento = arrayParaRevisar[i];
                }
            }
        }

        public virtual void EliminarDelegate()
        {

        }

        public Transform[] ElegirTransformsMasCercanos(Transform[] arrayDeTransforms, Transform transformVerDistancia, int numeroDeElementosRandom = 3)
        {
            ArrayList listaTemporal = new ArrayList();
            variablesAldeano.listaTemporalTransforms = new ArrayList();

            for (int i = 0; i < arrayDeTransforms.Length; i++)
            {
                listaTemporal.Add(arrayDeTransforms[i]);
            }

            for (int l = 0; l < numeroDeElementosRandom; l++)
            {
                VerificarDistanciaMinimaObjeto(arrayDeTransforms, ref variablesAldeano.transformDeReferencia, transformVerDistancia.position);
                variablesAldeano.listaTemporalTransforms.Add(variablesAldeano.transformDeReferencia);
                for (int j = 0; j < arrayDeTransforms.Length; j++)
                {
                    if (arrayDeTransforms[j] == variablesAldeano.transformDeReferencia)
                    {
                        listaTemporal.Remove(variablesAldeano.transformDeReferencia);
                        arrayDeTransforms = new Transform[listaTemporal.Count];
                        for (int k = 0; k < listaTemporal.Count; k++)
                        {
                            arrayDeTransforms[k] = (Transform)listaTemporal[k];
                        }
                        break;
                    }
                }
            }
            variablesAldeano.arrayDevolver = new Transform[variablesAldeano.listaTemporalTransforms.Count];
            variablesAldeano.listaTemporalTransforms.CopyTo(variablesAldeano.arrayDevolver);
            return variablesAldeano.arrayDevolver;
        }

        public T ElegirElementoAlAzar(T[] objetos)
        {
            int k = Random.Range(0, objetos.Length);
            for (int i = 0; i < 10; i++)
            {
                k = Random.Range(0, objetos.Length);
            }
            return objetos[k];
        }

        public Vector3 RestaVectores(Vector3 posicionDireccion, Vector3 posicionCentro, int normalizar)
        {
            posicionDireccion.y = 0;
            posicionCentro.y = 0;
            Vector3 contenedorDistancia = (posicionDireccion - posicionCentro).normalized * normalizar;
            return contenedorDistancia;
        }

        public Vector3 RestaVectores(Vector3 posicionDireccion, Vector3 posicionCentro)
        {
            posicionDireccion.y = 0;
            posicionCentro.y = 0;
            Vector3 contenedorDistancia = (posicionDireccion - posicionCentro);
            return contenedorDistancia;
        }

        private void OnDestroy()
        {
            transform.tag = "Aldeano";
        }
    }
}