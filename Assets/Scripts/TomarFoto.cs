using UnityEngine;

public class TomarFoto : MonoBehaviour
{
    [Header("Componentes de Captura")]
    [Tooltip("Cámara secundaria encargada del renderizado a textura (RenderTexture).")]
    public Camera camaraLente;
    
    [Tooltip("Prefabricado instanciable que actúa como soporte físico para la textura generada.")]
    public GameObject fotoPrefab;
    
    [Tooltip("Transform de referencia para el punto de instanciación e impulso físico.")]
    public Transform puntoSalida;

    [Header("Retroalimentación Auditiva")]
    public AudioSource sonidoFoto;

    public void DispararPolaroid()
    {
        // Validación de dependencias críticas antes de iniciar el volcado de memoria
        if (camaraLente == null || fotoPrefab == null || puntoSalida == null) return;

        // Extracción de la textura de renderizado actual del buffer de la cámara
        RenderTexture rt = camaraLente.targetTexture;
        if (rt == null) return;

        RenderTexture.active = rt;

        // Asignación en memoria de una nueva textura 2D y lectura del array de píxeles
        Texture2D fotoFinal = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        fotoFinal.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        fotoFinal.Apply();
        RenderTexture.active = null;

        // Instanciación del soporte físico en las coordenadas del dispensador
        GameObject nuevaFoto = Instantiate(fotoPrefab, puntoSalida.position, puntoSalida.rotation);

        // Inyección dinámica de la textura generada en el material principal del objeto
        MeshRenderer renderer = nuevaFoto.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material.mainTexture = fotoFinal;
        }

        // Aplicación de un impulso físico direccional para simular la expulsión mecánica
        Rigidbody rb = nuevaFoto.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(puntoSalida.forward * 0.1f, ForceMode.Impulse);
        }

        if (sonidoFoto != null) sonidoFoto.Play();
    }
}
