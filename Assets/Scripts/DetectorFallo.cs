using UnityEngine;

public class DetectorFallo : MonoBehaviour
{
    [Header("Efecto de Error")]
    public ParticleSystem particulasFallo;

    [Header("¿Qué etiqueta (Tag) NO debería entrar aquí?")]
    public string tagEquivocado;

    private void OnTriggerEnter(Collider other)
    {
        // Si el objeto que entra tiene la etiqueta equivocada...
        if (other.gameObject.CompareTag(tagEquivocado))
        {
            // Reproducimos las partículas amigables de error
            particulasFallo.Play();
        }
    }
}