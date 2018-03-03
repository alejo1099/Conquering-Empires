using UnityEngine;
using GameManager;
using InteligenciaArtificialNPC;

public class ControladorEstados : Salud
{
    struct VariableControladorEstados
    {
        internal bool reactivado;
    }
    VariableControladorEstados variableControladorEstados = new VariableControladorEstados();

    void Awake()
    {
        variableSalud.esteGameObject = gameObject;
        variableSalud.cantidadVida = 20;
    }

    void OnEnable()
    {
        if (variableControladorEstados.reactivado)
        {
            variableSalud.cantidadVida = 20;
            ControladorEventos.controladorEventos.FuncionesActuales += VerificarVida;
        }
    }

    void Start()
    {
        ControladorEventos.controladorEventos.FuncionesActuales += VerificarVida;
    }

    public void EstadoTrabajar()
    {
        if (!GetComponent<ControladorTrabajo>())
            gameObject.AddComponent<ControladorTrabajo>();
    }

    public void EstadoLuchar()
    {
        if (!GetComponent<ControladorSoldado>())
            gameObject.AddComponent<ControladorSoldado>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "ArmasEnemigo/Flecha")
        {
            variableSalud.cantidadVida -= 5;
        }
        else if (other.transform.tag == "ArmasEnemigo/Espada")
        {
            variableSalud.cantidadVida -= 15;
        }
        else if (other.transform.tag == "ArmasEnemigo/Pica")
        {
            variableSalud.cantidadVida -= 10;
        }
    }

    void OnDisable()
    {
        ControladorEventos.controladorEventos.FuncionesActuales -= VerificarVida;
        variableControladorEstados.reactivado = true;
    }
}