using UnityEngine;
using UnityEngine.Video; // ¡Esta línea es nueva y obligatoria para usar vídeos!

public class DetectorTele : MonoBehaviour
{
    [Header("Conexiones")]
    public SalonGameManager gameManager;

    [Tooltip("Arrastra aquí el objeto Screen que tiene el Video Player")]
    public VideoPlayer reproductorVideo;

    private bool yaEncendida = false;

    void Update()
    {
        // Si la tele no estaba encendida, pero de repente el VÍDEO empieza a reproducirse...
        if (!yaEncendida && reproductorVideo != null && reproductorVideo.isPlaying)
        {
            yaEncendida = true; // Marcamos que ya está encendida

            if (gameManager != null)
            {
                gameManager.MisionTeleCompletada();
            }
        }
    }
}