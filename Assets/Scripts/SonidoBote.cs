using UnityEngine;

public class SonidoBote : MonoBehaviour
{
    private AudioSource audioSource;
    private Rigidbody rb;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Cálculo del módulo de la velocidad relativa para determinar la energía del impacto físico
        float fuerzaImpacto = collision.relativeVelocity.magnitude;

        // Atenuación lineal de la magnitud para mapearla al rango de amplitud del componente de audio [0, 1]
        float volumen = fuerzaImpacto / 5f;
        volumen = Mathf.Clamp01(volumen);

        // Umbral de ruido (Noise Gate): filtramos colisiones de baja energía (ej. rotación o fricción continua por el suelo)
        if (volumen > 0.1f)
        {
            audioSource.PlayOneShot(audioSource.clip, volumen);
        }
    }
}
