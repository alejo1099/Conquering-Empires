using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameManager;
using System;

//Estructura usada para elegir el tipo de arma
public enum ArmaElegida
{
    Sinelegir,
    Arco,
    Espada,
    Pica
}
//Script encargado de administrar y ejecutar todas las mecanicas del player
[RequireComponent(typeof(Rigidbody))]
public class ControladorPlayer : Salud
{
    //Estructura que guarda los valores iniciales
    [Serializable]
    public struct VariablesPlayer
    {
        internal MovimientoRotacion moverYRotar;
        internal EleccionDeArmas eleccionDeArmas;
        internal AtaquePlayer ataquePlayer;
        internal ElegirLugarContruir elegirLugarConstruir;
        internal ElegirEdificio elegirEdificio;
        internal EleccionProfesionAldeano eleccionProfesionAldeano;

        internal Transform posicionArma;
        internal Transform Player;
        internal Transform CamaraTransform;
        internal Transform posicionFlecha;
        internal Transform aldeanoEnfrente;

        public LayerMask capasDeseadas;
        public LayerMask capasOverlap;
        public LayerMask capasRayCcast;

        internal LineRenderer lineRendererPlayer;
        internal Camera ReferenciaCamara;
        public Text textoEdificios;

        public float VelocidadMovimiento;
        public float VelocidadRotacion;
        internal string[] Tags;
        internal bool aldeanoEnRango;
    }

    public VariablesPlayer variablesPlayer = new VariablesPlayer();
    public ArmaElegida armaElegida = ArmaElegida.Sinelegir;

    public static ControladorPlayer controladorPlayer;

    public Transform PosicionFlecha
    {
        get
        {
            return variablesPlayer.posicionFlecha;
        }
        set
        {
            variablesPlayer.posicionFlecha = value;
        }
    }

    void Awake()
    {
        ObtenerReferencias();
        CrearInstancias();
        //Variables de salud
        variableSalud.esteGameObject = gameObject;
        variableSalud.cantidadVida = 100;
    }

    void Start()
    {
        AsignarTags();
        EjecutarMetodos();
    }

    private void ObtenerReferencias()
    {
        //Singleton
        AsignacionEventos.ConvertirSingleton<ControladorPlayer>(ref controladorPlayer, this, gameObject);

        variablesPlayer.ReferenciaCamara = GetComponentInChildren<Camera>();
        variablesPlayer.CamaraTransform = variablesPlayer.ReferenciaCamara.transform;
        variablesPlayer.Player = transform;
        variablesPlayer.lineRendererPlayer = GetComponent<LineRenderer>();
        variablesPlayer.posicionArma = transform.GetChild(0).GetChild(transform.GetChild(0).childCount - 1);
    }

    private void CrearInstancias()
    {
        variablesPlayer.eleccionDeArmas = new EleccionDeArmas(variablesPlayer.posicionArma);
        variablesPlayer.ataquePlayer = new AtaquePlayer(variablesPlayer.capasDeseadas, variablesPlayer.posicionArma, variablesPlayer.ReferenciaCamara);
        variablesPlayer.elegirLugarConstruir = new ElegirLugarContruir(variablesPlayer.CamaraTransform,
        variablesPlayer.posicionArma, variablesPlayer.capasOverlap, variablesPlayer.lineRendererPlayer, variablesPlayer.capasRayCcast);
        variablesPlayer.elegirEdificio = new ElegirEdificio(this, variablesPlayer.textoEdificios, variablesPlayer.elegirLugarConstruir);
        variablesPlayer.moverYRotar = new MovimientoRotacion(variablesPlayer.Player, variablesPlayer.CamaraTransform, variablesPlayer.VelocidadMovimiento, variablesPlayer.VelocidadRotacion);
        variablesPlayer.eleccionProfesionAldeano = new EleccionProfesionAldeano();
    }

    private void EjecutarMetodos()
    {
        ControladorEventos.controladorEventos.FuncionesActuales += VerificarVida;
        ControladorEventos.controladorEventos.FuncionesActuales += variablesPlayer.ataquePlayer.DispararFlecha;
        ControladorEventos.controladorEventos.FuncionesActuales += variablesPlayer.ataquePlayer.AtacarEspada;
        ControladorEventos.controladorEventos.FuncionesActuales += variablesPlayer.ataquePlayer.AtacarPica;
        ControladorEventos.controladorEventos.FuncionesActuales += variablesPlayer.eleccionDeArmas.ElegirArma;
        ControladorEventos.controladorEventos.FuncionesActuales += variablesPlayer.elegirLugarConstruir.DispararLinea;
        ControladorEventos.controladorEventos.FuncionesActuales += variablesPlayer.elegirEdificio.SeleccionarEdificio;
        ControladorEventos.controladorEventos.FuncionesActuales += variablesPlayer.moverYRotar.MovimientoRigidbody;
        ControladorEventos.controladorEventos.FuncionesActuales += variablesPlayer.moverYRotar.RotacionRigidbody;
        ControladorEventos.controladorEventos.FuncionesActuales += variablesPlayer.eleccionProfesionAldeano.VerificarInput;
    }

    private void AsignarTags()
    {
        variablesPlayer.Tags = new string[7];
        variablesPlayer.Tags[0] = "Aldeano";
        variablesPlayer.Tags[1] = "Aldeano/Trabajador/Agricultor";
        variablesPlayer.Tags[2] = "Aldeano/Trabajador/Leñador";
        variablesPlayer.Tags[3] = "Aldeano/Trabajador/Minero";
        variablesPlayer.Tags[4] = "Aldeano/Soldado/Espadachin";
        variablesPlayer.Tags[5] = "Aldeano/Soldado/Arquero";
        variablesPlayer.Tags[6] = "Aldeano/Soldado/Piquero";
    }

    void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Aldeano") && !variablesPlayer.aldeanoEnRango)
        {
            variablesPlayer.eleccionProfesionAldeano.AsignarTextoInicial();
            variablesPlayer.aldeanoEnRango = true;
            variablesPlayer.aldeanoEnfrente = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (variablesPlayer.aldeanoEnRango)
        {
            for (int i = 0; i < variablesPlayer.Tags.Length; i++)
            {
                if (other.transform.tag == variablesPlayer.Tags[i])
                {
                    variablesPlayer.eleccionProfesionAldeano.LimpiarTexto();
                    variablesPlayer.aldeanoEnRango = false;
                    variablesPlayer.aldeanoEnfrente = null;
                    break;
                }
            }
        }
    }
}