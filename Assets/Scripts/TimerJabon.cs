using UnityEngine;
using UnityEngine.Events;

public class TimerJabon : MonoBehaviour
{
    [Header("Configuración de Temporización")]
    [Tooltip("Tiempo continuo de manipulación requerido para validar la fase de enjabonado (en segundos).")]
    public float tiempoParaEnjabonar = 4f;
    
    private float tiempoTranscurrido = 0f;
    private bool agarrado = false;
    private bool yaEnjabonado = false;

    [Header("Eventos de Transición")]
    public UnityEvent OnTiempoCumplido; 

    // Callbacks expuestos para la integración con los eventos de selección del XR Grab Interactable
    public void EmpezarContar() { agarrado = true; }
    public void PararContar() { agarrado = false; tiempoTranscurrido = 0f; }

    void Update()
    {
        // Evaluación continua del tiempo de manipulación activa
        if (agarrado && !yaEnjabonado)
        {
            tiempoTranscurrido += Time.deltaTime;
            
            if (tiempoTranscurrido >= tiempoParaEnjabonar)
            {
                yaEnjabonado = true;
                OnTiempoCumplido.Invoke();
                Debug.Log("Validación: Umbral temporal de manipulación alcanzado (Enjabonado completado).");
            }
        }
    }
}
