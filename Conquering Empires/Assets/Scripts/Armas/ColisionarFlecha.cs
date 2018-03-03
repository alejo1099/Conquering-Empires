using System;
using System.Collections;
using UnityEngine;

public class ColisionarFlecha : MonoBehaviour
{
    [Serializable]
    struct VariablesFlecha
    {
        internal Transform thisTranform;
        internal Rigidbody rigidbodyFlecha;
        internal float tiempoDesdeCreacion;
        public float fuerzaDisparo;
    }

    [SerializeField] VariablesFlecha variablesFlecha = new VariablesFlecha();

    private void Awake()
    {
        variablesFlecha.rigidbodyFlecha = GetComponent<Rigidbody>();
        variablesFlecha.thisTranform = transform;
    }

    private void Start()
    {
        variablesFlecha.tiempoDesdeCreacion = 0;
    }

    private void Update()
    {
        variablesFlecha.tiempoDesdeCreacion += Time.fixedDeltaTime;
        if (variablesFlecha.tiempoDesdeCreacion <= 0.2f)
        {
            DispararFlecha();
        }
        if (variablesFlecha.tiempoDesdeCreacion > 10)
        {
            ReiniciarVelocidadYTiempo();
            gameObject.SetActive(false);
        }
    }

    void DispararFlecha()
    {
        variablesFlecha.rigidbodyFlecha.AddForce(variablesFlecha.thisTranform.forward * variablesFlecha.fuerzaDisparo);
    }

    void ReiniciarVelocidadYTiempo()
    {
        variablesFlecha.tiempoDesdeCreacion = 0;
        variablesFlecha.rigidbodyFlecha.isKinematic = true;
        variablesFlecha.rigidbodyFlecha.velocity = Vector3.zero;
        variablesFlecha.rigidbodyFlecha.angularVelocity = Vector3.zero;
        variablesFlecha.rigidbodyFlecha.isKinematic = false;
    }

    void OnTriggerEnter(Collider other)
    {
        ReiniciarVelocidadYTiempo();
        gameObject.SetActive(false);
    }
}