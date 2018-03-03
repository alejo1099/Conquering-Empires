using UnityEngine;
using GameManager;

public class EleccionProfesionAldeano
{
    enum Profesion
    {
        SinElegir,
        Trabajador,
        Soldado
    }
    enum Profesiones
    {
        SinElegir,
        Leñador,
        Minero,
        Agricultor,
        Espadachin,
        Arquero,
        Piquero
    }
    struct VariablesEleccionProfesionaldeano
    {
        internal bool aldeanoEnPosicion;
        internal Profesion profesion;
        internal Profesiones profesiones;
    }

    VariablesEleccionProfesionaldeano variablesEleccionProfesionaldeano = new VariablesEleccionProfesionaldeano();

    public void VerificarInput()
    {
        if (variablesEleccionProfesionaldeano.aldeanoEnPosicion)
        {
            if (Input.GetKeyDown(KeyCode.T) && variablesEleccionProfesionaldeano.profesion == Profesion.SinElegir)
            {
                variablesEleccionProfesionaldeano.profesion = Profesion.Trabajador;
                ControladorTextos.controladorTextos.CambiarTextoEmplearAldeano = 2;
                if (ControladorPlayer.controladorPlayer.variablesPlayer.aldeanoEnfrente)
                    ControladorPlayer.controladorPlayer.variablesPlayer.aldeanoEnfrente.GetComponent<ControladorEstados>().EstadoTrabajar();
            }
            else if (Input.GetKeyDown(KeyCode.L) && variablesEleccionProfesionaldeano.profesion == Profesion.SinElegir)
            {
                variablesEleccionProfesionaldeano.profesion = Profesion.Soldado;
                ControladorTextos.controladorTextos.CambiarTextoEmplearAldeano = 3;
                if (ControladorPlayer.controladorPlayer.variablesPlayer.aldeanoEnfrente)
                    ControladorPlayer.controladorPlayer.variablesPlayer.aldeanoEnfrente.GetComponent<ControladorEstados>().EstadoLuchar();
            }
            VerificarInputSoldado();
            VerificarInputTrabajador();
        }
    }

    private void VerificarInputTrabajador()
    {
        if (variablesEleccionProfesionaldeano.profesion == Profesion.Trabajador)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                ControladorTextos.controladorTextos.CambiarTextoEmplearAldeano = 0;
                if (ControladorPlayer.controladorPlayer.variablesPlayer.aldeanoEnfrente)
                    ControladorPlayer.controladorPlayer.variablesPlayer.aldeanoEnfrente.GetComponent<ControladorTrabajo>().TrabajoMinero();
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                ControladorTextos.controladorTextos.CambiarTextoEmplearAldeano = 0;
                if (ControladorPlayer.controladorPlayer.variablesPlayer.aldeanoEnfrente)
                    ControladorPlayer.controladorPlayer.variablesPlayer.aldeanoEnfrente.GetComponent<ControladorTrabajo>().TrabajoLeñador();
            }/*else if (Input.GetKeyDown(KeyCode.F))
            {
                ControladorTextos.controladorTextos.CambiarTextoEmplearAldeano = 0;
                if (ControladorPlayer.controladorPlayer.variablesPlayer.aldeanoEnfrente)
                    ControladorPlayer.controladorPlayer.variablesPlayer.aldeanoEnfrente.GetComponent<ControladorTrabajo>().TrabajoAgricultor();
            }*/
        }
    }

    private void VerificarInputSoldado()
    {
        if (variablesEleccionProfesionaldeano.profesion == Profesion.Soldado)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ControladorTextos.controladorTextos.CambiarTextoEmplearAldeano = 0;
                if (ControladorPlayer.controladorPlayer.variablesPlayer.aldeanoEnfrente)
                    ControladorPlayer.controladorPlayer.variablesPlayer.aldeanoEnfrente.GetComponent<ControladorSoldado>().SoldadoEspadachin();
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                ControladorTextos.controladorTextos.CambiarTextoEmplearAldeano = 0;
                if (ControladorPlayer.controladorPlayer.variablesPlayer.aldeanoEnfrente)
                    ControladorPlayer.controladorPlayer.variablesPlayer.aldeanoEnfrente.GetComponent<ControladorSoldado>().SoldadoPiquero();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                ControladorTextos.controladorTextos.CambiarTextoEmplearAldeano = 0;
                if (ControladorPlayer.controladorPlayer.variablesPlayer.aldeanoEnfrente)
                    ControladorPlayer.controladorPlayer.variablesPlayer.aldeanoEnfrente.GetComponent<ControladorSoldado>().SoldadoArquero();
            }
        }
    }

    public void AsignarTextoInicial()
    {
        ControladorTextos.controladorTextos.CambiarTextoEmplearAldeano = 1;
        variablesEleccionProfesionaldeano.aldeanoEnPosicion = true;
    }

    private void AsignarTrabajador()
    {
        ControladorTextos.controladorTextos.CambiarTextoEmplearAldeano = 2;
    }

    private void AsignarSoldado()
    {
        ControladorTextos.controladorTextos.CambiarTextoEmplearAldeano = 3;
    }

    public void LimpiarTexto()
    {
        ControladorTextos.controladorTextos.CambiarTextoEmplearAldeano = 0;
        variablesEleccionProfesionaldeano.aldeanoEnPosicion = false;
        variablesEleccionProfesionaldeano.profesion = Profesion.SinElegir;
        variablesEleccionProfesionaldeano.profesiones = Profesiones.SinElegir;
    }
}