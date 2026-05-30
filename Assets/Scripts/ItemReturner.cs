using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactables; 

public class ItemReturner : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;
    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    public void ReturnToStart()
    {
        StartCoroutine(EscapeFromSocketRoutine());
    }

    private IEnumerator EscapeFromSocketRoutine()
    {
        // Deshabilitamos temporalmente la interacción para forzar la liberación del objeto de cualquier XR Socket o mano
        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
        }

        // Retrasamos la ejecución un ciclo del motor para garantizar la desvinculación física antes de la traslación
        yield return null;

        // Restauramos las coordenadas matriciales de origen
        transform.position = startPosition;
        transform.rotation = startRotation;

        // Neutralizamos la velocidad y la inercia del Rigidbody para evitar comportamientos erráticos
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (grabInteractable != null)
        {
            grabInteractable.enabled = true;
        }
    }
}
