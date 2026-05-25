using UnityEngine;

public class HoopSensor : MonoBehaviour
{
    public bool esElDeArriba; // Si es true, es el PreCheck. Si es false, es el de la Cesta.
    private SalonGameManager manager;

    void Start() { manager = Object.FindFirstObjectByType<SalonGameManager>(); }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Balon")) // ¡Asegúrate de que la pelota tiene el tag "Balon"!
        {
            if (esElDeArriba) manager.BalonPorEncima();
            else manager.RegistrarCanasta();
        }
    }
}