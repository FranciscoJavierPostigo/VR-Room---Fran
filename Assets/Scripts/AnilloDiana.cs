using UnityEngine;

public class AnilloDiana : MonoBehaviour
{
    [Header("Configuración de Puntos")]
    [Tooltip("Puntuación otorgada por impactar en este anillo")]
    public int valorPuntos = 10;

    [Header("Audio")]
    public AudioClip sonidoImpacto;
    private AudioSource audioSource;

    private SalonGameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<SalonGameManager>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && sonidoImpacto != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Proyectil"))
        {
            if (audioSource && sonidoImpacto) 
                audioSource.PlayOneShot(sonidoImpacto);

            if (gameManager != null)
            {
                gameManager.RegistrarImpactoDiana(valorPuntos);
            }

            // Destruimos el proyectil para evitar saturar la escena
            Destroy(collision.gameObject);
        }
    }
}
