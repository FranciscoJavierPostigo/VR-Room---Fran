using UnityEngine;

public class DetectorVela : MonoBehaviour
{
    [Header("Conexiones")]
    public SalonGameManager gameManager;
    public GameObject particulaFuego; // Arrastra aquí el objeto Particle_Flame

    private bool yaEncendida = false;

    void Update()
    {
        // Si la vela no estaba encendida, pero de repente la partícula se activa...
        if (!yaEncendida && particulaFuego != null && particulaFuego.activeInHierarchy)
        {
            yaEncendida = true; // Marcamos que ya está encendida para no repetir

            // Avisamos al GameManager de que hemos ganado
            if (gameManager != null)
            {
                gameManager.MisionVelaCompletada();
            }
        }
    }
}