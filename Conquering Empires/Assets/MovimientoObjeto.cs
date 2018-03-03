using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoObjeto : MonoBehaviour
{
	private Vector3 desplazamientoJugador;

	private Transform transformPlayer;
	private Transform transformCamara;
	private Rigidbody rigidbodyPlayer;

	public float velocidadMovimiento;
	public float velocidadRotacion;
	private float guardarvelocidadMovimiento;

	private void Awake()
	{
		transformPlayer = transform;
		rigidbodyPlayer = GetComponent<Rigidbody>();
		transformCamara = GetComponentInChildren<Camera>().transform;
		guardarvelocidadMovimiento = velocidadRotacion;
	}

    void Update()
    {
		MovimientoRigidbody();
		RotacionRigidbody();
    }

    public void MovimientoRigidbody()
    {
        desplazamientoJugador = ((transformPlayer.forward *
        Input.GetAxis("Vertical")) + (transformPlayer.right * Input.GetAxis("Horizontal"))) * velocidadMovimiento;

        rigidbodyPlayer.MovePosition(transformPlayer.position + desplazamientoJugador);
    }

    public void RotacionRigidbody()
    {
        rigidbodyPlayer.MoveRotation(transformPlayer.rotation * Quaternion.Euler(0f, Input.GetAxis("Mouse X") * velocidadRotacion, 0f));

        transformCamara.localRotation *= Quaternion.Euler(Input.GetAxis("Mouse Y") * -velocidadRotacion, 0f, 0f);

        velocidadRotacion = (transformCamara.localRotation.x >= 0.3826f && Input.GetAxis("Mouse Y") < -0.1f)
         || (transformCamara.localRotation.x <= -0.5f && Input.GetAxis("Mouse Y") > 0.1f) ? 0 : guardarvelocidadMovimiento;
    }
}
