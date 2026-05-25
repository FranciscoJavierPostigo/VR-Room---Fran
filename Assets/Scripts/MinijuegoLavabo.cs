using UnityEngine;
using UnityEngine.Events;

public class MinijuegoLavabo : MonoBehaviour
{
    [Header("Ajustes del Minijuego")]
    public bool manosEnjabonadas = false;
    public float tiempoNecesario = 5f;
    private float tiempoActual = 0f;
    private bool minijuegoTerminado = false;

    [Header("Efectos Opcionales")]
    public ParticleSystem particulasAgua;
    public AudioSource sonidoAgua;

    [Header("Evento al Terminar")]
    public UnityEvent OnLavadoCompletado;

    // Esta función la llamará la pastilla de jabón al ser agarrada
    public void EnjabonarManos()
    {
        if (!minijuegoTerminado)
        {
            manosEnjabonadas = true;
            Debug.Log("El niño se ha puesto jabón.");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Si ya ha terminado, no hacemos nada
        if (minijuegoTerminado) return;

        // Comprobamos si lo que ha entrado en el grifo es una "Manos"
        if (other.CompareTag("Manos"))
        {
            if (manosEnjabonadas)
            {
                // Encendemos el agua (si tienes puestas partículas o sonido)
                if (particulasAgua != null && !particulasAgua.isPlaying) particulasAgua.Play();
                if (sonidoAgua != null && !sonidoAgua.isPlaying) sonidoAgua.Play();

                // Empezamos a contar el tiempo
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
        // Si saca las manos antes de tiempo, paramos el agua
        if (other.CompareTag("Manos") && !minijuegoTerminado)
        {
            if (particulasAgua != null) particulasAgua.Stop();
            if (sonidoAgua != null) sonidoAgua.Stop();
        }
    }

    private void CompletarLavado()
    {
        minijuegoTerminado = true;

        // Apagamos el grifo
        if (particulasAgua != null) particulasAgua.Stop();
        if (sonidoAgua != null) sonidoAgua.Stop();

        Debug.Log("¡Lavado completado!");

        // Esto avisa al GameManager (o a lo que conectemos en Unity) de que ha terminado
        OnLavadoCompletado.Invoke();
    }
}