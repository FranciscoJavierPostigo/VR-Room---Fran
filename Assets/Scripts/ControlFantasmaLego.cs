using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables; 

public class ControlFantasmaLego : MonoBehaviour
{
    [Header("Andamiaje Visual")]
    public GameObject miFantasma;

    private XRGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (miFantasma != null) 
            miFantasma.SetActive(false);

        grabInteractable.selectEntered.AddListener(AlAgarrar);
        grabInteractable.selectExited.AddListener(AlSoltar);
    }

    private void AlAgarrar(SelectEnterEventArgs args)
    {
        // Evitamos reactivar la malla translúcida si el evento Select es disparado por un encaje automático (Socket)
        if (!(args.interactorObject is XRSocketInteractor))
        {
            if (miFantasma != null) 
                miFantasma.SetActive(true);
        }
    }

    private void AlSoltar(SelectExitEventArgs args)
    {
        if (miFantasma != null) 
            miFantasma.SetActive(false);
    }
}
