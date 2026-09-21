using UnityEngine;

public enum TrafficLightState { Green, Yellow, Red }

[RequireComponent(typeof(BoxCollider))]
public class TrafficLight : MonoBehaviour
{
    [Header("Tiempos de cada luz (Segundos)")]
    public float greenDuration = 8f;
    public float yellowDuration = 3f;
    public float redDuration = 8f;

    [Header("Visuales de las Luces (GameObjects o Meshes)")]
    public GameObject greenLightObject;
    public GameObject yellowLightObject;
    public GameObject redLightObject;

    [Header("Configuración de Infracción")]
    public string playerTag = "Player";

    public TrafficLightState CurrentState { get; private set; }
    private float timer = 0f;

    private void Start()
    {
        SetState(TrafficLightState.Green);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        switch (CurrentState)
        {
            case TrafficLightState.Green:
                if (timer >= greenDuration) SetState(TrafficLightState.Yellow);
                break;

            case TrafficLightState.Yellow:
                if (timer >= yellowDuration) SetState(TrafficLightState.Red);
                break;

            case TrafficLightState.Red:
                if (timer >= redDuration) SetState(TrafficLightState.Green);
                break;
        }
    }

    private void SetState(TrafficLightState newState)
    {
        CurrentState = newState;
        timer = 0f;

        // Encender/apagar objetos visuales según la luz activa
        if (greenLightObject != null) greenLightObject.SetActive(newState == TrafficLightState.Green);
        if (yellowLightObject != null) yellowLightObject.SetActive(newState == TrafficLightState.Yellow);
        if (redLightObject != null) redLightObject.SetActive(newState == TrafficLightState.Red);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        // Si el jugador cruza la línea de detención durante la luz roja
        if (CurrentState == TrafficLightState.Red)
        {
            if (EvaluationUI.Instance != null)
            {
                EvaluationUI.Instance.RegistrarInfraccionSemaforo();
                Debug.LogWarning("Infracción: Cruzó el semáforo en luz roja.");
            }
        }
    }
}