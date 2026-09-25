using UnityEngine;
using TMPro;

public class EvaluationUI : MonoBehaviour
{
    public static EvaluationUI Instance { get; private set; }

    [Header("Referencias UI")]
    public GameObject resultPanel;
    public TMP_Text statusCarrilText;
    public TMP_Text statusSemaforoText;
    public TMP_Text statusVelocidadText;

    private bool infraccionCarril = false;
    private bool infraccionSemaforo = false;
    private bool infraccionVelocidad = false;

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

    public void RegistrarInfraccionVelocidad()
    {
        infraccionVelocidad = true;
    }

    public void MostrarResultado()
    {
        if (resultPanel != null)
            resultPanel.SetActive(true);

        // Evaluación Carril
        if (statusCarrilText != null)
        {
            statusCarrilText.text = infraccionCarril ? "NO (✗)" : "SÍ (✓)";
            statusCarrilText.color = infraccionCarril ? Color.red : Color.green;
        }

        // Evaluación Semáforo
        if (statusSemaforoText != null)
        {
            statusSemaforoText.text = infraccionSemaforo ? "NO (✗)" : "SÍ (✓)";
            statusSemaforoText.color = infraccionSemaforo ? Color.red : Color.green;
        }

        // Evaluación Velocidad
        if (statusVelocidadText != null)
        {
            statusVelocidadText.text = infraccionVelocidad ? "NO (✗)" : "SÍ (✓)";
            statusVelocidadText.color = infraccionVelocidad ? Color.red : Color.green;
        }
    }
}