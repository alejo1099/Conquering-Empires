using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EspecificacionBoton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text textoFeedback;

    [TextArea] public string texto;

    public void OnPointerEnter(PointerEventData eventData)
    {
        textoFeedback.text = texto;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        textoFeedback.text = " ";
    }
}