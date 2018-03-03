using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorMenuPrincipal : MonoBehaviour
{
    public void CargarEscena()
    {
		SceneManager.LoadScene("EscenaUno");
    }

    public void SalirJuego()
    {
        Application.Quit();
    }
}