using UnityEngine;
using UnityEngine.Events;

public class TimerJabon : MonoBehaviour
{
    public float tiempoParaEnjabonar = 4f;
    private float tiempoTranscurrido = 0f;
    private bool agarrado = false;
    private bool yaEnjabonado = false;

    public UnityEvent OnTiempoCumplido; // Aquí conectaremos el lavabo

    // Estos métodos los llamaremos desde los eventos del XR Grab
    public void EmpezarContar() { agarrado = true; }
    public void PararContar() { agarrado = false; tiempoTranscurrido = 0f; }

    void Update()
    {
        if (agarrado && !yaEnjabonado)
        {
            tiempoTranscurrido += Time.deltaTime;
            if (tiempoTranscurrido >= tiempoParaEnjabonar)
            {
                yaEnjabonado = true;
                OnTiempoCumplido.Invoke();
                Debug.Log("¡Manos enjabonadas tras 4 segundos!");
            }
        }
    }
}