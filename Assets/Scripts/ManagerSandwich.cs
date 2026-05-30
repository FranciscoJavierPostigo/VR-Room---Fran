using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit; 

public class ManagerSandwich : MonoBehaviour
{
    [Header("Andamiaje Visual y Secuenciación")]
    [Tooltip("Mallas translúcidas que guían la secuencia lógica del paciente paso a paso.")]
    public GameObject[] fantasmas; 
    
    [Tooltip("Receptores XR Socket ordenados cronológicamente según la receta.")]
    public GameObject[] sockets;  

    private int pasoActual = 0;

    [Header("Eventos de Finalización")]
    public UnityEvent OnSandwichTerminado;

    void Start()
    {
        ActualizarVisuales();
    }

    public void AvanzarPaso(SelectEnterEventArgs args)
    {
        GameObject ingrediente = args.interactableObject.transform.gameObject;

        // Convertimos el objeto en cinemático para neutralizar las físicas y evitar interpenetraciones 
        // o colisiones erráticas (jittering) al superponer múltiples mallas en un espacio reducido.
        Rigidbody rb = ingrediente.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true; 

        // Deshabilitamos el colisionador para consolidar la pieza en la estructura final 
        // y prevenir que el usuario la extraiga accidentalmente, rompiendo la secuencia.
        Collider col = ingrediente.GetComponent<Collider>();
        if (col != null) col.enabled = false;  

        fantasmas[pasoActual].SetActive(false);

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
            // Solo renderizamos el andamiaje visual correspondiente al paso activo
            fantasmas[i].SetActive(i == pasoActual);

            if (i > pasoActual)
            {
                // Desactivamos los receptores de pasos posteriores para forzar un ensamblaje 
                // secuencial estricto, evitando que el paciente anticipe piezas incorrectas.
                sockets[i].SetActive(false);
            }
            else if (i == pasoActual)
            {
                sockets[i].SetActive(true);
            }
            // Nota arquitectónica: Los Sockets previos (i < pasoActual) no se modifican; 
            // deben permanecer activos para sostener el vínculo (Attach Transform) con su ingrediente.
        }
    }

    void TerminarMinijuego()
    {
        Debug.Log("Evento: Minijuego de secuenciación (Cocina) completado con éxito.");
        OnSandwichTerminado.Invoke();
    }
}
