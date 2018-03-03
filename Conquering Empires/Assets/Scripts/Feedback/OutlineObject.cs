using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Emperor
{
    public class OutlineObject : MonoBehaviour, IPointerDownHandler
    {
        public Action<int> OnClickCollider;
        public UnityEvent OnClick;
        public UnityEvent OnNoClick;
        private Material[] materiales;

        public bool emisionActivada { get; private set; }
        public int idGameObject { get; private set; }

        private void Awake()
        {
            idGameObject = gameObject.GetInstanceID();
            materiales = GetComponentInChildren<MeshRenderer>().materials;
        }

        public void OnPointerDown(PointerEventData pointerEventData)
        {
            OnClickCollider(idGameObject);
        }

        public void ActivarEmisionAction()
        {
            ActivarEmision();
            if (OnClick != null) OnClick.Invoke();
            emisionActivada = true;
        }

        public void DesactivarEmisionAction()
        {
            DesactivarEmision();
            emisionActivada = false;
            if (OnNoClick != null) OnNoClick.Invoke();
        }

        public void ActivarEmision()
        {
            for (int i = 0; i < materiales.Length; i++)
            {
                materiales[i].EnableKeyword("_EMISSION");
            }
        }

        public void DesactivarEmision()
        {
            for (int i = 0; i < materiales.Length; i++)
            {
                materiales[i].DisableKeyword("_EMISSION");
            }
        }
    }
}