using UnityEngine;
using UnityEngine.UI;

public class CargasUIController : MonoBehaviour
{
    public Image[] cargas;
    public Image[] recargasOverlay;

    public void AtualizarCargas(int cargasAtuais, int maxCargas, float progressoRecarga)
    {
        for (int i = 0; i < cargas.Length; i++)
        {
            if (i < cargasAtuais)
            {
                cargas[i].enabled = true;
                recargasOverlay[i].fillAmount = 0f;
            }
            else if (i == cargasAtuais && i < maxCargas)
            {
                cargas[i].enabled = false;
                recargasOverlay[i].fillAmount = progressoRecarga;
            }
            else
            {
                cargas[i].enabled = false;
                recargasOverlay[i].fillAmount = 0f;
            }
        }
    }
}