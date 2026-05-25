using UnityEngine;

public class AnilloDiana : MonoBehaviour
{
    [Header("Configuración de Puntos")]
    [Tooltip("¿Cuántos puntos vale golpear este anillo específico?")]
    public int valorPuntos = 10;

    [Header("Sonido Local")]
    [Tooltip("Opcional: Sonido de 'pop' al golpear la diana")]
    public AudioClip sonidoImpacto;
    private AudioSource audioSource;

    private SalonGameManager gameManager;

    void Start()
    {
        // Buscamos el GameManager automáticamente al empezar
        gameManager = FindObjectOfType<SalonGameManager>();

        // Preparamos el AudioSource para el sonido local
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && sonidoImpacto != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Esta función detecta choques físicos (no triggers)
    void OnCollisionEnter(Collision collision)
    {
        // Comprobamos si lo que nos ha chocado tiene el Tag "Proyectil"
        if (collision.gameObject.CompareTag("Proyectil"))
        {
            // Sonido local de impacto
            if (audioSource && sonidoImpacto) audioSource.PlayOneShot(sonidoImpacto);

            // Avisamos al GameManager y le pasamos nuestros puntos
            if (gameManager != null)
            {
                gameManager.RegistrarImpactoDiana(valorPuntos);
            }

            // Opcional: Destruir el dardo para no llenar la escena
            Destroy(collision.gameObject);
        }
    }
}