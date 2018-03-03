using System;
using UnityEngine;
using GameManager;

public static class AsignacionEventos
{
    ///<Summary>
    ///Metodo para convertir en singleton, con claseSingleton como la variable de referencia
    ///clase referencia como la que se le va asignar a claseSingleton, y objetoAdestruir como el 
    ///objeto que sera destruido si la referencia ya existe
    ///</Summary>
    public static void ConvertirSingleton<T>(ref T claseSingleton, T claseReferencia, GameObject objetoADestruir)
    {
        if (claseSingleton == null)
            claseSingleton = claseReferencia;
        else
            UnityEngine.Object.Destroy(objetoADestruir);
    }

    ///<Summary>
    ///Metodo para verificar si un metodo ya ha sido agregado a un delegate, Action, etc
    ///</Summary>
    public static bool VerificarMetodo(Action metodo)
    {
        Delegate[] arrayMetodos = ControladorEventos.controladorEventos.FuncionesActuales.GetInvocationList();
        for (int i = 0; i < arrayMetodos.Length; i++)
        {
            if (arrayMetodos[i].Target == metodo.Target && arrayMetodos[i].GetHashCode() == metodo.GetHashCode())
            {
                return false;
            }
        }
        return true;
    }

    public static void AsignarMetodo(Action metodo)
    {
        ControladorEventos.controladorEventos.FuncionesActuales += metodo;
    }

    public static void RemoverMetodo(Action metodo)
    {
        ControladorEventos.controladorEventos.FuncionesActuales -= metodo;
    }

    public static void VerificarAsignar(Action metodo)
    {
        if (VerificarMetodo(metodo))
            AsignarMetodo(metodo);
    }

    public static void VerificarRemover(Action metodo)
    {
        if (!VerificarMetodo(metodo))
            RemoverMetodo(metodo);
    }
}