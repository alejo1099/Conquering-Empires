using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using GameManager;
using InteligenciaArtificialNPC;

public class Minero : Trabajador
{
    struct VariablesMinero
    {
        internal Transform[] Canteras;
        internal Transform posicionInstanciar;
        internal Transform canteraElegida;
        internal int flagCapacity;
    }
    VariablesMinero variablesMinero = new VariablesMinero();

    private void Awake()
    {
        ObtenerComponentesPrincipales();
        variablesMinero.posicionInstanciar = transform.GetChild(1);
    }

    private void Start()
    {
        transform.tag = "Aldeano/Trabajador/Minero";
        variablesMinero.Canteras = ControladorLugares.controladorLugares.Canteras;
        ActualizarDestinoAgente(GameManagement.gameManagement.ElegirAlmacenCercano(GameManagement.gameManagement.AlmacenesRecursos, TransformAgente.position, ref posicionObjetivo), 1.5f, 3);
        AsignacionEventos.VerificarAsignar(LlegandoAlMapa);
    }

    private void LlegandoAlMapa()
    {
        if (AlLlegarAlDestino(TransformAgente.position, posicionObjetivo, 4))
        {
            AsignacionEventos.VerificarRemover(LlegandoAlMapa);
            ElegirCantera();
        }
    }

    private void ElegirCantera()
    {
        Transform[] temporal = new Transform[5];
        temporal = ElegirTransformsMasCercanos(variablesMinero.Canteras, TransformAgente, 5);
        variablesMinero.canteraElegida = ElegirElementoAlAzar(temporal);

        posicionObjetivo = variablesMinero.canteraElegida.GetChild(0).position;
        ActualizarDestinoAgente(posicionObjetivo);
        AsignacionEventos.VerificarAsignar(IrCantera);
    }

    private void IrCantera()
    {
        if (AlLlegarAlDestino(TransformAgente.position, posicionObjetivo, 4))
        {
            DetenerAgente();
            AsignacionEventos.VerificarRemover(IrCantera);
            StartCoroutine(ObtenerPiedra());
        }
    }

    private IEnumerator ObtenerPiedra()
    {
        ControladorSpawn.controladorSpawn.PoolingObjeto(variablesMinero.posicionInstanciar, ref objetivoActualAgente, ControladorSpawn.controladorSpawn.variablesControladorSpawn.piedra, "Recurso/Piedra");
        objetivoActualAgente.GetComponent<MeshRenderer>().enabled = false;
        while (variablesMinero.flagCapacity < 5 && ObtenerRecurso(variablesMinero.canteraElegida, objetivoActualAgente))
        {
            variablesMinero.flagCapacity++;
            yield return new WaitForSeconds(2);
        }
        variablesMinero.flagCapacity = 0;
        objetivoActualAgente.GetComponent<MeshRenderer>().enabled = true;
        RecogerPiedra();
    }

    private void RecogerPiedra()
    {
        objetivoActualAgente.SetParent(transform);
        objetivoActualAgente.GetComponent<Rigidbody>().isKinematic = true;
        ActualizarDestinoAgente(GameManagement.gameManagement.ElegirAlmacenCercano(GameManagement.gameManagement.AlmacenesRecursos, TransformAgente.position, ref posicionObjetivo), 1.5f, 3);
        AsignacionEventos.VerificarAsignar(AlCargarLaPiedra);
    }

    private void AlCargarLaPiedra()
    {
        objetivoActualAgente.localPosition = Vector3.MoveTowards(objetivoActualAgente.localPosition, new Vector3(-1.5f, 0, 0), Time.deltaTime);
        objetivoActualAgente.localRotation = Quaternion.Slerp(objetivoActualAgente.rotation, Quaternion.identity, Mathf.Lerp(0f, 1f, Time.time / 16f));
        if (AlLlegarAlDestino(TransformAgente.position, posicionObjetivo, 4))
        {
            DetenerAgente();
            objetivoActualAgente.GetComponent<Rigidbody>().isKinematic = false;
            objetivoActualAgente.SetParent(null);
            AsignacionEventos.VerificarRemover(AlCargarLaPiedra);
            SeguirTrabajando();
        }
    }

    private void SeguirTrabajando()
    {
        posicionObjetivo = variablesMinero.canteraElegida.GetChild(0).position;
        ActualizarDestinoAgente(posicionObjetivo, 1.5f, 3);
        AsignacionEventos.VerificarAsignar(IrCantera);
    }

    public override void EliminarDelegate()
    {
        AsignacionEventos.VerificarRemover(LlegandoAlMapa);
        AsignacionEventos.VerificarRemover(IrCantera);
        AsignacionEventos.VerificarRemover(AlCargarLaPiedra);
    }

    private void OnDisable()
    {
        EliminarDelegate();
        Destroy(this);
    }
}