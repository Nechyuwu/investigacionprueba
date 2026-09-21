using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EvaluationTrigger : MonoBehaviour
{
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (EvaluationUI.Instance != null)
            {
                EvaluationUI.Instance.MostrarResultado();
            }
        }
    }
}