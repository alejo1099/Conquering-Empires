using UnityEngine;

//Script encargado de mover y rotar al player
public class MovimientoRotacion
{
    private struct VariablesMovimiento
    {
        public Transform transformPlayer;
        public Transform transformCamara;

        public Rigidbody rigidbodyPlayer;

        public float velocidadMovimiento;
        public float velocidadRotacion;
        public float guardarvelocidadMovimiento;

        public Vector3 desplazamientoJugador;
    }

    private VariablesMovimiento variablesMovimiento = new VariablesMovimiento();

    public MovimientoRotacion(Transform transformJugador, Transform transformCamara, float velocidadMovimiento, float velocidadRotacion)
    {
        variablesMovimiento.transformPlayer = transformJugador;
        variablesMovimiento.transformCamara = transformCamara;
        variablesMovimiento.rigidbodyPlayer = transformJugador.GetComponent<Rigidbody>();

        variablesMovimiento.velocidadMovimiento = velocidadMovimiento;
        variablesMovimiento.velocidadRotacion = velocidadRotacion;
        variablesMovimiento.guardarvelocidadMovimiento = velocidadRotacion;
    }

    public void MovimientoRigidbody()
    {
        variablesMovimiento.desplazamientoJugador = ((variablesMovimiento.transformPlayer.forward *
        Input.GetAxis("Vertical")) + (variablesMovimiento.transformPlayer.right * Input.GetAxis("Horizontal"))) * variablesMovimiento.velocidadMovimiento;

        variablesMovimiento.rigidbodyPlayer.MovePosition(variablesMovimiento.transformPlayer.position + variablesMovimiento.desplazamientoJugador);
    }

    public void RotacionRigidbody()
    {
        variablesMovimiento.rigidbodyPlayer.MoveRotation(variablesMovimiento.transformPlayer.rotation * Quaternion.Euler(0f, Input.GetAxis("Mouse X") * variablesMovimiento.velocidadRotacion, 0f));

        variablesMovimiento.transformCamara.localRotation *= Quaternion.Euler(Input.GetAxis("Mouse Y") * -variablesMovimiento.velocidadRotacion, 0f, 0f);

        variablesMovimiento.velocidadRotacion = (variablesMovimiento.transformCamara.localRotation.x >= 0.3826f && Input.GetAxis("Mouse Y") < -0.1f)
         || (variablesMovimiento.transformCamara.localRotation.x <= -0.5f && Input.GetAxis("Mouse Y") > 0.1f) ? 0 : variablesMovimiento.guardarvelocidadMovimiento;
    }
}
