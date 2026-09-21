using UnityEngine;
using TMPro;

public class EvaluationUI : MonoBehaviour
{
    public static EvaluationUI Instance { get; private set; }

    [Header("Referencias UI")]
    public GameObject resultPanel;
    public TMP_Text statusCarrilText;
    public TMP_Text statusSemaforoText;

    private bool infraccionCarril = false;
    private bool infraccionSemaforo = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegistrarInfraccionCarril()
    {
        infraccionCarril = true;
    }

    public void RegistrarInfraccionSemaforo()
    {
        infraccionSemaforo = true;
    }

    public void MostrarResultado()
    {
        if (resultPanel != null)
            resultPanel.SetActive(true);

        // Evaluacion de Carril
        if (statusCarrilText != null)
        {
            statusCarrilText.text = infraccionCarril ? "NO (✗)" : "SÍ (✓)";
            statusCarrilText.color = infraccionCarril ? Color.red : Color.green;
        }

        // Evaluacion de Semaforo
        if (statusSemaforoText != null)
        {
            statusSemaforoText.text = infraccionSemaforo ? "NO (✗)" : "SÍ (✓)";
            statusSemaforoText.color = infraccionSemaforo ? Color.red : Color.green;
        }
    }
}