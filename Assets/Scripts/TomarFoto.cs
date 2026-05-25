using UnityEngine;

public class TomarFoto : MonoBehaviour
{
    public Camera camaraLente;      // La cámara que enfoca (el hijo)
    public GameObject fotoPrefab;   // El papel en blanco (tu Prefab azul)
    public Transform puntoSalida;   // Por dónde sale la foto (el Empty frontal)
    public AudioSource sonidoFoto;  // (Opcional) Sonido de disparo

    public void DispararPolaroid()
    {
        // Seguridad: Comprobamos que no falte nada por rellenar en el Inspector
        if (camaraLente == null || fotoPrefab == null || puntoSalida == null) return;

        // 1. "Congelamos" la imagen que está viendo la lente
        RenderTexture rt = camaraLente.targetTexture;
        if (rt == null) return;

        RenderTexture.active = rt;

        // 2. Creamos una textura nueva y le pegamos los píxeles
        Texture2D fotoFinal = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        fotoFinal.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        fotoFinal.Apply();
        RenderTexture.active = null;

        // 3. Imprimimos el papel en el mundo real (Instanciar)
        GameObject nuevaFoto = Instantiate(fotoPrefab, puntoSalida.position, puntoSalida.rotation);

        // 4. Le ponemos la imagen recién horneada a la foto
        MeshRenderer renderer = nuevaFoto.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material.mainTexture = fotoFinal;
        }

        // 5. Escupimos la foto hacia afuera con las físicas
        Rigidbody rb = nuevaFoto.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(puntoSalida.forward * 0.1f, ForceMode.Impulse);
        }

        // 6. Reproducir sonido si le has puesto uno
        if (sonidoFoto != null) sonidoFoto.Play();
    }
}