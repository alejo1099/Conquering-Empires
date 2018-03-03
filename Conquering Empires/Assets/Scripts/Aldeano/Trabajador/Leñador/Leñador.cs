using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GameManager;
using InteligenciaArtificialNPC;

public class Leñador : Trabajador
{
    [Serializable]
    struct VariablesLeñador
    {
        internal Transform[] bosques;
        internal Transform arbolElegido;
        internal Transform posicionInstanciar;
        internal Transform bosqueElegido;
        public List<Transform> arbolesDisponibles;
        internal Rigidbody cuerpoTronco;

        internal Vector3 referenciaArbolAnterior;
        internal Vector3 rotarOjos;
        internal LayerMask capasEnvironmentArbol;
        internal bool arbolEncontrado;
        internal bool puedoActualizar;
        internal int flagCapacity;
    }
    VariablesLeñador variablesLeñador = new VariablesLeñador();

    private void Awake()
    {
        ObtenerComponentesPrincipales();
        variablesLeñador.posicionInstanciar = transform.GetChild(1);
        variablesLeñador.arbolesDisponibles = new List<Transform>();
    }

    private void Start()
    {
        PrimeraAsignacion();
        ActualizarDestinoAgente(GameManagement.gameManagement.ElegirAlmacenCercano(GameManagement.gameManagement.AlmacenesRecursos, TransformAgente.position, ref posicionObjetivo), 1.5f, 3);
        AsignacionEventos.VerificarAsignar(IrAlMapa);
    }

    private void PrimeraAsignacion()
    {
        variablesLeñador.capasEnvironmentArbol = GameManagement.gameManagement[3];
        transform.tag = "Aldeano/Trabajador/Leñador";
        variablesLeñador.bosques = ControladorLugares.controladorLugares.Bosques;
        variablesLeñador.rotarOjos = new Vector3(0, 60, 0);
    }

    public void IrAlMapa()
    {
        if (AlLlegarAlDestino(TransformAgente.position, posicionObjetivo, 4))
            ElegirBosque();
    }

    private void ElegirBosque()
    {
        variablesLeñador.bosqueElegido = ElegirElementoAlAzar(variablesLeñador.bosques);
        posicionObjetivo = variablesLeñador.bosqueElegido.position;
        ActualizarDestinoAgente(posicionObjetivo);
        AsignacionEventos.VerificarRemover(IrAlMapa);
        AsignacionEventos.VerificarAsignar(IrAlBosque);
        AsignacionEventos.VerificarAsignar(BuscarArbol);
    }

    private void IrAlBosque()
    {
        if (!variablesLeñador.arbolEncontrado)
        {
            if (AlLlegarAlDestino(TransformAgente.position, posicionObjetivo, 4))
            {
                DetenerAgente();
                AsignacionEventos.VerificarRemover(IrAlBosque);
            }
        }
        else if (variablesLeñador.arbolEncontrado)
        {
            AsignacionEventos.VerificarRemover(IrAlBosque);
        }
    }

    private void BuscarArbol()
    {
        if (BuscarObjetoSphereCast(0.4f, 20, variablesLeñador.capasEnvironmentArbol, ref variablesLeñador.arbolElegido, "Environment/Arbol", variablesLeñador.rotarOjos))
        {
            if (!variablesLeñador.arbolElegido.GetComponent<EstadoArbol>().ArbolCortado)
            {
                variablesLeñador.arbolElegido.GetComponent<EstadoArbol>().OnTreeCut += ActualizarArbolElegido;
                variablesLeñador.arbolEncontrado = true;
                variablesLeñador.referenciaArbolAnterior = variablesLeñador.arbolElegido.position;
                AlEncontrarElArbol();
            }
        }
    }

    private void AlEncontrarElArbol()
    {
        objetivoActualAgente = variablesLeñador.arbolElegido;
        posicionObjetivo = variablesLeñador.arbolElegido.position + RestaVectores(TransformAgente.position, variablesLeñador.arbolElegido.position, 2);
        ActualizarDestinoAgente(posicionObjetivo, 1.5f, 3);
        variablesLeñador.puedoActualizar = true;
        AsignacionEventos.VerificarRemover(BuscarArbol);
        AsignacionEventos.VerificarAsignar(IrHaciaElArbol);
    }

    private void IrHaciaElArbol()
    {
        if (AlLlegarAlDestino(TransformAgente.position, posicionObjetivo, 4))
        {
            DetenerAgente();
            AsignacionEventos.VerificarRemover(IrHaciaElArbol);
            variablesLeñador.puedoActualizar = false;
            StartCoroutine(TalarArbol());
        }
    }

    private IEnumerator TalarArbol()
    {
        ControladorSpawn.controladorSpawn.PoolingObjeto(variablesLeñador.posicionInstanciar, ref objetivoActualAgente, ControladorSpawn.controladorSpawn.variablesControladorSpawn.tronco, "Recurso/Madera");
        objetivoActualAgente.GetComponent<MeshRenderer>().enabled = false;
        while (variablesLeñador.flagCapacity < 10 && ObtenerRecurso(variablesLeñador.arbolElegido, objetivoActualAgente))
        {
            variablesLeñador.flagCapacity++;
            yield return new WaitForSeconds(1);
        }
        objetivoActualAgente.GetComponent<MeshRenderer>().enabled = true;
        variablesLeñador.flagCapacity = 0;
        BuscarMasArboles();
        ConvertirRecursoEnHijo();
    }

