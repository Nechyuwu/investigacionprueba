using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject menuPrincipal;
    public GameObject instrucciones;

    [Header("Configuración de Espera")]
    public Button botonContinuar; // El botón dentro del panel de instrucciones
    public float tiempoDeEspera = 5f; // Tiempo en segundos antes de poder continuar

    void Start()
    {
        // Aseguramos el estado inicial: Menú activo, instrucciones ocultas
        menuPrincipal.SetActive(true);
        instrucciones.SetActive(false);
        
        // El botón de continuar debe iniciar desactivado
        if (botonContinuar != null)
        {
            botonContinuar.gameObject.SetActive(false);
        }
    }

    // Esta función se asignará al botón "Iniciar" de tu pantalla principal
    public void MostrarInstrucciones()
    {
        menuPrincipal.SetActive(false);
        instrucciones.SetActive(true);

        // Inicia la cuenta regresiva
        StartCoroutine(RutinaDeEspera());
    }

    private IEnumerator RutinaDeEspera()
    {
        // Espera la cantidad de segundos definida
        yield return new WaitForSeconds(tiempoDeEspera);
        
        // Activa el botón para que el jugador pueda avanzar
        botonContinuar.gameObject.SetActive(true);
    }

    // Esta función se asignará al botón "Continuar" que acaba de aparecer
    public void IniciarSimulador()
    {
        // Cambia "NombreDeTuEscena" por el nombre exacto de la escena de tu simulador de manejo
        SceneManager.LoadScene("pruebas"); 
    }

    public void Salir()
    {
        
        // Cierra la aplicación (esto solo hace efecto cuando el juego ya está compilado o "buildeado")
        Application.Quit();
    }
}