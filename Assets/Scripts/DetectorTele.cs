using UnityEngine;
using UnityEngine.Video;

public class DetectorTele : MonoBehaviour
{
    [Header("Dependencias del Sistema")]
    public SalonGameManager gameManager;

    [Header("Componentes Multimedia")]
    [Tooltip("Referencia al reproductor de vídeo integrado en la pantalla")]
    public VideoPlayer reproductorVideo;

    private bool yaEncendida = false;

    void Update()
    {
        // Evaluamos el cambio de estado del reproductor para registrar la acción una única vez
        if (!yaEncendida && reproductorVideo != null && reproductorVideo.isPlaying)
        {
            yaEncendida = true;

            if (gameManager != null)
            {
                gameManager.MisionTeleCompletada();
            }
        }
    }
}
