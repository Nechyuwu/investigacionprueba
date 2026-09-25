
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EvaluacionVelocidad : MonoBehaviour
{
    [Header("Configuración de Velocidad")]
    [Tooltip("Límite de velocidad permitido en esta zona (km/h)")]
    public float maxSpeedLimit = 50f;

    [Tooltip("Segundos continuos sobre el límite antes de marcar la falta")]
    public float tiempoTolerancia = 1.5f;

    [Header("Configuración del Jugador")]
    public string playerTag = "Player";

    private float temporizadorExceso = 0f;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        // Busca el componente PrometeoCarController en el objeto o en sus padres
        PrometeoCarController carController = other.GetComponentInParent<PrometeoCarController>();
        if (carController == null)
        {
            carController = other.GetComponent<PrometeoCarController>();
        }

        if (carController != null)
        {
            // Obtenemos la velocidad absoluta actual directa de PrometeoCarController
            float currentSpeed = Mathf.Abs(carController.carSpeed);

            if (currentSpeed > maxSpeedLimit)
            {
                temporizadorExceso += Time.deltaTime;

                if (temporizadorExceso >= tiempoTolerancia)
                {
                    if (EvaluationUI.Instance != null)
                    {
                        EvaluationUI.Instance.RegistrarInfraccionVelocidad();
                        Debug.LogWarning($"Infracción: Exceso de velocidad ({currentSpeed:F1} km/h en zona de {maxSpeedLimit} km/h)");
                    }
                }
            }
            else
            {
                // Si el conductor frena por debajo del límite, reiniciamos el temporizador
                temporizadorExceso = 0f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            temporizadorExceso = 0f;
        }
    }
}
