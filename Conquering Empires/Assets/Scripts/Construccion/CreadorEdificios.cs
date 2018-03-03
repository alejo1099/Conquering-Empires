using UnityEngine;

namespace Emperor
{
    public class CreadorEdificios : MonoBehaviour
    {
        private Transform padreInstancia;

        private void Awake()
        {
            padreInstancia = new GameObject("Padre Edificio").transform;
            padreInstancia.SetParent(transform.GetChild(0));
        }

        private bool VerificarMadera(int cantidadRecurso)
        {
            if (ControladorRecursos.SingletonControladorRecursos.cantidadMadera >= cantidadRecurso)
            {
                ControladorRecursos.SingletonControladorRecursos.cantidadMadera -= cantidadRecurso;
                return true;
            }
            else
            {
                print("Recursos Insuficientes");
                return false;
            }
        }

        private bool VerificarAlimento(int cantidadRecurso)
        {
            if (ControladorRecursos.SingletonControladorRecursos.cantidadAlimento >= cantidadRecurso)
            {
                ControladorRecursos.SingletonControladorRecursos.cantidadAlimento -= cantidadRecurso;
                return true;
            }
            else
            {
                print("Recursos Insuficientes");
                return false;
            }
        }

        private bool VerificarPiedra(int cantidadRecurso)
        {
            if (ControladorRecursos.SingletonControladorRecursos.cantidadPiedra >= cantidadRecurso)
            {
                ControladorRecursos.SingletonControladorRecursos.cantidadPiedra -= cantidadRecurso;
                return true;
            }
            else
            {
                print("Recursos Insuficientes");
                return false;
            }
        }

        public void CrearCasa()
        {
            if (!VerificarMadera(50))
                return;

            SpawnEdificios.SingletonSpawnEdificios.InstanciarCasa(ref padreInstancia);
            ControladorOutline.SingletonControladorOutline.AgregarOutline(
                padreInstancia.GetComponentInChildren<OutlineObject>());

            PosicionarEdificios.SingletonPosicionarEdificios.MoverEdificio(padreInstancia);
        }

        public void CrearGranja()
        {
            if (!VerificarMadera(70))
                return;

            SpawnEdificios.SingletonSpawnEdificios.InstanciarGranja(ref padreInstancia);
            ControladorOutline.SingletonControladorOutline.AgregarOutline(
                padreInstancia.GetComponentInChildren<OutlineObject>());

            PosicionarEdificios.SingletonPosicionarEdificios.MoverEdificio(padreInstancia);
        }

        public void CrearPuestoLenador()
        {
            if (!VerificarMadera(70) || !VerificarAlimento(30))
                return;

            SpawnEdificios.SingletonSpawnEdificios.InstanciarPuestoLenador(ref padreInstancia);
            ControladorOutline.SingletonControladorOutline.AgregarOutline(
                padreInstancia.GetComponentInChildren<OutlineObject>());

            PosicionarEdificios.SingletonPosicionarEdificios.MoverEdificio(padreInstancia);
        }

        public void CrearCantera()
        {
            if (!VerificarMadera(60) || !VerificarAlimento(20))
                return;

            SpawnEdificios.SingletonSpawnEdificios.InstanciarCantera(ref padreInstancia);
            ControladorOutline.SingletonControladorOutline.AgregarOutline(
                padreInstancia.GetComponentInChildren<OutlineObject>());

            PosicionarEdificios.SingletonPosicionarEdificios.MoverEdificio(padreInstancia);
        }

        public void CrearCuartel()
        {
            if (!VerificarMadera(250) || !VerificarAlimento(100))
                return;

            SpawnEdificios.SingletonSpawnEdificios.InstanciarCuartel(ref padreInstancia);
            ControladorOutline.SingletonControladorOutline.AgregarOutline(
                padreInstancia.GetComponentInChildren<OutlineObject>());

            PosicionarEdificios.SingletonPosicionarEdificios.MoverEdificio(padreInstancia);
        }

        public void CrearEstablo()
        {
            if (!VerificarMadera(300) || !VerificarAlimento(150))
                return;

            SpawnEdificios.SingletonSpawnEdificios.InstanciarEstablo(ref padreInstancia);
            ControladorOutline.SingletonControladorOutline.AgregarOutline(
                padreInstancia.GetComponentInChildren<OutlineObject>());

            PosicionarEdificios.SingletonPosicionarEdificios.MoverEdificio(padreInstancia);
        }

        public void CrearArqueria()
        {
            if (!VerificarMadera(200) || !VerificarAlimento(100))
                return;

            SpawnEdificios.SingletonSpawnEdificios.InstanciarArqueria(ref padreInstancia);
            ControladorOutline.SingletonControladorOutline.AgregarOutline(
                padreInstancia.GetComponentInChildren<OutlineObject>());

            PosicionarEdificios.SingletonPosicionarEdificios.MoverEdificio(padreInstancia);
        }
    }
}