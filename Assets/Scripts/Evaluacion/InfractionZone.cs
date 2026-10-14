using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class InfractionZone : MonoBehaviour
{
    [Header("UI de Advertencia")]
    [Tooltip("Arrastra aquí el GameObject Advertencia del Canvas")]
    public GameObject objetoAdvertencia;

    [Header("Configuración")]
    public string playerTag = "Player";
    public float tiempoTolerancia = 2.0f;

    private float temporizador = 0f;

    private void Start()
    {
        // Se asegura de que la advertencia empiece oculta al iniciar el juego
        if (objetoAdvertencia != null)
        {
            objetoAdvertencia.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Muestra la advertencia en pantalla inmediatamente al entrar al sentido contrario
        if (other.CompareTag(playerTag))
        {
            if (objetoAdvertencia != null)
            {
                objetoAdvertencia.SetActive(true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        temporizador += Time.deltaTime;

        // Si pasa el tiempo de tolerancia, registra la falta para la evaluación final
        if (temporizador >= tiempoTolerancia)
        {
            if (EvaluationUI.Instance != null)
            {
                EvaluationUI.Instance.RegistrarInfraccionCarril();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            temporizador = 0f;

            // Oculta la advertencia al salir de la zona prohibida
            if (objetoAdvertencia != null)
            {
                objetoAdvertencia.SetActive(false);
            }
        }
    }
}