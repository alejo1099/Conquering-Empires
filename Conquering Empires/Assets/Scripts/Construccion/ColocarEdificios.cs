using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColocarEdificios : MonoBehaviour
{
    public Transform objetoAColocar;
    private Camera camaraPlayer;
    public LayerMask capasSuelo;

    private bool cogiendoEdificio;

    void Start()
    {
        camaraPlayer = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        PosicionarEdificio();
    }

    private void PosicionarEdificio()
    {
        if (Input.GetKeyDown(KeyCode.P))
            cogiendoEdificio = true;
        if (cogiendoEdificio && Input.GetMouseButtonDown(0))
            cogiendoEdificio = false;

        if (!cogiendoEdificio)
            return;

        Ray rayoMouse = camaraPlayer.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(rayoMouse, out hit, 1000f, capasSuelo, QueryTriggerInteraction.Ignore))
        {
            objetoAColocar.position = hit.point;
        }
    }
}
