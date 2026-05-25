using UnityEngine;
using System.Collections; // Necesario para Coroutines

public class DoorOpener : MonoBehaviour
{
    public float targetYRotationMovement = 88f; // Cuántos grados mover en Y
    public float openingSpeed = 2f; // Velocidad de la animación (grados por segundo * multiplicador)

    private bool doorIsOpening = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        // Guardamos la rotación inicial de la puerta como "Cerrada"
        closedRotation = transform.localRotation; // Usamos LOCAL para que no dependa de hacia dónde mira la habitación

        // Calculamos la rotación "Abierta" sumando el movimiento en Y
        // Esto añade +88 grados a la rotación Y actual
        openRotation = closedRotation * Quaternion.Euler(0, targetYRotationMovement, 0);
    }

    // Función pública que llamará el Manager
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
        float duration = 2.0f; // Tiempo que tarda en abrirse completamente (puedes ajustarlo)

        while (elapsedTime < duration)
        {
            // Interpola suavemente entre cerrada y abierta
            transform.localRotation = Quaternion.Slerp(closedRotation, openRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Espera al siguiente frame
        }

        // Nos aseguramos de que termine exactamente en la rotación final
        transform.localRotation = openRotation;
        Debug.Log("La puerta se ha abierto.");
    }
}