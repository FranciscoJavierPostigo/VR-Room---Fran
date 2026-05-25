using UnityEngine;

public class SonidoBote : MonoBehaviour
{
    // Variables para guardar nuestros componentes
    private AudioSource audioSource;
    private Rigidbody rb;

    void Start()
    {
        // Al empezar, le decimos al script quiénes son el AudioSource y el Rigidbody de la pelota
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    // Esta función mágica de Unity se activa automáticamente CADA VEZ que la pelota choca con algo
    void OnCollisionEnter(Collision collision)
    {
        // BONUS: Calculamos la fuerza del impacto. 
        // collision.relativeVelocity.magnitude nos da la velocidad exacta del golpe
        float fuerzaImpacto = collision.relativeVelocity.magnitude;

        // Convertimos esa fuerza en un volumen de 0 a 1 (dividimos entre 5 para suavizarlo un poco, 
        // puedes cambiar este número si suena muy fuerte o muy flojo)
        float volumen = fuerzaImpacto / 5f;

        // Nos aseguramos de que el volumen no pase de 1
        volumen = Mathf.Clamp01(volumen);

        // Si el golpe es muy, muy flojito (ej: está rodando por el suelo), no hacemos ruido
        if (volumen > 0.1f)
        {
            // ¡Reproducimos el sonido de bote (AudioClip) con el volumen calculado!
            audioSource.PlayOneShot(audioSource.clip, volumen);
        }
    }
}