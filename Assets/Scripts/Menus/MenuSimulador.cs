using UnityEngine;
using UnityEngine.SceneManagement; 

public class MenuSimulador : MonoBehaviour
{
    
    public void CargarEscena(string nombreEscena)
    {
        SceneManager.LoadScene("MenuInicial");
       Debug.Log("lacasa");

    }
}