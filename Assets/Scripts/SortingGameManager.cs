using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SortingGameManager : MonoBehaviour
{
    [Header("Interfaz y Métricas de Progreso")]
    public TMP_Text scoreText; 
    public int currentScore = 0;
    private const int maxScore = 6;

    [Header("Retroalimentación Auditiva")]
    public AudioSource audioSource; 
    public AudioClip correctSound; 
    public AudioClip wrongSound; 

    [Header("Eventos de Transición")]
    public DoorOpener doorOpener; 

    void Start()
    {
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Puntos: " + currentScore + "/" + maxScore;
    }

    // --- CALLBACKS DE VALIDACIÓN: CONTENEDOR ROJO ---
    public void OnObjectDroppedInRedSocket(SelectEnterEventArgs args)
    {
        // Extracción de la referencia al GameObject interactuado desde el evento del XR Socket
        GameObject itemDropped = args.interactableObject.transform.gameObject;

        // Validación de categorización mediante etiquetas espaciales (Target: Pelotas)
        if (itemDropped.CompareTag("Pelotas"))
        {
            Action_CorrectEntry(itemDropped);
        }
        else
        {
            Action_WrongEntry(itemDropped);
        }
    }

    // --- CALLBACKS DE VALIDACIÓN: CONTENEDOR AZUL ---
    public void OnObjectDroppedInBlueSocket(SelectEnterEventArgs args)
    {
        GameObject itemDropped = args.interactableObject.transform.gameObject;

        // Validación de categorización mediante etiquetas espaciales (Target: Libros)
        if (itemDropped.CompareTag("Libros"))
        {
            Action_CorrectEntry(itemDropped);
        }
        else
        {
            Action_WrongEntry(itemDropped);
        }
    }

    // --- MÉTODOS DE RESOLUCIÓN LÓGICA ---

    private void Action_CorrectEntry(GameObject item)
    {
        if (audioSource && correctSound) audioSource.PlayOneShot(correctSound);

        // Desactivamos el renderizado y las físicas del objeto una vez categorizado correctamente
        item.SetActive(false);

        currentScore++;
        UpdateScoreUI();

        // Verificación de la condición de victoria para la apertura de la siguiente fase
        if (currentScore >= maxScore)
        {
            scoreText.text = "¡Perfecto! ¡Ahora puedes ir a la siguiente habitación!";

            if (doorOpener != null)
            {
                doorOpener.OpenDoor();
            }
        }
    }

    private void Action_WrongEntry(GameObject item)
    {
        if (audioSource && wrongSound) audioSource.PlayOneShot(wrongSound);

        // Restauración espacial del objeto para evitar frustración o bloqueos cognitivos en el paciente
        ItemReturner returner = item.GetComponent<ItemReturner>();
        if (returner != null)
        {
            returner.ReturnToStart();
        }
        else
        {
            Debug.LogWarning($"Validación: El objeto '{item.name}' carece del componente ItemReturner para su restauración espacial.");
        }
    }
}
