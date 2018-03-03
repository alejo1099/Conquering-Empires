using UnityEngine;
using GameManager;

public class ControladorEnemigos : Salud
{
    struct VariablesControladorEnemigos
    {
        internal MeshRenderer mallaAgente;
        internal bool reactivado;
    }

    VariablesControladorEnemigos variablesControladorEnemigos = new VariablesControladorEnemigos();

    void Awake()
    {
        variableSalud.esteGameObject = gameObject;
        variablesControladorEnemigos.mallaAgente = GetComponent<MeshRenderer>();
    }

    void OnEnable()
    {
        int eleccionAzar = Random.Range(1, 4);
        switch (eleccionAzar)
        {
            case 1:
                variableSalud.esteGameObject.AddComponent<PiqueroEnemigo>();
                variableSalud.cantidadVida = 200;
                variablesControladorEnemigos.mallaAgente.material.color = Color.blue;
                break;
            case 2:
                variableSalud.esteGameObject.AddComponent<ArqueroEnemigo>();
                variableSalud.cantidadVida = 150;
                variablesControladorEnemigos.mallaAgente.material.color = Color.green;
                break;
            case 3:
                variableSalud.esteGameObject.AddComponent<EspadachinEnemigo>();
                variableSalud.cantidadVida = 250;
                variablesControladorEnemigos.mallaAgente.material.color = Color.red;
                break;
        }
        if (variablesControladorEnemigos.reactivado) ControladorEventos.controladorEventos.FuncionesActuales += VerificarVida;
    }

    void Start()
    {
        ControladorEventos.controladorEventos.FuncionesActuales += VerificarVida;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Armas/Flecha")
        {
            variableSalud.cantidadVida -= 5;
        }
        else if (other.transform.tag == "Armas/Espada")
        {
            variableSalud.cantidadVida -= 15;
        }
        else if (other.transform.tag == "Armas/Pica")
        {
            variableSalud.cantidadVida -= 10;
        }
    }

    void OnDisable()
    {
        variablesControladorEnemigos.reactivado = true;
        ControladorEventos.controladorEventos.FuncionesActuales -= VerificarVida;
    }
}
