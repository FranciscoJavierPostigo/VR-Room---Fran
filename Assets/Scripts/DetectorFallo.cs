using UnityEngine;

public class DetectorFallo : MonoBehaviour
{
    [Header("Retroalimentación Visual")]
    public ParticleSystem particulasFallo;

    [Header("Configuración de Control")]
    [Tooltip("Identificador Tag que activará la señal de error")]
    public string tagEquivocado;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(tagEquivocado))
        {
            if (particulasFallo != null)
            {
                particulasFallo.Play();
            }
        }
    }
}
