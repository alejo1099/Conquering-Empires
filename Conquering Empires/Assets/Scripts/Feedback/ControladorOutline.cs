using System;
using System.Collections.Generic;
using UnityEngine;

namespace Emperor
{
    public class ControladorOutline : MonoBehaviour
    {
        public static ControladorOutline SingletonControladorOutline;
        [SerializeField] private List<OutlineObject> listaEdificios;

        private bool emisionActivada;
        private bool emisionDesactivada;

        private void Awake()
        {
            if (SingletonControladorOutline == null)
                SingletonControladorOutline = this;
            else
                Destroy(this);
            AsignarOnClick();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && PosicionarEdificios.SingletonPosicionarEdificios.VerificarClick() == false)
                DesactivarEmision();
        }

        private void AsignarOnClick()
        {
            for (int i = 0; i < listaEdificios.Count; i++)
            {
                listaEdificios[i].OnClickCollider += VerificarID;
            }
        }

        private void VerificarID(int id)
        {
            emisionActivada = false;
            emisionDesactivada = false;
            ConfigurarEmisionId(id);
        }

        private void ConfigurarEmisionId(int id)
        {
            for (int i = 0; i < listaEdificios.Count; i++)
            {
                if (listaEdificios[i].gameObject.activeInHierarchy)
                {
                    if (!emisionActivada && listaEdificios[i].idGameObject == id)
                    {
                        emisionActivada = true;
                        listaEdificios[i].ActivarEmisionAction();
                    }
                    else if (emisionDesactivada == false && listaEdificios[i].emisionActivada && listaEdificios[i].idGameObject != id)
                    {
                        emisionDesactivada = true;
                        listaEdificios[i].DesactivarEmisionAction();
                    }
                    if (emisionActivada && emisionDesactivada)
                        break;
                }
            }
        }

        private void DesactivarEmision()
        {
            for (int i = 0; i < listaEdificios.Count; i++)
            {
                if (listaEdificios[i].gameObject.activeInHierarchy)
                {
                    if (listaEdificios[i].emisionActivada)
                    {
                        listaEdificios[i].DesactivarEmisionAction();
                        return;
                    }
                }
            }
        }

        public void AgregarOutline(OutlineObject objeto)
        {
            if (!listaEdificios.Exists(x => x.idGameObject == objeto.idGameObject))
            {
                listaEdificios.Add(objeto);
                objeto.OnClickCollider += VerificarID;
            }
        }
    }
}