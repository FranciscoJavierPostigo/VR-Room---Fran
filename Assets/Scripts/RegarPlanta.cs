using UnityEngine;

public class RegarPlanta : MonoBehaviour
{
    [Header("Configuración del Umbral")]
    [Tooltip("Cantidad de colisiones de partículas requeridas para validar la tarea.")]
    public int gotasNecesarias = 80; 
    
    private int gotasActuales = 0;
    private bool yaRegada = false;

    [Header("Dependencias del Sistema")]
    public SalonGameManager gameManager; 

    void OnParticleCollision(GameObject other)
    {
        if (yaRegada) return;

        gotasActuales++;

        // Validación del estado físico una vez alcanzado el umbral acumulativo de partículas
        if (gotasActuales >= gotasNecesarias)
        {
            yaRegada = true;
            
            if (gameManager != null)
            {
                gameManager.MisionPlantaCompletada();
            }
        }
    }
}
