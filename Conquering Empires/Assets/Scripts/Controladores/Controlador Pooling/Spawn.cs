using System.Collections;
using UnityEngine;

namespace Spawner
{
    public class Spawn : MonoBehaviour
    {
        protected int cantidadInstanciar = 50;

        public void InstanciarObjeto(ref GameObject[] arrayDeAlmacenamiento, GameObject tipoDeObjeto, Transform padreDeAlmacenamiento)
        {
            arrayDeAlmacenamiento = new GameObject[cantidadInstanciar];
            // for (int i = 0; i < arrayDeAlmacenamiento.Length; i++)
            // {
            //     GameObject prefab = Instantiate(tipoDeObjeto, Vector3.zero, Quaternion.identity);
            //     prefab.SetActive(false);
            //     prefab.transform.SetParent(padreDeAlmacenamiento);
            //     arrayDeAlmacenamiento[i] = prefab;
            // }
            StartCoroutine(InstanciarPausado(arrayDeAlmacenamiento, tipoDeObjeto, padreDeAlmacenamiento));
        }

        private IEnumerator InstanciarPausado(GameObject[] arrayDeAlmacenamiento, GameObject tipoDeObjeto, Transform padreDeAlmacenamiento)
        {
            for (int i = 0; i < arrayDeAlmacenamiento.Length; i++)
            {
                GameObject prefab = Instantiate(tipoDeObjeto, Vector3.zero, Quaternion.identity);
                prefab.SetActive(false);
                prefab.transform.SetParent(padreDeAlmacenamiento);
                arrayDeAlmacenamiento[i] = prefab;
                yield return null;
            }
        }

        public void PoolingObjeto(Transform posicionPooling, ref Transform transformObjeto, GameObject[] objetoElegido, string tagAsignar)
        {
            for (int i = 0; i < objetoElegido.Length; i++)
            {
                if (!objetoElegido[i].activeInHierarchy)
                {
                    objetoElegido[i].transform.position = posicionPooling.position;
                    objetoElegido[i].transform.rotation = posicionPooling.rotation;
                    objetoElegido[i].tag = tagAsignar;
                    objetoElegido[i].SetActive(true);
                    transformObjeto = objetoElegido[i].transform;
                    break;
                }
            }
        }

        public void PoolingObjeto(Transform posicionPooling, ref Transform transformObjeto, GameObject[] objetoElegido)
        {
            for (int i = 0; i < objetoElegido.Length; i++)
            {
                if (!objetoElegido[i].activeInHierarchy)
                {
                    objetoElegido[i].transform.position = posicionPooling.position;
                    objetoElegido[i].transform.rotation = posicionPooling.rotation;
                    objetoElegido[i].SetActive(true);
                    transformObjeto = objetoElegido[i].transform;
                    break;
                }
            }
        }

        public void PoolingObjeto(Transform posicionPooling, GameObject[] objetoElegido)
        {
            for (int i = 0; i < objetoElegido.Length; i++)
            {
                if (!objetoElegido[i].activeInHierarchy)
                {
                    objetoElegido[i].transform.position = posicionPooling.position;
                    objetoElegido[i].transform.rotation = posicionPooling.rotation;
                    objetoElegido[i].SetActive(true);
                    break;
                }
            }
        }

        public void PoolingObjeto(Vector3 posicionPooling, GameObject[] objetoElegido)
        {
            for (int i = 0; i < objetoElegido.Length; i++)
            {
                if (!objetoElegido[i].activeInHierarchy)
                {
                    objetoElegido[i].transform.position = posicionPooling;
                    objetoElegido[i].SetActive(true);
                    break;
                }
            }
        }

        public void PoolingObjeto(Vector3 posicionPooling, GameObject[] objetoElegido, Transform padreObjeto)
        {
            for (int i = 0; i < objetoElegido.Length; i++)
            {
                if (!objetoElegido[i].activeInHierarchy)
                {
                    objetoElegido[i].transform.position = posicionPooling;
                    objetoElegido[i].SetActive(true);
                    objetoElegido[i].transform.SetParent(padreObjeto);
                    break;
                }
            }
        }

        public void PoolingObjeto(Vector3 posicionPooling, GameObject[] objetoElegido, ref Transform refrenciaObjeto, Transform padreObjeto)
        {
            for (int i = 0; i < objetoElegido.Length; i++)
            {
                if (!objetoElegido[i].activeInHierarchy)
                {
                    objetoElegido[i].transform.position = posicionPooling;
                    objetoElegido[i].SetActive(true);
                    objetoElegido[i].transform.SetParent(padreObjeto);
                    refrenciaObjeto = objetoElegido[i].transform;
                    break;
                }
            }
        }

        public virtual void PoolingObjeto(Transform posicionPooling, GameObject[] objetoElegido, Transform padreObjeto)
        {
            for (int i = 0; i < objetoElegido.Length; i++)
            {
                if (!objetoElegido[i].activeInHierarchy)
                {
                    objetoElegido[i].transform.position = posicionPooling.position;
                    objetoElegido[i].transform.rotation = posicionPooling.rotation;
                    objetoElegido[i].SetActive(true);
                    objetoElegido[i].transform.SetParent(padreObjeto);
                    break;
                }
            }
        }

        public void PoolingObjeto(Transform posicionPooling, GameObject[] objetoElegido, string tagAsignar)
        {
            for (int i = 0; i < objetoElegido.Length; i++)
            {
                if (!objetoElegido[i].activeInHierarchy)
                {
                    objetoElegido[i].transform.position = posicionPooling.position;
                    objetoElegido[i].transform.rotation = posicionPooling.rotation;
                    objetoElegido[i].tag = tagAsignar;
                    objetoElegido[i].SetActive(true);
                    break;
                }
            }
        }

        public virtual void PoolingObjeto(Transform posicionPooling, GameObject[] objetoElegido, Transform padreObjeto, string tagAsignar)
        {
            for (int i = 0; i < objetoElegido.Length; i++)
            {
                if (!objetoElegido[i].activeInHierarchy)
                {
                    objetoElegido[i].transform.position = posicionPooling.position;
                    objetoElegido[i].transform.rotation = posicionPooling.rotation;
                    objetoElegido[i].tag = tagAsignar;
                    objetoElegido[i].SetActive(true);
                    objetoElegido[i].transform.SetParent(padreObjeto);
                    break;
                }
            }
        }
    }
}