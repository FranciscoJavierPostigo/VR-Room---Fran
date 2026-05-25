using UnityEngine;

public class ReticleAnimator : MonoBehaviour
{
    [Tooltip("Velocidad a la que gira el retículo")]
    public float rotationSpeed = 90f; // Gira 90 grados cada segundo

    [Tooltip("Velocidad a la que crece y encoge")]
    public float scaleSpeed = 3f;

    [Tooltip("Cuánto crece y encoge")]
    public float scaleAmount = 0.2f;

    // Variable interna para recordar el tamaño original
    private Vector3 originalScale;

    void Start()
    {
        // Nada más empezar, guardamos el tamaño original del retículo
        // para no deformarlo al hacerlo latir.
        originalScale = transform.localScale;
    }

    void Update()
    {
        // 1. HACER QUE GIRE
        // Vector3.up es el eje Y (hacia arriba). 
        // Time.deltaTime hace que gire suavemente sin importar los FPS de tu ordenador.
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        // 2. HACER QUE PALPITE (ESCALA)
        // Mathf.Sin crea una onda matemática que sube y baja constantemente con el tiempo.
        // Esto nos da un modificador que hace el efecto de "latido".
        float scaleModifier = Mathf.Sin(Time.time * scaleSpeed) * scaleAmount;

        // Le sumamos ese latido al tamaño original en los ejes X, Y y Z.
        transform.localScale = originalScale + new Vector3(scaleModifier, scaleModifier, scaleModifier);
    }
}