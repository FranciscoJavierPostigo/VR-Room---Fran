using UnityEngine;
using System;

public class ClockController : MonoBehaviour
{
    [Header("Referencias de las Agujas")]
    public Transform hoursHand;
    public Transform minutesHand;
    public Transform secondsHand;

    void Update()
    {
        DateTime time = DateTime.Now;

        float secondsAngle = time.Second * 6f;
        float minutesAngle = time.Minute * 6f;
        
        // Cálculo del ángulo horario con interpolación continua basada en los minutos transcurridos
        float hoursAngle = (time.Hour % 12) * 30f + (time.Minute * 0.5f);

        if (secondsHand != null) secondsHand.localRotation = Quaternion.Euler(secondsAngle, 0, 0);
        if (minutesHand != null) minutesHand.localRotation = Quaternion.Euler(minutesAngle, 0, 0);
        if (hoursHand != null) hoursHand.localRotation = Quaternion.Euler(hoursAngle, 0, 0);
    }
}
