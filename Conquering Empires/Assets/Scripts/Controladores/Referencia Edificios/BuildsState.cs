using UnityEngine;
using GameManager;

public class BuildsState : Salud
{
    struct VariablesBuildsState
    {
        internal string nombreGameObject;
        internal bool reactivado;
    }

    VariablesBuildsState variablesBuildsState = new VariablesBuildsState();

    void Awake()
    {
        variableSalud.esteGameObject = gameObject;
        variablesBuildsState.nombreGameObject = variableSalud.esteGameObject.tag;
        CantidadSaludEdificio();
    }

    void OnEnable()
    {
        if (variablesBuildsState.reactivado)
        {
            CantidadSaludEdificio();
            AsignacionEventos.VerificarAsignar(VerificarVida);
        }
    }

    void Start()
    {
        AsignacionEventos.VerificarAsignar(VerificarVida);
    }

    private void CantidadSaludEdificio()
    {
        switch (variablesBuildsState.nombreGameObject)
        {
            case "Builds/Economia/Casa":
                variableSalud.cantidadVida = 200;
                break;
            case "Builds/Economia/AlmacenMadera":
                variableSalud.cantidadVida = 300;
                break;
            case "Builds/Economia/AlmacenPiedra":
                variableSalud.cantidadVida = 300;
                break;
            case "Arqueria":
                variableSalud.cantidadVida = 300;
                break;
            case "Cuartel":
                variableSalud.cantidadVida = 300;
                break;
            case "Muro":
                variableSalud.cantidadVida = 700;
                break;
            default:
                variableSalud.cantidadVida = 100;
                break;
        }
    }

    private void VerificarTag(string tag)
    {
        switch (tag)
        {
            case "ArmasEnemigo/Flecha":
                variableSalud.cantidadVida -= 5;
                break;
            case "ArmasEnemigo/Espada":
                variableSalud.cantidadVida -= 20;
                break;
            case "ArmasEnemigo/Pica":
                variableSalud.cantidadVida -= 10;
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        VerificarTag(other.transform.tag);
        ControladorAlmacen.controladorAlmacen.VerificarRecurso(other.transform);
    }

    void OnDisable()
    {
        ControladorEventos.controladorEventos.FuncionesActuales -= VerificarVida;
        variablesBuildsState.reactivado = true;
    }
}