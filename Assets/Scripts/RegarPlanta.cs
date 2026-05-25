using UnityEngine;

public class RegarPlanta : MonoBehaviour
{
    [Header("Ajustes de Riego")]
    public int gotasNecesarias = 80; // Sube o baja este número para cambiar la dificultad
    private int gotasActuales = 0;
    private bool yaRegada = false;

    [Header("Conexión")]
    public SalonGameManager gameManager; // Para avisar de que hemos ganado

    // Esta función mágica de Unity se activa cada vez que una partícula choca con este objeto
    void OnParticleCollision(GameObject other)
    {
        // Si ya terminamos, no hacemos nada más
        if (yaRegada) return;

        // Sumamos una gota
        gotasActuales++;

        // Puedes descomentar la siguiente línea si quieres ver en la consola cómo se llena
        // Debug.Log("Gotas recibidas: " + gotasActuales);

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