using UnityEngine;

public class BotonTocadiscos : MonoBehaviour
{
    private AudioSource miAudio;

    // Creamos un enchufe para conectar el script del plato que gira
    public GirarTocadiscos scriptGiro;

    void Start()
    {
        miAudio = GetComponent<AudioSource>();
    }

    public void AlternarMusica()
    {
        if (miAudio.isPlaying)
        {
            miAudio.Pause();
            // Apagamos el motor de giro
            if (scriptGiro != null) scriptGiro.enabled = false;
        }
        else
        {
            miAudio.Play();
            // Encendemos el motor de giro
            if (scriptGiro != null) scriptGiro.enabled = true;
        }
    }
}