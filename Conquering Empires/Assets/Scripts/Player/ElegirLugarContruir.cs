using UnityEngine;
using GameManager;

public class ElegirLugarContruir
{

    private LineRenderer indicadorDeContruccion;

    private Camera camaraPlayer;

    private Transform transformCamara;
    private Transform transformInicioLaser;

    private LayerMask capasDeseadas;
    private LayerMask capaSuelo = 9;

    Collider[] collidersPosibles;

    public Vector3 dimensionEdificio = new Vector3(11, 5, 12);
    private Vector3 inicioRayo;
    private Vector3 almacenarPosicionEdificio;

    private bool activarLinea;
    public bool construccionDisponible;
    //public bool recursosDisponibles;

    //Tamaño generico (11,5,12)
    //Tamaño muro (10,10,5)
    //Tamaño torre (10,11,10)
    //Tamaño castillo (30,11,30)

    public ElegirLugarContruir(Transform transformcamara, Transform transformInicioLaser, LayerMask capasDeseadas, LineRenderer lineRendererPlayer, LayerMask capasRaycast)
    {
        this.transformCamara = transformcamara;
        this.transformInicioLaser = transformInicioLaser;
        this.capasDeseadas = capasDeseadas;
        indicadorDeContruccion = lineRendererPlayer;
        capaSuelo = capasRaycast;
        camaraPlayer = transformCamara.GetComponent<Camera>();
    }

    public void DispararLinea()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            activarLinea = true;
            indicadorDeContruccion.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightControl))
        {
            activarLinea = false;
            construccionDisponible = false;
            indicadorDeContruccion.enabled = false;
        }
        if (activarLinea)
        {
            VerificarPuntoDeConstruccion();
        }
    }

    private void VerificarPuntoDeConstruccion()
    {
        inicioRayo = camaraPlayer.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        indicadorDeContruccion.SetPosition(0, transformInicioLaser.position);
        RaycastHit infoGolpe;

        if (Physics.Raycast(inicioRayo, transformCamara.forward, out infoGolpe, 20, capaSuelo, QueryTriggerInteraction.Ignore))
        {
            collidersPosibles = Physics.OverlapBox(infoGolpe.point, dimensionEdificio, Quaternion.identity, capasDeseadas, QueryTriggerInteraction.Ignore);
            if (collidersPosibles.Length == 0)
            {
                almacenarPosicionEdificio = infoGolpe.point;
                almacenarPosicionEdificio.y = 0;
                construccionDisponible = true;
                indicadorDeContruccion.material.color = Color.green;
                Gizmos.color = Color.green;
            }
            else
            {
                construccionDisponible = false;
                indicadorDeContruccion.material.color = Color.red;
                Gizmos.color = Color.red;
            }
        }
        else
        {
            construccionDisponible = false;
            indicadorDeContruccion.material.color = Color.red;
            Gizmos.color = Color.red;
        }
        indicadorDeContruccion.SetPosition(1, inicioRayo + (transformCamara.forward * 20));
    }

    public void Construir(GameObject[] edificioSeleccionado)
    {
        SpawnBuilds.spawnBuilds.PoolingObjeto(almacenarPosicionEdificio, edificioSeleccionado);
    }
}