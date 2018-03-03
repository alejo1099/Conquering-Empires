using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameManager;
using Spawner;

public class GameManagement : MonoBehaviour
{
    [Serializable]
    public struct VariablesGameManagement
    {
        public Transform objetivoEnemigos;
        public Transform posicionArmamento;

        //public Text textoPropiedades;

        public LayerMask[] capasUtiles;

        public List<Transform> arrayEdificios;
        // public List<Transform> almacenesDeMadera;
        // public List<Transform> almacenesDePiedra;
        public List<Transform> almacenesRecursos;
        public List<Transform> almacenesArmas;

        internal Vector3 posicionMenor;

        internal int cantidadAldeanos;
        internal int aldeanosDisponibles;
        internal int cantidadEdificios;
        internal int poblacionDisponible;
    }

    public VariablesGameManagement variablesGameManagement = new VariablesGameManagement();
    public static GameManagement gameManagement;

    public int CantidadAldeanos
    {
        get
        {
            return variablesGameManagement.cantidadAldeanos;
        }
        set
        {
            variablesGameManagement.cantidadAldeanos = value;
            ControladorTextos.controladorTextos.ActualizarTextoEdificio();
        }
    }
    public int CantidadEdificios
    {
        get
        {
            return variablesGameManagement.cantidadEdificios;
        }
        set
        {
            variablesGameManagement.cantidadEdificios = value;
            // ActualizarEdificios(SpawnBuilds.spawnBuilds.variablesSpawnBuilds.almacenMadera, variablesGameManagement.almacenesDeMadera);
            // ActualizarEdificios(SpawnBuilds.spawnBuilds.variablesSpawnBuilds.almacenPiedra, variablesGameManagement.almacenesDePiedra);

            ControladorTextos.controladorTextos.ActualizarTextoEdificio();
        }
    }
    public int AldeanosDisponibles { get { return variablesGameManagement.aldeanosDisponibles; } set { variablesGameManagement.aldeanosDisponibles = value * 5; } }
    public List<Transform> AlmacenesRecursos
    {
        get
        {
            EliminarDesactivados(variablesGameManagement.almacenesRecursos);
            return variablesGameManagement.almacenesRecursos;
        }
    }
    public LayerMask this[int index] { get { return variablesGameManagement.capasUtiles[index]; } }
    public List<Transform> AlmacenesArmas
    {
        get
        {
            EliminarDesactivados(variablesGameManagement.almacenesArmas);
            return variablesGameManagement.almacenesArmas;
        }
    }

    private void Awake()
    {
        AsignacionEventos.ConvertirSingleton<GameManagement>(ref gameManagement, this, gameObject);
        variablesGameManagement.arrayEdificios = new List<Transform>();
        //variablesGameManagement.almacenesRecursos = new List<Transform>();
        //variablesGameManagement.almacenesDeMadera = new List<Transform>();
        //variablesGameManagement.almacenesDePiedra = new List<Transform>();
        variablesGameManagement.cantidadAldeanos = 10;
        variablesGameManagement.aldeanosDisponibles = 10;
    }

    private void ActualizarEdificios(GameObject[] arrayBuscar, List<Transform> listaAgregar)
    {
        for (int i = 0; i < arrayBuscar.Length; i++)
        {
            if (arrayBuscar[i].activeInHierarchy)
            {
                listaAgregar.Add(arrayBuscar[i].transform);
            }
        }
    }

    private void EliminarDesactivados(List<Transform> listaReducir)
    {
        for (int i = 0; i < listaReducir.Count; i++)
        {
            if (!listaReducir[i].gameObject.activeInHierarchy)
            {
                listaReducir.Remove(listaReducir[i]);
                i--;
            }
        }
    }

    public Vector3 ElegirAlmacenCercano(List<Transform> listaAlmacen, Vector3 posicionTrabajador)
    {
        float distanciaMenor = Mathf.Infinity;
        for (int i = 0; i < listaAlmacen.Count; i++)
        {
            if (Vector3.Distance(posicionTrabajador, listaAlmacen[i].position) <= distanciaMenor)
            {
                distanciaMenor = Vector3.Distance(posicionTrabajador, listaAlmacen[i].position);
                variablesGameManagement.posicionMenor = listaAlmacen[i].position;
            }
        }
        return variablesGameManagement.posicionMenor;
    }

    public Vector3 ElegirAlmacenCercano(List<Transform> listaAlmacen, Vector3 posicionTrabajador, ref Vector3 referenciaDestino)
    {
        float distanciaMenor = Mathf.Infinity;
        for (int i = 0; i < listaAlmacen.Count; i++)
        {
            if (Vector3.Distance(posicionTrabajador, listaAlmacen[i].position) <= distanciaMenor)
            {
                distanciaMenor = Vector3.Distance(posicionTrabajador, listaAlmacen[i].position);
                variablesGameManagement.posicionMenor = listaAlmacen[i].GetChild(0).position;
            }
        }
        variablesGameManagement.posicionMenor.y = 0;
        referenciaDestino = variablesGameManagement.posicionMenor;
        //referenciaDestino.y = 0;
        return variablesGameManagement.posicionMenor;
    }
}