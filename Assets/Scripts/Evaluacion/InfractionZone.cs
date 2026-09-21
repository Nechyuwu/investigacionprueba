using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class InfractionZone : MonoBehaviour
{
    public string playerTag = "Player";
    public float tiempoTolerancia = 2.0f;

    private float temporizador = 0f;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        temporizador += Time.deltaTime;

        if (temporizador >= tiempoTolerancia)
        {
            if (EvaluationUI.Instance != null)
            {
                // Cambiado a RegistrarInfraccionCarril()
                EvaluationUI.Instance.RegistrarInfraccionCarril(); 
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            temporizador = 0f;
        }
    }
}