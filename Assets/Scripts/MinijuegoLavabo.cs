using UnityEngine;
using UnityEngine.Events;

public class MinijuegoLavabo : MonoBehaviour
{
    [Header("Mecánica de Lavado")]
    [Tooltip("Estado booleano que determina si el paciente ha interactuado previamente con el jabón sólido.")]
    public bool manosEnjabonadas = false;
    
    [Tooltip("Tiempo de exposición continua requerido bajo el flujo de agua (en segundos).")]
    public float tiempoNecesario = 5f;
    
    private float tiempoActual = 0f;
    private bool minijuegoTerminado = false;

    [Header("Retroalimentación Sensorial")]
    public ParticleSystem particulasAgua;
    public AudioSource sonidoAgua;

    [Header("Eventos de Finalización")]
    public UnityEvent OnLavadoCompletado;

    public void EnjabonarManos()
    {
        if (!minijuegoTerminado)
        {
            manosEnjabonadas = true;
            Debug.Log("Evento: Fase de enjabonado validada.");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (minijuegoTerminado) return;

        // Validación espacial de los avatares de las manos dentro de la zona de activación del grifo
        if (other.CompareTag("Manos"))
        {
            if (manosEnjabonadas)
            {
                if (particulasAgua != null && !particulasAgua.isPlaying) particulasAgua.Play();
                if (sonidoAgua != null && !sonidoAgua.isPlaying) sonidoAgua.Play();

                // Acumulación del tiempo de permanencia requerido para validar la tarea terapéutica
                tiempoActual += Time.deltaTime;

                if (tiempoActual >= tiempoNecesario)
                {
                    CompletarLavado();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Interrupción de los estímulos visuales y sonoros si el usuario retira las manos prematuramente
        if (other.CompareTag("Manos") && !minijuegoTerminado)
        {
            if (particulasAgua != null) particulasAgua.Stop();
            if (sonidoAgua != null) sonidoAgua.Stop();
        }
    }

    private void CompletarLavado()
    {
        minijuegoTerminado = true;

        if (particulasAgua != null) particulasAgua.Stop();
        if (sonidoAgua != null) sonidoAgua.Stop();

        Debug.Log("Evento: Rutina de higiene personal (Baño) completada con éxito.");
        
        OnLavadoCompletado.Invoke();
    }
}
