using UnityEngine;
using System.Collections;

public class DoorOpener : MonoBehaviour
{
    [Header("Cinemática de la Puerta")]
    [Tooltip("Desplazamiento angular objetivo en el eje Y (grados)")]
    public float targetYRotationMovement = 88f; 
    
    [Tooltip("Velocidad o multiplicador para la interpolación de apertura")]
    public float openingSpeed = 2f; 

    private bool doorIsOpening = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        // Almacenamos la rotación en espacio local para independizar la cinemática de la orientación global de la sala
        closedRotation = transform.localRotation; 

        // Cálculo del cuaternión objetivo mediante la composición de rotaciones
        openRotation = closedRotation * Quaternion.Euler(0, targetYRotationMovement, 0);
    }

    public void OpenDoor()
    {
        if (!doorIsOpening)
        {
            StartCoroutine(AnimateDoorOpen());
        }
    }

    private IEnumerator AnimateDoorOpen()
    {
        doorIsOpening = true;
        float elapsedTime = 0f;
        float duration = 2.0f; 

        while (elapsedTime < duration)
        {
            transform.localRotation = Quaternion.Slerp(closedRotation, openRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; 
        }

        // Garantizamos la precisión posicional al finalizar la interpolación (evitando micro-desviaciones de coma flotante)
        transform.localRotation = openRotation;
        Debug.Log("Evento: Puerta abierta con éxito.");
    }
}
