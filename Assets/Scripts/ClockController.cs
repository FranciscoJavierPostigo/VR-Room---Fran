using UnityEngine;
using System; // Necesario para usar DateTime

// ¡OJO! Es vital que ponga ": MonoBehaviour" para poder usarlo en Unity
public class ClockController : MonoBehaviour
{
    // Creamos tres "huecos" para arrastrar las agujas del reloj desde Unity
    public Transform hoursHand;
    public Transform minutesHand;
    public Transform secondsHand;

    void Update()
    {
        // 1. Obtenemos la hora actual de tu ordenador
        DateTime time = DateTime.Now;

        // 2. Calculamos los ángulos de rotación (360 grados de la esfera)
        // Cada segundo o minuto son 6 grados (360/60)
        float secondsAngle = time.Second * 6f;
        float minutesAngle = time.Minute * 6f;
        // Cada hora son 30 grados (360/12). Le sumamos un extra para que la aguja se mueva poco a poco entre hora y hora.
        float hoursAngle = (time.Hour % 12) * 30f + (time.Minute * 0.5f);

        // 3. Aplicamos la rotación a las agujas (normalmente giran sobre el eje Z)
        // Usamos rotación negativa (-angle) porque los relojes giran en el sentido de las agujas del reloj
        if (secondsHand != null) secondsHand.localRotation = Quaternion.Euler(secondsAngle, 0, 0);
        if (minutesHand != null) minutesHand.localRotation = Quaternion.Euler(minutesAngle, 0, 0);
        if (hoursHand != null) hoursHand.localRotation = Quaternion.Euler(hoursAngle, 0, 0);
    }
}