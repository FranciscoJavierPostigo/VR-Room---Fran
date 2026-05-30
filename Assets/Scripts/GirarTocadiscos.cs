using UnityEngine;

public class GirarTocadiscos : MonoBehaviour
{
    [Header("Cinemática del Tocadiscos")]
    [Tooltip("Velocidad angular de rotación en el eje Y (grados por segundo)")]
    public float velocidadGiro = 45f;

    void Update()
    {
        transform.Rotate(Vector3.up * velocidadGiro * Time.deltaTime);
    }
}
