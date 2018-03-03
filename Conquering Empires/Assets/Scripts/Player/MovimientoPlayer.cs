using System.Collections;
using UnityEngine;

public class MovimientoPlayer : MonoBehaviour
{
    private Transform esteTransform;

    public float velocidad;

    [SerializeField] private Transform[] puntosDeVista;

    private int indiceActualPuntoDeVista;

    void Awake()
    {
        esteTransform = transform;
    }

    private void Update()
    {
        Mover();
        RotarCamara();
    }

    private void Mover()
    {
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            Vector3 movimientoHorizontal = puntosDeVista[indiceActualPuntoDeVista].right * (
                Input.GetAxis("Horizontal") * velocidad);

            Vector3 movimientoVertical = puntosDeVista[indiceActualPuntoDeVista].forward * (
                Input.GetAxis("Vertical") * velocidad);

            Vector3 relativo = new Vector3(Mathf.Clamp(
                esteTransform.localPosition.x, -200, 200), esteTransform.localPosition.y, Mathf.Clamp(
                    esteTransform.localPosition.z, -200, 200));

            esteTransform.localPosition = relativo + (movimientoHorizontal + movimientoVertical);
        }
    }

    private void RotarCamara()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            puntosDeVista[indiceActualPuntoDeVista].gameObject.SetActive(false);
            indiceActualPuntoDeVista = indiceActualPuntoDeVista >= puntosDeVista.Length - 1 ? 0 : ++indiceActualPuntoDeVista;
            puntosDeVista[indiceActualPuntoDeVista].gameObject.SetActive(true);
        }
    }
}