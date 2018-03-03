using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using GameManager;

public class Agricultor : MonoBehaviour
{
    public ControladorSpawn spawnRecursos;

    private NavMeshAgent agenteAgricultor;

    public LineRenderer rayo;

    private Transform[] granjas;
    private Transform[] wayPointsGranja;
    private Transform[] frutosACortar;
    public Transform objetivoActual;
    public Transform almacen;
    private Transform almacenajeGranja;

    public Vector3 otri;

    public int numeroGranja;
    private int wayPointActual;
    public int frutasRecogidas;

    private float contadorDetenerse;

    void Awake()
    {
        agenteAgricultor = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        granjas = ControladorMapa.controladorMapa.variablesControladorMapa.informacionGranjas;
       // almacen = ControladorCanvas.controladorCanvas.almacen;
        transform.tag = "Aldeano/Trabajador/Agricultor";
		ControladorEventos.controladorEventos.FuncionesActuales += IrHaciaElMapa;
    }

    void IrHaciaElMapa()
    {
        //agenteAgricultor.destination = controladorEstados.mapa.GetChild(0).position;
		ControladorEventos.controladorEventos.FuncionesActuales -= IrHaciaElMapa;
		ControladorEventos.controladorEventos.FuncionesActuales += AlLlegarAlmapa;
    }

    void AlLlegarAlmapa()
    {
        if (agenteAgricultor.remainingDistance <= agenteAgricultor.stoppingDistance && !agenteAgricultor.pathPending)
        {
			ControladorEventos.controladorEventos.FuncionesActuales -= AlLlegarAlmapa;
			ControladorEventos.controladorEventos.FuncionesActuales += RecogerInformacion;
        }
    }

    void RecogerInformacion()
    {
        //granjas = new Transform[controladorMapa.informacionGranjas.Length];
        //controladorMapa.DarInformacionGranjas(granjas);
		ControladorEventos.controladorEventos.FuncionesActuales -= RecogerInformacion;
		ControladorEventos.controladorEventos.FuncionesActuales += ElegirUnaGranja;
    }

    public void ElegirUnaGranja()
    {
        almacenajeGranja = granjas[Random.Range(0, granjas.Length)];
        HacerProseguirAgente(almacenajeGranja.position, 3.5f, 0);
        ControladorEventos.controladorEventos.FuncionesActuales -= ElegirUnaGranja;
        ControladorEventos.controladorEventos.FuncionesActuales += AlLlegarALaGranja;
    }

    public void AlLlegarALaGranja()
    {
        if (agenteAgricultor.remainingDistance <= agenteAgricultor.stoppingDistance && !agenteAgricultor.pathPending)
        {
            ControladorEventos.controladorEventos.FuncionesActuales -= AlLlegarALaGranja;
            ControladorEventos.controladorEventos.FuncionesActuales += ElegirTerreno;
        }
    }

    public void ElegirTerreno()
    {
        for (int Num = 0; Num <= almacenajeGranja.childCount - 1; Num++)
        {
            if (almacenajeGranja.GetChild(Num).tag != "Aldeano/Granja/" + Num)
            {
                numeroGranja = Num;
                almacenajeGranja.GetChild(Num).tag = "Aldeano/Granja/" + Num;
                agenteAgricultor.destination = almacenajeGranja.GetChild(Num).transform.position;
                ControladorEventos.controladorEventos.FuncionesActuales -= ElegirTerreno;
                ControladorEventos.controladorEventos.FuncionesActuales += AlLlegarAlTerreno;
                break;
            }
        }
    }

    public void AlLlegarAlTerreno()
    {
        if (agenteAgricultor.remainingDistance <= agenteAgricultor.stoppingDistance && !agenteAgricultor.pathPending)
        {
            ControladorEventos.controladorEventos.FuncionesActuales -= AlLlegarAlTerreno;
            ControladorEventos.controladorEventos.FuncionesActuales += ObtenerTransformWayPoints;
        }
    }

    public void ObtenerTransformWayPoints()
    {
        wayPointsGranja = new Transform[almacenajeGranja.GetChild(numeroGranja).childCount];
        if (wayPointsGranja[0] == null)
        {
            for (int Num = 0; Num < wayPointsGranja.Length; Num++)
            {
                wayPointsGranja[Num] = almacenajeGranja.GetChild(numeroGranja).GetChild(Num);
            }
        }
        frutosACortar = new Transform[wayPointsGranja.Length];
        HacerProseguirAgente(wayPointsGranja[wayPointActual].transform.position, 0.3f, 0.3f);
        ControladorEventos.controladorEventos.FuncionesActuales -= ObtenerTransformWayPoints;
        ControladorEventos.controladorEventos.FuncionesActuales += RecorrerTerreno;
    }

    public void RecorrerTerreno()
    {
        StartCoroutine(CrecimientoFruta());
        if (agenteAgricultor.remainingDistance <= agenteAgricultor.stoppingDistance && !agenteAgricultor.pathPending)
        {
            contadorDetenerse += Time.deltaTime;
            DetenerAgente();
            if (contadorDetenerse >= 2f)
            {
                AlPasarElTiempoDeDetenerse();
                contadorDetenerse = 0;
            }
        }
    }

    void AlPasarElTiempoDeDetenerse()
    {
        frutosACortar[wayPointActual] = wayPointsGranja[wayPointActual].transform;
        wayPointActual++;
        HacerProseguirAgente(wayPointsGranja[wayPointActual].transform.position, 0.3f, 0);
        frutosACortar[wayPointActual] = wayPointsGranja[wayPointActual].transform;
        if (wayPointActual >= wayPointsGranja.Length - 1)
        {
            ControladorEventos.controladorEventos.FuncionesActuales -= RecorrerTerreno;
            ControladorEventos.controladorEventos.FuncionesActuales += EsperarCrecimientoFruta;
        }
    }

