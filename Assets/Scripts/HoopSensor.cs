using UnityEngine;

public class HoopSensor : MonoBehaviour
{
    [Header("Configuración del Sensor")]
    [Tooltip("Determina si este colisionador actúa como validador de entrada superior (PreCheck) o como registro final.")]
    public bool esElDeArriba; 
    
    private SalonGameManager manager;

    void Start() 
    { 
        manager = Object.FindFirstObjectByType<SalonGameManager>(); 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Balon")) 
        {
            // Sistema de validación en dos pasos para evitar falsos positivos (ej. encestar desde abajo)
            if (esElDeArriba) 
            {
                manager.BalonPorEncima();
            }
            else 
            {
                manager.RegistrarCanasta();
            }
        }
    }
}
