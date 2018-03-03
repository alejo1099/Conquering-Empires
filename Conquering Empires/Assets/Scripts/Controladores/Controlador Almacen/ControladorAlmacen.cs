using UnityEngine;
using UnityEngine.UI;

public class ControladorAlmacen : MonoBehaviour
{
    public static ControladorAlmacen controladorAlmacen;

    private void Awake()
    {
        AsignacionEventos.ConvertirSingleton<ControladorAlmacen>(ref controladorAlmacen, this, gameObject);
    }

    public void IndicesCanvasRecursos()
    {
        IndicesEconomicos.indicesEconomicos.ActualizarRecursos();
    }

    void AlmacenarAlimento(Transform alimento)
    {
        //IndicesEconomicos.indicesEconomicos.CantidadMadera += alimento.GetComponent<ValorRecurso>().CantidadRecurso;
        alimento.SetParent(transform.GetChild(3));
        alimento.gameObject.SetActive(false);
    }

    void AlmacenarMadera(Transform madera)
    {
        IndicesEconomicos.indicesEconomicos.CantidadMadera += madera.GetComponent<ValorRecurso>().CantidadRecurso;
        madera.SetParent(transform.GetChild(1));
        madera.gameObject.SetActive(false);
    }

    void AlamacenarPiedra(Transform piedra)
    {
        IndicesEconomicos.indicesEconomicos.CantidadPiedra += piedra.GetComponent<ValorRecurso>().CantidadRecurso;
        piedra.SetParent(transform.GetChild(2));
        piedra.gameObject.SetActive(false);
    }

    public void VerificarRecurso(Transform Recurso)
    {
        switch (Recurso.tag)
        {
            case "Recuro/Alimento":
                AlmacenarAlimento(Recurso);
                break;
            case "Recurso/Madera":
                AlmacenarMadera(Recurso);
                break;
            case "Recurso/Piedra":
                AlamacenarPiedra(Recurso);
                break;
            default:
                break;
        }
        IndicesEconomicos.indicesEconomicos.ActualizarRecursos();
    }
}