using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables; 
public class ControlFantasmaLego : MonoBehaviour
{
    [Header("Arrastra aquí el fantasma de esta pieza")]
    public GameObject miFantasma;

    private XRGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Nos aseguramos de que empiece apagado por si se nos olvida en el editor
        if (miFantasma != null) miFantasma.SetActive(false);

        // Le decimos al código que escuche automáticamente cuando agarramos y soltamos la pieza
        grabInteractable.selectEntered.AddListener(AlAgarrar);
        grabInteractable.selectExited.AddListener(AlSoltar);
    }

    private void AlAgarrar(SelectEnterEventArgs args)
    {
        // MAGIA: Solo encendemos el fantasma si lo agarra el jugador (una mano).
        // Si lo "agarra" el Socket al encajarlo, lo ignoramos para que no vuelva a aparecer.
        if (!(args.interactorObject is XRSocketInteractor))
        {
            if (miFantasma != null) miFantasma.SetActive(true);
        }
    }

    private void AlSoltar(SelectExitEventArgs args)
    {
        // Al soltarlo en la mesa o al encajarlo en el socket, el fantasma se apaga siempre.
        if (miFantasma != null) miFantasma.SetActive(false);
    }
}