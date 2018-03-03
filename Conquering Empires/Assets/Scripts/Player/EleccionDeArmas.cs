using UnityEngine;
using GameManager;

//Script encargado de seleccionar el arma que escogio el usuario
public class EleccionDeArmas
{
    struct VariablesEleccionArmas
    {
        public Transform posicionArma;
    }

    VariablesEleccionArmas variablesEleccionArmas = new VariablesEleccionArmas();

    public EleccionDeArmas(Transform posicionArma)
    {
        this.variablesEleccionArmas.posicionArma = posicionArma;
    }

    public void ElegirArma()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && ControladorPlayer.controladorPlayer.armaElegida != ArmaElegida.Arco)
        {
            VerificarArma(SpawnerMilitar.spawnerMilitar.Arcos, ref ControladorPlayer.controladorPlayer.armaElegida, "Armas/Arco");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && ControladorPlayer.controladorPlayer.armaElegida != ArmaElegida.Espada)
        {
            VerificarArma(SpawnerMilitar.spawnerMilitar.Espadas, ref ControladorPlayer.controladorPlayer.armaElegida, "Armas/Espada");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && ControladorPlayer.controladorPlayer.armaElegida != ArmaElegida.Pica)
        {
            VerificarArma(SpawnerMilitar.spawnerMilitar.Picas, ref ControladorPlayer.controladorPlayer.armaElegida, "Armas/Pica");
        }
    }

    private void VerificarArma(GameObject[] armaElegidaPool, ref ArmaElegida armaElegida, string tagArmaEscogida)
    {
        EleccionArmaElegida(tagArmaEscogida, ref armaElegida);
        for (int i = 0; i < variablesEleccionArmas.posicionArma.childCount; i++)
        {
            variablesEleccionArmas.posicionArma.GetChild(i).gameObject.SetActive(false);
        }
        PoolingArma(armaElegidaPool, tagArmaEscogida);
    }

    private void PoolingArma(GameObject[] armaSeleccionada, string tagArmaEscogida)
    {
        SpawnerMilitar.spawnerMilitar.PoolingObjeto(variablesEleccionArmas.posicionArma, armaSeleccionada, variablesEleccionArmas.posicionArma);
        if (tagArmaEscogida == "Armas/Arco")
        {
            for (int i = 0; i < variablesEleccionArmas.posicionArma.childCount; i++)
            {
                if (variablesEleccionArmas.posicionArma.GetChild(i).tag == tagArmaEscogida)
                {
                    ControladorPlayer.controladorPlayer.PosicionFlecha = variablesEleccionArmas.posicionArma.GetChild(i).GetChild(0);
                }
            }
        }
    }

    private void EleccionArmaElegida(string tagArmaEscogida, ref ArmaElegida armaElegida)
    {
        switch (tagArmaEscogida)
        {
            case "Armas/Arco":
                armaElegida = ArmaElegida.Arco;
                break;
            case "Armas/Espada":
                armaElegida = ArmaElegida.Espada;
                break;
            case "Armas/Pica":
                armaElegida = ArmaElegida.Pica;
                break;
        }
    }
}