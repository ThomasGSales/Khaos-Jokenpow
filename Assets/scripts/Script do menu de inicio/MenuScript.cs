using UnityEngine;
using UnityEngine.SceneManagement;

public class CarregarCena : MonoBehaviour
{
    public void CarregarCenaPorNome(string nomeDaCena)
    {
        SceneManager.LoadScene(nomeDaCena);
    }

    public void SairDoJogo()
    {
        Application.Quit();
    }
}