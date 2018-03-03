using UnityEngine;
using GameManager;

//Script encargado de los ataques del player
public class AtaquePlayer
{
    struct VariablesAtaqueArmas
    {
        internal Camera ReferenciaCamara;
        internal Transform posicionArma;

        internal LayerMask capasDeseadas;

        internal float intervalo;
        internal float tiempo;
        internal float tiempoLerpEspada;
        internal float tiempoRestaurarEspada;
        internal float tiempoLerpPica;
        internal float tiempoRestaurarPica;

        internal bool atacandoEspada;
        internal bool restauracionEspadaCompleta;
        internal bool atacandoPica;
        internal bool restauracionPicaCompleta;
    }

    VariablesAtaqueArmas variablesAtaqueArmas = new VariablesAtaqueArmas();

    public AtaquePlayer(LayerMask capasDeseadas, Transform posicionArma, Camera ReferenciaCamara)
    {
        variablesAtaqueArmas.capasDeseadas = capasDeseadas;
        variablesAtaqueArmas.posicionArma = posicionArma;
        variablesAtaqueArmas.ReferenciaCamara = ReferenciaCamara;

        variablesAtaqueArmas.intervalo = 0.25f;
        variablesAtaqueArmas.restauracionEspadaCompleta = true;
        variablesAtaqueArmas.restauracionPicaCompleta = true;
    }

    public void DispararFlecha()
    {
        if (ControladorPlayer.controladorPlayer.armaElegida == ArmaElegida.Arco)
        {
            if (Input.GetButtonDown("Fire1") && Time.time > variablesAtaqueArmas.tiempo)
            {
                variablesAtaqueArmas.tiempo = Time.time + variablesAtaqueArmas.intervalo;
                ApuntarFlecha();
                SpawnerMilitar.spawnerMilitar.PoolingObjeto(ControladorPlayer.controladorPlayer.PosicionFlecha, SpawnerMilitar.spawnerMilitar.Flechas);
            }
        }
    }

    private void ApuntarFlecha()
    {
        Vector3 One = variablesAtaqueArmas.ReferenciaCamara.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(One, variablesAtaqueArmas.ReferenciaCamara.transform.forward, out hit, 50, variablesAtaqueArmas.capasDeseadas, QueryTriggerInteraction.Ignore))
        {
            ControladorPlayer.controladorPlayer.PosicionFlecha.LookAt(hit.point);
        }
        else
        {
            ControladorPlayer.controladorPlayer.PosicionFlecha.LookAt(One + variablesAtaqueArmas.ReferenciaCamara.transform.forward * 50);
        }
    }

    public void AtacarEspada()
    {
        if (ControladorPlayer.controladorPlayer.armaElegida == ArmaElegida.Espada)
        {
            if (Input.GetButtonDown("Fire1") && variablesAtaqueArmas.restauracionEspadaCompleta)
            {
                variablesAtaqueArmas.atacandoEspada = true;
            }
            AtaqueEspada();
            RestaurarEspada();
        }
    }

    private void AtaqueEspada()
    {
        if (variablesAtaqueArmas.atacandoEspada)
        {
            variablesAtaqueArmas.tiempoLerpEspada += Time.fixedDeltaTime;
            variablesAtaqueArmas.posicionArma.localRotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(90, 0, 60), variablesAtaqueArmas.tiempoLerpEspada * 4);
            if (variablesAtaqueArmas.posicionArma.localEulerAngles.x >= 89)
            {
                variablesAtaqueArmas.atacandoEspada = false;
                variablesAtaqueArmas.tiempoRestaurarEspada = 0;
                variablesAtaqueArmas.restauracionEspadaCompleta = false;
            }
        }
    }

    private void RestaurarEspada()
    {
        if (!variablesAtaqueArmas.atacandoEspada && !variablesAtaqueArmas.restauracionEspadaCompleta)
        {
            variablesAtaqueArmas.tiempoRestaurarEspada += Time.fixedDeltaTime;
            variablesAtaqueArmas.posicionArma.localRotation = Quaternion.Lerp(variablesAtaqueArmas.posicionArma.localRotation, Quaternion.identity, variablesAtaqueArmas.tiempoRestaurarEspada);
            if (variablesAtaqueArmas.posicionArma.localRotation == Quaternion.identity)
            {
                variablesAtaqueArmas.tiempoLerpEspada = 0;
                variablesAtaqueArmas.restauracionEspadaCompleta = true;
            }
        }
    }

    public void AtacarPica()
    {
        if (ControladorPlayer.controladorPlayer.armaElegida == ArmaElegida.Pica)
        {
            if (Input.GetButtonDown("Fire1") && variablesAtaqueArmas.restauracionPicaCompleta)
            {
                variablesAtaqueArmas.atacandoPica = true;
            }
            AtaquePica();
            RestaurarPica();
        }
    }

    private void AtaquePica()
    {
        if (variablesAtaqueArmas.atacandoPica)
        {
            variablesAtaqueArmas.tiempoLerpPica += Time.fixedDeltaTime;
            variablesAtaqueArmas.posicionArma.localRotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(90, 0, 10), variablesAtaqueArmas.tiempoLerpPica * 4);
            if (variablesAtaqueArmas.posicionArma.localEulerAngles.x >= 89)
            {
                variablesAtaqueArmas.posicionArma.localPosition = Vector3.Lerp(new Vector3(0.4f, -0.3f, 1), new Vector3(0.2f, -0.3f, 2), variablesAtaqueArmas.tiempoLerpPica * 3);
            }
            if (variablesAtaqueArmas.posicionArma.localPosition.z >= 2)
            {
                variablesAtaqueArmas.atacandoPica = false;
                variablesAtaqueArmas.tiempoRestaurarPica = 0;
                variablesAtaqueArmas.restauracionPicaCompleta = false;
            }
        }
    }

    private void RestaurarPica()
    {
        if (!variablesAtaqueArmas.atacandoPica && !variablesAtaqueArmas.restauracionPicaCompleta)
        {
            variablesAtaqueArmas.tiempoRestaurarPica += Time.fixedDeltaTime;
            variablesAtaqueArmas.posicionArma.localRotation = Quaternion.Lerp(variablesAtaqueArmas.posicionArma.localRotation, Quaternion.identity, variablesAtaqueArmas.tiempoRestaurarPica);
            variablesAtaqueArmas.posicionArma.localPosition = Vector3.Lerp(new Vector3(0.2f, -0.3f, 2), new Vector3(0.4f, -0.3f, 1), variablesAtaqueArmas.tiempoRestaurarPica * 1.5f);
            if (variablesAtaqueArmas.posicionArma.localRotation == Quaternion.identity && variablesAtaqueArmas.posicionArma.localPosition.z <= 1)
            {
                variablesAtaqueArmas.tiempoLerpPica = 0;
                variablesAtaqueArmas.restauracionPicaCompleta = true;
            }
        }
    }
}