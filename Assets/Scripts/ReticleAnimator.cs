using UnityEngine;

public class ReticleAnimator : MonoBehaviour
{
    [Header("Cinemática de Rotación")]
    [Tooltip("Velocidad angular de rotación en el eje Y (grados por segundo).")]
    public float rotationSpeed = 90f; 

    [Header("Cinemática de Escala (Feedback Visual)")]
    [Tooltip("Frecuencia de la interpolación sinusoidal de la escala.")]
    public float scaleSpeed = 3f;

    [Tooltip("Amplitud máxima de la deformación volumétrica.")]
    public float scaleAmount = 0.2f;

    private Vector3 originalScale;

    void Start()
    {
        // Almacenamos la escala base para garantizar que la transformación posterior sea relativa y no destructiva
        originalScale = transform.localScale;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        // Aplicación de una función trigonométrica para generar un efecto de pulso continuo (retroalimentación pasiva)
        float scaleModifier = Mathf.Sin(Time.time * scaleSpeed) * scaleAmount;

        transform.localScale = originalScale + new Vector3(scaleModifier, scaleModifier, scaleModifier);
    }
}
