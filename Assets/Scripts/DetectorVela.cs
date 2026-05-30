using UnityEngine;

public class DetectorVela : MonoBehaviour
{
    [Header("Dependencias del Sistema")]
    public SalonGameManager gameManager;

    [Header("Componentes Visuales")]
    [Tooltip("Referencia al GameObject (Particle System) que representa la llama de la vela")]
    public GameObject particulaFuego;

    private bool yaEncendida = false;

    void Update()
    {
        // Monitorizamos la jerarquía activa para registrar el cambio de estado físico una única vez
        if (!yaEncendida && particulaFuego != null && particulaFuego.activeInHierarchy)
        {
            yaEncendida = true;

            if (gameManager != null)
            {
                gameManager.MisionVelaCompletada();
            }
        }
    }
}
