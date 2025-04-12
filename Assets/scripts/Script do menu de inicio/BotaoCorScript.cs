using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MudarCorTexto : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text textoDoBotao;
    public Color corMouseOver = Color.red;
    private Color corOriginal;

    void Start()
    {
        if (textoDoBotao != null)
        {
            corOriginal = textoDoBotao.color;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (textoDoBotao != null)
        {
            textoDoBotao.color = corMouseOver;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (textoDoBotao != null)
        {
            textoDoBotao.color = corOriginal;
        }
    }
}