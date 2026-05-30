using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SalonGameManager : MonoBehaviour
{
    public enum FaseJuego { Reciclaje, Planta, Vela, Tele, Sombrero, Diana, Basket, Lego, Terminado }
    
    [Header("Control de Flujo (Máquina de Estados)")]
    public FaseJuego faseActual = FaseJuego.Reciclaje;

    [Header("Interfaz y Métricas")]
    public TMP_Text scoreText;
    public int currentScore = 0;
    public int totalItemsToRecycle = 6;

    [Header("Subsistema de Audio")]
    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip wrongSound;

    [Header("Elementos de Transición")]
    public GameObject regadera; 
    public DoorOpener puertaAlBaño; 

    [Header("Fase: Diana")]
    private int puntosDianaActuales = 0;
    public int puntosNecesariosDiana = 100;

    [Header("Fase: Baloncesto")]
    private int canastasActuales = 0;
    public int canastasObjetivo = 5;
    public bool esperandoDeteccionCesta = false; 

    [Header("Fase: Puzzle Lego")]
    private int piezasLegoEncajadas = 0;
    public int piezasLegoObjetivo = 5;

    void Start()
    {
        faseActual = FaseJuego.Reciclaje; 
        UpdateUI();

        if (regadera != null)
        {
            regadera.SetActive(false);
        }
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Aunque antes de jugar con todo esto, tenemos una pequeña misión: ¡Toca reciclar! Tira las cosas orgánicas (frutas) al cubo VERDE y los plásticos (botellas) al AMARILLO. \n\n Reciclaje: " + currentScore + "/" + totalItemsToRecycle;
    }

    // --- FASE 1: RECICLAJE ---
    public void OnFruitDropped(SelectEnterEventArgs args)
    {
        if (faseActual != FaseJuego.Reciclaje) return; 

        GameObject objeto = args.interactableObject.transform.gameObject;
        if (objeto.CompareTag("Frutas")) ProcesarAcierto(objeto);
        else ProcesarError(objeto);
    }

    public void OnBottleDropped(SelectEnterEventArgs args)
    {
        if (faseActual != FaseJuego.Reciclaje) return; 

        GameObject objeto = args.interactableObject.transform.gameObject;
        if (objeto.CompareTag("Botellas")) ProcesarAcierto(objeto);
        else ProcesarError(objeto);
    }

    private void ProcesarAcierto(GameObject obj)
    {
        if (audioSource && correctSound) audioSource.PlayOneShot(correctSound);

        obj.SetActive(false);
        currentScore++;
        UpdateUI();

        if (currentScore >= totalItemsToRecycle)
        {
            FinalizarReciclaje();
        }
    }

    private void ProcesarError(GameObject obj)
    {
        if (audioSource && wrongSound) audioSource.PlayOneShot(wrongSound);

        obj.SetActive(false);
        obj.SetActive(true);

        ItemReturner returner = obj.GetComponent<ItemReturner>();
        if (returner != null) 
        {
            returner.ReturnToStart();
        }
        else 
        {
            Debug.LogWarning($"Validación: Al objeto '{obj.name}' no se le ha asociado el componente ItemReturner.");
        }
    }

    private void FinalizarReciclaje()
    {
        faseActual = FaseJuego.Planta; 

        if (scoreText != null) scoreText.text = "¡Misión superada! Eres un genio del reciclaje.\n\nAhora mira a tu izquierda y coge la regadera. Para darle de beber a la maceta, acércate y gira tu mano hacia abajo... ¡como si estuvieras regando de verdad!";
        if (regadera != null) regadera.SetActive(true);
    }

    // --- FASE 2: PLANTA ---
    public void MisionPlantaCompletada()
    {
        if (faseActual != FaseJuego.Planta) return; 
        faseActual = FaseJuego.Vela; 

        if (audioSource && correctSound) audioSource.PlayOneShot(correctSound);
        if (scoreText != null) scoreText.text = "¡Genial! Siguiente misión: encender la vela de la mesa.\n\nMira a la derecha, al borde de la encimera de la cocina, y coge el encendedor azul. Para usarlo, aprieta el gatillo de arriba de tu mando y saldrá la llama. Acércalo a la vela y... ¡magia! Para apagar el encendedor cuando termines, solo tienes que agitar un poco la mano en el aire.";
    }

    // --- FASE 3: VELA ---
    public void MisionVelaCompletada()
    {
        if (faseActual != FaseJuego.Vela) return; 
        faseActual = FaseJuego.Tele; 

        if (audioSource && correctSound) audioSource.PlayOneShot(correctSound);
        if (scoreText != null) scoreText.text = "¡Qué luz más bonita! Siguiente misión: ¡Hora de ver la tele!\n\nMira a la mesita de madera que tienes justo delante y coge el mando de la tele. Para encenderla, aprieta el gatillo de arriba de tu mando. Si quieres apagarla, ¡solo tienes que volver a darle al gatillo!";
    }

    // --- FASE 4: TELE ---
    public void MisionTeleCompletada()
    {
        if (faseActual != FaseJuego.Tele) return; 
        faseActual = FaseJuego.Sombrero; 

        if (audioSource && correctSound) audioSource.PlayOneShot(correctSound);
        if (scoreText != null) scoreText.text = "¡Qué bien! Ya casi estamos.\n\nMira a la repisa de la chimenea de piedra y elige el sombrero que más te guste. Cógelo con tu mano, súbelo hasta tu cabeza de verdad y suéltalo para ponértelo. ¡A ver qué tal te queda!";
    }

    // --- FASE 5: SOMBRERO ---
    public void OnHatEquipped(SelectEnterEventArgs args)
    {
        GameObject objeto = args.interactableObject.transform.gameObject;

        if (faseActual != FaseJuego.Sombrero)
        {
            RechazarObjetoCabeza(objeto);
            return;
        }

        if (objeto.CompareTag("Sombrero"))
        {
            faseActual = FaseJuego.Diana; 

            if (audioSource && correctSound) audioSource.PlayOneShot(correctSound);
            if (scoreText != null) scoreText.text = "¡Te queda genial ese sombrero! Como gran final, vamos a jugar a los dardos.\n\nBusca la pistola azul que está en el sofá. Agárrala y usa el gatillo de arriba para disparar a la diana. ¡Intenta conseguir la puntuación máxima! El rojo da 10 puntos, el amarillo da 20 y el centro verde da 50 puntos. ¡Apunta bien!";

            puntosDianaActuales = 0;
        }
        else
        {
            RechazarObjetoCabeza(objeto);
        }
    }

    private void RechazarObjetoCabeza(GameObject objeto)
    {
        if (audioSource && wrongSound) audioSource.PlayOneShot(wrongSound);
        objeto.SetActive(false);
        objeto.SetActive(true);
        ItemReturner returner = objeto.GetComponent<ItemReturner>();
        if (returner != null) returner.ReturnToStart();
    }

    // --- FASE 6: DIANA ---
    public void RegistrarImpactoDiana(int puntosASumar)
    {
        if (faseActual != FaseJuego.Diana) return; 

        puntosDianaActuales += puntosASumar;

        if (scoreText != null) scoreText.text = $"¡Buen tiro! Puntos Diana: {puntosDianaActuales} / {puntosNecesariosDiana}";

        if (puntosDianaActuales >= puntosNecesariosDiana)
        {
            MisionDianaCompletada();
        }
    }

    private void MisionDianaCompletada()
    {
        faseActual = FaseJuego.Basket; 

        if (audioSource && correctSound) audioSource.PlayOneShot(correctSound);
        if (scoreText != null) scoreText.text = "¡Qué puntería tienes! Siguiente misión: ¡a encestar!\n\nBusca la pelota de baloncesto que está en el suelo, justo al lado de la canasta. Cógela con tu mano y lánzala por el aro. Tienes que encestar hasta conseguir la puntuación máxima. ¡A ver qué tal se te da el baloncesto!";
    }

    // --- FASE 7: BALONCESTO ---
    public void BalonPorEncima()
    {
        if (faseActual != FaseJuego.Basket) return; 
        esperandoDeteccionCesta = true;
        Invoke("ResetearChivatoBalon", 2f);
    }

    void ResetearChivatoBalon() { esperandoDeteccionCesta = false; }

    public void RegistrarCanasta()
    {
        if (faseActual == FaseJuego.Basket && esperandoDeteccionCesta)
        {
            esperandoDeteccionCesta = false;
            canastasActuales++;

            if (audioSource && correctSound) audioSource.PlayOneShot(correctSound);
            if (scoreText != null) scoreText.text = $"¡CANASTA! {canastasActuales} / {canastasObjetivo}";

            if (canastasActuales >= canastasObjetivo)
            {
                MisionBasketCompletada();
            }
        }
    }

    private void MisionBasketCompletada()
    {
        faseActual = FaseJuego.Lego; 

        if (audioSource && correctSound) audioSource.PlayOneShot(correctSound);
        if (scoreText != null) scoreText.text = "¡Encestas como un profesional! Ahora toca un poco de concentración.\n\nBusca las piezas de construcción sueltas por la mesa. Al coger una pieza, fíjate bien en la plataforma roja: verás una pista brillante indicando su lugar perfecto. ¡Pon cada pieza en su marca para completar el puzzle!";

        piezasLegoEncajadas = 0;
    }

    // --- FASE 8: LEGO ---
    public void RegistrarPiezaLego(SelectEnterEventArgs args)
    {
        if (faseActual != FaseJuego.Lego) return; 

        piezasLegoEncajadas++;

        if (audioSource && correctSound) audioSource.PlayOneShot(correctSound);
        if (scoreText != null) scoreText.text = $"Piezas colocadas: {piezasLegoEncajadas} / {piezasLegoObjetivo}";

        GameObject pieza = args.interactableObject.transform.gameObject;

        // Deshabilitamos las dinámicas físicas del Rigidbody para consolidar la pieza sobre la malla base
        Rigidbody rb = pieza.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // Desactivamos el colisionador para consolidar el objeto en su posición final y restringir nuevas interacciones
        Collider col = pieza.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        if (piezasLegoEncajadas >= piezasLegoObjetivo)
        {
            MisionLegoCompletada();
        }
    }

    private void MisionLegoCompletada()
    {
        faseActual = FaseJuego.Terminado; 

        if (audioSource && correctSound) audioSource.PlayOneShot(correctSound);
        if (scoreText != null) scoreText.text = "¡PERFECTO! Como recompensa... ¡La puerta del baño se acaba de abrir!\n\nComo nuestro gran premio final va a ser preparar un riquísimo sándwich, ¡primero hay que lavarse las manos! Entra, coge la pastilla de jabón y frótala bien entre tus manos. Después, ponlas bajo el agua del lavabo un ratito hasta que queden relucientes. Cuando termines, vuelve al salón";

        if (puertaAlBaño != null)
        {
            puertaAlBaño.OpenDoor();
        }
    }

    public void MinijuegoLavaboSuperado()
    {
        Debug.Log("Telemetría: Evento de lavado (Baño) notificado al GameManager.");

        if (scoreText != null) 
        {
            scoreText.text = "¡Manos relucientes! Ahora sí, ¡a la cocina!\n\nVamos a preparar un súper sándwich. Fíjate bien en el plato blanco: aparecerá un fantasmita enseñándote qué ingrediente va en cada paso. Primero pon el pan, y ve apilando lo que te pida. ¡Busca todo lo que necesitas en la mesa de al lado y monta el sándwich perfecto!";
        }
    }

    public void MinijuegoSandwichSuperado()
    {
        Debug.Log("Telemetría: Flujo de secuenciación completado globalmente.");

        if (scoreText != null)
        {
            scoreText.text = "¡Qué pintaza tiene ese sándwich! \n\n¡ENHORABUENA, HAS SUPERADO EL JUEGO! Has completado absolutamente todos los retos de la habitación. Eres un jugador de primera. A partir de ahora, eres totalmente libre: ¡juega con la pelota, dispara la pistola, monta más Legos o simplemente relájate! ¡Disfruta de tu sala virtual!";
        }
    }
}