    private void BuscarMasArboles()
    {
        Collider[] collidersArboles = Physics.OverlapSphere(variablesLeñador.arbolElegido.position, 40, variablesLeñador.capasEnvironmentArbol, QueryTriggerInteraction.Collide);
        for (int i = 0; i < collidersArboles.Length; i++)
        {
            if (collidersArboles[i].transform != variablesLeñador.arbolElegido)
            {
                if (collidersArboles[i].transform.tag == "Environment/Arbol" && !variablesLeñador.arbolesDisponibles.Contains(collidersArboles[i].transform))
                    variablesLeñador.arbolesDisponibles.Add(collidersArboles[i].transform);
            }
        }
    }

    private void ConvertirRecursoEnHijo()
    {
        objetivoActualAgente.SetParent(TransformAgente);
        variablesLeñador.cuerpoTronco = objetivoActualAgente.transform.GetComponent<Rigidbody>();
        variablesLeñador.cuerpoTronco.isKinematic = true;
        LlevarTroncoAlmacen();
    }

    private void LlevarTroncoAlmacen()
    {
        ActualizarDestinoAgente(GameManagement.gameManagement.ElegirAlmacenCercano(GameManagement.gameManagement.AlmacenesRecursos, TransformAgente.position, ref posicionObjetivo), 1.5f, 3);
        variablesLeñador.puedoActualizar = false;
        AsignacionEventos.VerificarAsignar(IrAlmacen);
    }

    private void IrAlmacen()
    {
        objetivoActualAgente.localPosition = Vector3.MoveTowards(objetivoActualAgente.localPosition, new Vector3(-0.7f, 0, 0), Time.deltaTime);
        objetivoActualAgente.localRotation = Quaternion.Slerp(objetivoActualAgente.rotation, Quaternion.Euler(90f, 0, 0), Mathf.Lerp(0f, 1f, Time.time / 16f));
        if (AlLlegarAlDestino(TransformAgente.position, posicionObjetivo, 4))
        {
            DetenerAgente();
            Almacenar();
            AsignacionEventos.VerificarRemover(IrAlmacen);
        }
    }

    private void Almacenar()
    {
        variablesLeñador.cuerpoTronco.isKinematic = false;
        variablesLeñador.cuerpoTronco.transform.SetParent(null);
        OjosAgente.localRotation = Quaternion.identity;
        variablesLeñador.puedoActualizar = true;
        ElegirArbol();
    }

    private void ElegirArbol()
    {
        if (variablesLeñador.arbolElegido.GetComponent<EstadoArbol>().ArbolCortado)
        {
            ActualizarArbolElegido();
        }
        else
        {
            posicionObjetivo = variablesLeñador.arbolElegido.position + RestaVectores(TransformAgente.position, variablesLeñador.arbolElegido.position, 2);
            ActualizarDestinoAgente(posicionObjetivo, 1.5f, 3);
        }
        AsignacionEventos.VerificarAsignar(IrHaciaElArbol);
    }

    private void ActualizarArbolElegido()
    {
        if (variablesLeñador.puedoActualizar && variablesLeñador.arbolElegido.GetComponent<EstadoArbol>().ArbolCortado)
        {
            ElegirArbolCercano();
            posicionObjetivo = variablesLeñador.arbolElegido.position + RestaVectores(TransformAgente.position, variablesLeñador.arbolElegido.position, 2);
            ActualizarDestinoAgente(posicionObjetivo, 1.5f, 3);
        }
    }

    private void ElegirArbolCercano()
    {
        RemoverArbolesDesactivados();
        VerificarDistanciaMinimaObjeto(variablesLeñador.arbolesDisponibles, ref variablesLeñador.arbolElegido, variablesLeñador.referenciaArbolAnterior);
        variablesLeñador.referenciaArbolAnterior = variablesLeñador.arbolElegido.position;
        variablesLeñador.referenciaArbolAnterior.y = 0;
        variablesLeñador.arbolElegido.GetComponent<EstadoArbol>().OnTreeCut += ActualizarArbolElegido;
    }

    private void RemoverArbolesDesactivados()
    {
        for (int i = 0; i < variablesLeñador.arbolesDisponibles.Count; i++)
        {
            if (variablesLeñador.arbolesDisponibles[i].GetComponent<EstadoArbol>().ArbolCortado)
            {
                variablesLeñador.arbolesDisponibles.Remove(variablesLeñador.arbolesDisponibles[i]);
                --i;
            }
        }
    }

    public override void EliminarDelegate()
    {
        AsignacionEventos.VerificarRemover(IrHaciaElArbol);
        AsignacionEventos.VerificarRemover(IrAlmacen);
        AsignacionEventos.VerificarRemover(IrAlBosque);
        AsignacionEventos.VerificarRemover(BuscarArbol);
        AsignacionEventos.VerificarRemover(IrAlMapa);
    }

    void OnDisable()
    {
        EliminarDelegate();
        Destroy(this);
    }
}