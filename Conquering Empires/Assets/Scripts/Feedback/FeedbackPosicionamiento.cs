using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Emperor
{
    public class FeedbackPosicionamiento : MonoBehaviour
    {
        private OutlineObject outlineObject;
        private Collider esteCollider;
        public bool edificioColocado { get; private set; }
        public bool colliderEstorbando { get; private set; }

        private void Awake()
        {
            esteCollider = GetComponent<Collider>();
            outlineObject = GetComponent<OutlineObject>();
        }

        public void FeedbackPosicion()
        {
            if (colliderEstorbando == true)
                outlineObject.ActivarEmision();
            else
                outlineObject.DesactivarEmision();
        }

        public void PosicionarEdificio()
        {
            esteCollider.enabled = false;
            colliderEstorbando = false;
        }

        private void OnTriggerStay(Collider other)
        {
            colliderEstorbando = true;
        }

        private void OnTriggerExit(Collider other)
        {
            colliderEstorbando = false;
        }
    }
}