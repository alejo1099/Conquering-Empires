using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Emperor
{
    public class PosicionarEdificios : MonoBehaviour
    {
        public static PosicionarEdificios SingletonPosicionarEdificios;
        private FeedbackPosicionamiento feedbackPosicionamiento;
        private Transform objetoMover;
        [SerializeField] private Camera[] camarasPlayer;
        public LayerMask capasSuelo;
        private Ray rayoMouse;

        private bool cogiendoEdificio;

        private void Awake()
        {
            if (SingletonPosicionarEdificios == null)
                SingletonPosicionarEdificios = this;
            else
                Destroy(this);
        }

        private void Update()
        {
            if (!cogiendoEdificio)
                return;

            if (feedbackPosicionamiento != null)
                feedbackPosicionamiento.FeedbackPosicion();

            if (cogiendoEdificio && Input.GetMouseButtonDown(0))
            {
                if (feedbackPosicionamiento.colliderEstorbando == false)
                {
                    cogiendoEdificio = false;
                    feedbackPosicionamiento.PosicionarEdificio();
                }
                    
            }
            ConfigurarRayo();
        }

        public void MoverEdificio(Transform objetoAMover)
        {
            objetoMover = objetoAMover;
            cogiendoEdificio = true;
            feedbackPosicionamiento = objetoMover.GetComponentInChildren<FeedbackPosicionamiento>();
        }

        private Camera SeleccionarCamaraActiva()
        {
            for (int i = 0; i < camarasPlayer.Length; i++)
            {
                if (camarasPlayer[i].gameObject.activeInHierarchy)
                {
                    return camarasPlayer[i];
                }
            }
            return new GameObject("Camara").AddComponent<Camera>();
        }

        private void ConfigurarRayo()
        {
            rayoMouse = SeleccionarCamaraActiva().ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(rayoMouse, out hit, 500f, capasSuelo, QueryTriggerInteraction.Ignore))
            {
                objetoMover.position = hit.point;
            }
        }

        public bool VerificarClick()
        {
            rayoMouse = SeleccionarCamaraActiva().ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(rayoMouse, out hit, 1000f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                if (hit.transform.GetComponent<OutlineObject>() != null)
                    return true;
            }
            return false;
        }
    }
}