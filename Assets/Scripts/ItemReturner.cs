using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactables; // Necesario en Unity 6 para el XRGrabInteractable

public class ItemReturner : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;
    private XRGrabInteractable grabInteractable; // Referencia al componente que permite agarrarlo

    void Awake()
    {
        // Guardamos dónde empieza
        startPosition = transform.position;
        startRotation = transform.rotation;

        // Buscamos el componente de agarre VR
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    public void ReturnToStart()
    {
        StartCoroutine(EscapeFromSocketRoutine());
    }

    private IEnumerator EscapeFromSocketRoutine()
    {
        // 1. Apagamos SOLO el componente de agarre (así el cubo lo suelta por la fuerza)
        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
        }

        // 2. Esperamos un frame
        yield return null;

        // 3. Lo movemos a su sitio de origen
        transform.position = startPosition;
        transform.rotation = startRotation;

        // 4. Paramos su velocidad física
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // 5. Volvemos a encender el componente de agarre para que puedas volver a cogerlo
        if (grabInteractable != null)
        {
            grabInteractable.enabled = true;
        }
    }
}