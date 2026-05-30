using UnityEngine;

public class BotonTocadiscos : MonoBehaviour
{
    [Header("Sincronización de Componentes")]
    [Tooltip("Referencia al script de rotación para vincular la cinemática al estado de reproducción auditiva.")]
    public GirarTocadiscos scriptGiro;

    private AudioSource miAudio;

    void Start()
    {
        miAudio = GetComponent<AudioSource>();
    }

    public void AlternarMusica()
    {
        if (miAudio.isPlaying)
        {
            miAudio.Pause();
            
            // Deshabilitamos la cinemática angular para mantener coherencia visual con la pausa auditiva
            if (scriptGiro != null) scriptGiro.enabled = false;
        }
        else
        {
            miAudio.Play();
            
            // Reactivamos la cinemática en sincronía con el flujo de audio
            if (scriptGiro != null) scriptGiro.enabled = true;
        }
    }
}