    IEnumerator CrecimientoFruta()
    {
        for (int Num = 0; Num < frutosACortar.Length; Num++)
        {
            if (frutosACortar[Num] == null || frutosACortar[Num].localScale.y == 0.5f)
            {
                continue;
            }
            else
            {
                frutosACortar[Num].localScale = frutosACortar[Num].localScale.y < 0.5f ? frutosACortar[Num].localScale += Vector3.one * 0.0008f : Vector3.one * 0.5f;
                frutosACortar[Num].position = new Vector3(frutosACortar[Num].position.x, frutosACortar[Num].localScale.y / 2, frutosACortar[Num].position.z);
                yield return null;
            }
        }
        yield return null;
    }

    public void EsperarCrecimientoFruta()
    {
        StartCoroutine(CrecimientoFruta());
        DetenerAgente();
        VerificarCrecimiento();
    }

    void VerificarCrecimiento()
    {
        for (int i = 0; i < frutosACortar.Length; i++)
        {
            if (frutosACortar[i].localScale.y < 0.5f)
            {
                return;
            }
        }
        ControladorEventos.controladorEventos.FuncionesActuales -= EsperarCrecimientoFruta;
        ControladorEventos.controladorEventos.FuncionesActuales += ElegirFruta;
    }

    public void ElegirFruta()
    {
        for (int i = 0; true; i++)
        {
            objetivoActual = frutosACortar[Random.Range(0, frutosACortar.Length)];
            if (objetivoActual.gameObject.activeInHierarchy == true)
            {
                break;
            }
            else if (i > 1000)
            {
                break;
            }
        }
        HacerProseguirAgente(objetivoActual.position, 3.5f, 1.5f);
        ControladorEventos.controladorEventos.FuncionesActuales -= ElegirFruta;
        ControladorEventos.controladorEventos.FuncionesActuales += CortarFruta;
    }

    public void CortarFruta()
    {
        if (agenteAgricultor.remainingDistance <= agenteAgricultor.stoppingDistance && !agenteAgricultor.pathPending)
        {
            objetivoActual.gameObject.SetActive(false);
            spawnRecursos.PoolingObjeto(objetivoActual, ref objetivoActual, spawnRecursos.variablesControladorSpawn.alimento, "Recurso/Alimento");
            //spawnRecursos.PosicionarRecurso(objetivoActual, out objetivoActual, spawnRecursos.fruto, "Recurso/Alimento");
            Rigidbody FisicaFruta = objetivoActual.GetComponent<Rigidbody>();
            FisicaFruta.isKinematic = true;
            objetivoActual.SetParent(transform);
            HacerProseguirAgente(almacen.GetChild(0).position, 3.5f, 3);
            ControladorEventos.controladorEventos.FuncionesActuales -= CortarFruta;
            ControladorEventos.controladorEventos.FuncionesActuales += PosicionarFruta;
        }
    }

    public void PosicionarFruta()
    {
        objetivoActual.localPosition = Vector3.MoveTowards(objetivoActual.localPosition, -Vector3.right * 0.9f, Time.deltaTime);
        if (agenteAgricultor.remainingDistance <= agenteAgricultor.stoppingDistance && !agenteAgricultor.pathPending)
        {
            AlLlegarAlAlmacen();
        }
    }

    public void AlLlegarAlAlmacen()
    {
        DetenerAgente();
        objetivoActual.SetParent(null);
        frutasRecogidas++;
        objetivoActual.GetComponent<Rigidbody>().isKinematic = false;
        if (frutasRecogidas >= 16)
        {
            CultivarDeNuevo();
        }
        else
        {
            SeguirRecogiendoFruta();
        }
    }

    public void CultivarDeNuevo()
    {
        HacerProseguirAgente(almacenajeGranja.position, 3.5f, 0);
        ControladorEventos.controladorEventos.FuncionesActuales -= PosicionarFruta;
        ControladorEventos.controladorEventos.FuncionesActuales += ReiniciarCicloAgricultor;
    }

    public void SeguirRecogiendoFruta()
    {
        ControladorEventos.controladorEventos.FuncionesActuales -= PosicionarFruta;
        ControladorEventos.controladorEventos.FuncionesActuales += ElegirFruta;
    }

    public void ReiniciarCicloAgricultor()
    {
        if (agenteAgricultor.remainingDistance <= agenteAgricultor.stoppingDistance && !agenteAgricultor.pathPending)
        {
            ReiniciarFruto();
            frutasRecogidas = 0;
            wayPointActual = 0;
            ControladorEventos.controladorEventos.FuncionesActuales -= ReiniciarCicloAgricultor;
            ControladorEventos.controladorEventos.FuncionesActuales += AlLlegarAlTerreno;
        }
    }

    public void ReiniciarFruto()
    {
        for (int i = 0; i < frutosACortar.Length; i++)
        {
            frutosACortar[i].localScale = Vector3.one * 0.05f;
            frutosACortar[i].gameObject.SetActive(true);
        }
    }

    void DetenerAgente()
    {
        if (!agenteAgricultor.isStopped)
        {
            agenteAgricultor.isStopped = true;
            agenteAgricultor.speed = 0;
        }
    }

    void HacerProseguirAgente(Vector3 destinoAgente, float velocidadAgente, float distanciaADetenerse)
    {
        agenteAgricultor.destination = destinoAgente;
        agenteAgricultor.isStopped = false;
        agenteAgricultor.speed = velocidadAgente;
        agenteAgricultor.stoppingDistance = distanciaADetenerse;
    }
}