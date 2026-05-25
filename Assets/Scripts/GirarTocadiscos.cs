using UnityEngine;

public class GirarTocadiscos : MonoBehaviour
{
    // Esta variable la podremos cambiar desde Unity para que gire más rápido o más lento
    public float velocidadGiro = 45f;

    // Update se ejecuta continuamente, muchísimas veces por segundo
    void Update()
    {
        // Le decimos al objeto que rote sobre su eje "Arriba" (Y) de forma suave (Time.deltaTime)
        transform.Rotate(Vector3.up * velocidadGiro * Time.deltaTime);
    }
}