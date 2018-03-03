using UnityEngine;
using UnityEngine.UI;
using GameManager;
public class ElegirEdificio
{
    enum Edificio
    {
        Economico,
        Militar
    }
    enum EdificiosMilitares
    {
        Arqueria,
        Cuartel,
        Castillo,
        Muro,
        Torre
    }

    enum EdificiosEconomicos
    {
        AlmacenMadera,
        AlmacenPiedra,
        Casa
    }

    EdificiosMilitares edificioMilitarSeleccionado;
    EdificiosEconomicos edificioEconomicoSeleccionado;
    private Edificio tipoDeEdificio;

    private ElegirLugarContruir elegirLugarConstruir;

    public Text textoEleccionEdificios;

    private bool seleccionarEdificio;

    public ElegirEdificio(ControladorPlayer controladorPlayer, Text textoEdificios, ElegirLugarContruir elegirLugarConstruir)
    {
        textoEleccionEdificios = textoEdificios;
        this.elegirLugarConstruir = elegirLugarConstruir;
    }

    public void SeleccionarEdificio()
    {
        if (Input.GetKeyDown(KeyCode.B) && !seleccionarEdificio)
        {
            textoEleccionEdificios.text = "Edificios Economicos: Presione 9 \nEdificios Militares:Presione 8 \nPara cancelar: Presione N";
            seleccionarEdificio = true;
        }
        else if (Input.GetKeyDown(KeyCode.N) && seleccionarEdificio)
        {
            textoEleccionEdificios.text = "Opciones Edificios \nPresiona B";
            seleccionarEdificio = false;
        }
        SeleccionarTipoEdificio();
        PosicionarEdificio();
    }

    private void SeleccionarTipoEdificio()
    {
        if (seleccionarEdificio)
        {
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                tipoDeEdificio = Edificio.Economico;
                textoEleccionEdificios.text = "Casa H: 10 Madera 5 Piedra \nAlmacenMadera M: 7 Madera 15 Piedra \nAlmacenPiedra P: 15 Madera 7 Piedra \nPara cancelar: Presione N";
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                tipoDeEdificio = Edificio.Militar;
                textoEleccionEdificios.text = "Arqueria Q: 20 Madera 12 Piedra \nCuartel E: 12 Madera 20 Piedra \nMuro M: 5 Madera 10 Piedra "/*\nTorre T: 15 Madera 30 Piedra*/ + "\nPara cancelar: Presione N";
            }
        }
    }

    private void PosicionarEdificio()
    {
        if (seleccionarEdificio)
        {

            if (tipoDeEdificio == Edificio.Economico && Input.GetKeyDown(KeyCode.H))
            {
                ConstruirEdificio(10, 5, SpawnBuilds.spawnBuilds.variablesSpawnBuilds.casa);
                GameManagement.gameManagement.AldeanosDisponibles++;
            }
            else if (tipoDeEdificio == Edificio.Economico && Input.GetKeyDown(KeyCode.M))
            {
                ConstruirEdificio(7, 15, SpawnBuilds.spawnBuilds.variablesSpawnBuilds.almacenMadera);
            }
            else if (tipoDeEdificio == Edificio.Economico && Input.GetKeyDown(KeyCode.P))
            {
                ConstruirEdificio(15, 7, SpawnBuilds.spawnBuilds.variablesSpawnBuilds.almacenPiedra);
            }
            else if (tipoDeEdificio == Edificio.Militar && Input.GetKeyDown(KeyCode.Q))
            {
                ConstruirEdificio(20, 12, SpawnBuilds.spawnBuilds.variablesSpawnBuilds.arqueria);
            }
            else if (tipoDeEdificio == Edificio.Militar && Input.GetKeyDown(KeyCode.E))
            {
                ConstruirEdificio(12, 20, SpawnBuilds.spawnBuilds.variablesSpawnBuilds.cuartel);
            }
            else if (tipoDeEdificio == Edificio.Militar && Input.GetKeyDown(KeyCode.M))
            {
                ConstruirEdificio(5, 10, SpawnBuilds.spawnBuilds.variablesSpawnBuilds.muro);
            }/*else if (tipoDeEdificio == Edificio.Militar && Input.GetKeyDown(KeyCode.T))
            {
                ConstruirEdificio(15, 30, SpawnBuilds.spawnBuilds.torre);
            }*/
        }
    }

    private void ConstruirEdificio(int numeroDeMadera, int numeroDePiedra, GameObject[] objetoConstruir)
    {
        if (IndicesEconomicos.indicesEconomicos.CantidadMadera >= numeroDeMadera && IndicesEconomicos.indicesEconomicos.CantidadPiedra >= numeroDePiedra)
        {
            if (elegirLugarConstruir.construccionDisponible)
            {
                elegirLugarConstruir.Construir(objetoConstruir);
                IndicesEconomicos.indicesEconomicos.CantidadMadera -= numeroDeMadera;
                IndicesEconomicos.indicesEconomicos.CantidadPiedra -= numeroDePiedra;
                ControladorAlmacen.controladorAlmacen.IndicesCanvasRecursos();
                GameManagement.gameManagement.CantidadEdificios++;
            }
        }
    }
    //Tamaño generico (11,5,12)
    //Tamaño muro (10,10,5)
    //Tamaño torre (10,11,10)
    //Tamaño castillo (30,11,30)
}