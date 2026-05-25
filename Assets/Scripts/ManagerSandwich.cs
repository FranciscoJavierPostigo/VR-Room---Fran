using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit; // Necesario para los eventos del Socket

public class ManagerSandwich : MonoBehaviour
{
    [Header("Secuencia de Pasos (Fantasmas)")]
    public GameObject[] fantasmas; // Arrastra aquí los fantasmas en orden
    public GameObject[] sockets;   // Arrastra aquí los sockets en orden

    private int pasoActual = 0;

    [Header("Eventos finales")]
    public UnityEvent OnSandwichTerminado;

    void Start()
    {
        ActualizarVisuales();
    }

    // AHORA RECIBE EL EVENTO PARA SABER QUÉ PIEZA HEMOS PUESTO
    public void AvanzarPaso(SelectEnterEventArgs args)
    {
        // 1. LA MAGIA DE LOS LEGOS: Congelamos el ingrediente para que no explote
        GameObject ingrediente = args.interactableObject.transform.gameObject;

        Rigidbody rb = ingrediente.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true; // Adiós temblores

        Collider col = ingrediente.GetComponent<Collider>();
        if (col != null) col.enabled = false;  // Adiós choques y no se puede volver a coger

        // 2. Apagamos el fantasma que ya hemos completado
        fantasmas[pasoActual].SetActive(false);

        // OJO: NO apagamos el socket actual, lo dejamos encendido para que siga sujetando el ingrediente para siempre.

        pasoActual++;

        if (pasoActual < fantasmas.Length)
        {
            ActualizarVisuales();
        }
        else
        {
            TerminarMinijuego();
        }
    }

    void ActualizarVisuales()
    {
        for (int i = 0; i < fantasmas.Length; i++)
        {
            // Solo encendemos el fantasma del paso actual
            fantasmas[i].SetActive(i == pasoActual);

            // Lógica de los Sockets:
            if (i > pasoActual)
            {
                // Los sockets del futuro se apagan para que no metan ingredientes adelantados
                sockets[i].SetActive(false);
            }
            else if (i == pasoActual)
            {
                // Encendemos el socket que toca ahora
                sockets[i].SetActive(true);
            }
            // Los sockets del pasado (i < pasoActual) no se tocan. Se quedan encendidos sujetando su ingrediente.
        }
    }

    void TerminarMinijuego()
    {
        Debug.Log("Sándwich terminado!");
        OnSandwichTerminado.Invoke();
    }
}