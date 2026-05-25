using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SortingGameManager : MonoBehaviour
{
    [Header("UI & Puntos")]
    public TMP_Text scoreText; // Arrastra TextoPuntaje aqu�
    public int currentScore = 0;
    private const int maxScore = 6;

    [Header("Sonidos")]
    public AudioSource audioSource; // Necesario para reproducir sonido
    public AudioClip correctSound; // Sonido de acierto
    public AudioClip wrongSound; // Sonido de error

    [Header("Resultados")]
    public DoorOpener doorOpener; // Arrastra el objeto con el script de la puerta

    void Start()
    {
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Puntos: " + currentScore + "/" + maxScore;
    }

    // --- FUNCIONES PÚBLICAS PARA EL CUBO ROJO ---

    // Llamado cuando algo entra en el Socket Rojo
    public void OnObjectDroppedInRedSocket(SelectEnterEventArgs args)
    {
        // En Unity 6 sacamos el GameObject real desde los "args" (argumentos)
        GameObject itemDropped = args.interactableObject.transform.gameObject;

        // LÓGICA CORRECTA: Pelota en Rojo
        if (itemDropped.CompareTag("Pelotas"))
        {
            Action_CorrectEntry(itemDropped);
        }
        // LÓGICA INCORRECTA: Cualquier otra cosa (Libro u Untagged)
        else
        {
            Action_WrongEntry(itemDropped);
        }
    }

    // --- FUNCIONES PÚBLICAS PARA EL CUBO AZUL ---

    public void OnObjectDroppedInBlueSocket(SelectEnterEventArgs args)
    {
        GameObject itemDropped = args.interactableObject.transform.gameObject;

        // LÓGICA CORRECTA: Libro en Azul
        if (itemDropped.CompareTag("Libros"))
        {
            Action_CorrectEntry(itemDropped);
        }
        // LÓGICA INCORRECTA: Cualquier otra cosa (Pelota u Untagged)
        else
        {
            Action_WrongEntry(itemDropped);
        }
    }


    // --- ACCIONES GENERALES ---

    private void Action_CorrectEntry(GameObject item)
    {
        // 1. Sonido acierto
        if (audioSource && correctSound) audioSource.PlayOneShot(correctSound);

        // 2. Desaparece del cubo/escena (lo desactivamos)
        item.SetActive(false);

        // 3. Sube un punto y actualiza UI
        currentScore++;
        UpdateScoreUI();

        // 4. Comprobar si hemos ganado
        if (currentScore >= maxScore)
        {
            // --- AÑADIMOS SOLO LA LÍNEA DEL TEXTO AQUÍ ---
            scoreText.text = "¡Perfecto! ¡Ahora puedes ir a la siguiente habitación!";

            if (doorOpener != null)
            {
                doorOpener.OpenDoor();
            }
        }
    }

    private void Action_WrongEntry(GameObject item)
    {
        // 1. Sonido error
        if (audioSource && wrongSound) audioSource.PlayOneShot(wrongSound);

        // 2. Vuelve a su sitio de origen
        ItemReturner returner = item.GetComponent<ItemReturner>();
        if (returner != null)
        {
            returner.ReturnToStart();
        }
        else
        {
            Debug.LogWarning("El objeto equivocado " + item.name + " no tiene ItemReturner script, no puede volver.");
        }
    }
}