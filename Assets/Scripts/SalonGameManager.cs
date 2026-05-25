using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SalonGameManager : MonoBehaviour
{
    // --- NUEVO SEMÁFORO DE FASES ---
    public enum FaseJuego { Reciclaje, Planta, Vela, Tele, Sombrero, Diana, Basket, Lego, Terminado }
    [Header("Estado Actual (No tocar)")]
    public FaseJuego faseActual = FaseJuego.Reciclaje;

    [Header("UI y Puntuación")]
    public TMP_Text scoreText;
    public int currentScore = 0;
    public int totalItemsToRecycle = 6;

    [Header("Sonidos")]
    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip wrongSound;

    [Header("Siguiente Paso")]
    public GameObject regadera; 
    public DoorOpener puertaAlBaño; 

    [Header("FASE DIANA")]
    private int puntosDianaActuales = 0;
    public int puntosNecesariosDiana = 100;

    [Header("FASE BALONCESTO")]
    private int canastasActuales = 0;
    public int canastasObjetivo = 5;
    public bool esperandoDeteccionCesta = false; 

    [Header("FASE LEGO")]
    private int piezasLegoEncajadas = 0;
    public int piezasLegoObjetivo = 5;




    void Start()
    {
        faseActual = FaseJuego.Reciclaje; // Nos aseguramos de empezar en el paso 1
        UpdateUI();

        // Ocultamos la regadera al empezar el juego
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
        if (faseActual != FaseJuego.Reciclaje) return; // Si no es la fase de reciclaje, ignora

        GameObject objeto = args.interactableObject.transform.gameObject;
        if (objeto.CompareTag("Frutas")) ProcesarAcierto(objeto);
        else ProcesarError(objeto);
    }

    public void OnBottleDropped(SelectEnterEventArgs args)
    {
        if (faseActual != FaseJuego.Reciclaje) return; // Si no es la fase de reciclaje, ignora

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
        if (returner != null) returner.ReturnToStart();
        else Debug.LogWarning("¡OJO! Al objeto " + obj.name + " le falta el script ItemReturner.");
    }

    private void FinalizarReciclaje()
    {
        faseActual = FaseJuego.Planta; // PASAMOS A LA FASE PLANTA

        if (scoreText != null) scoreText.text = "¡Misión superada! Eres un genio del reciclaje.\n\nAhora mira a tu izquierda y coge la regadera. Para darle de beber a la maceta, acércate y gira tu mano hacia abajo... ¡como si estuvieras regando de verdad!";
        if (regadera != null) regadera.SetActive(true);
    }

    // --- FASE 2: PLANTA ---
    public void MisionPlantaCompletada()
    {
        if (faseActual != FaseJuego.Planta) return; // SEMÁFORO
        faseActual = FaseJuego.Vela; // PASAMOS A LA VELA

        if (audioSource && correctSound) audioSource.PlayOneShot(correctSound);
        if (scoreText != null) scoreText.text = "¡Genial! Siguiente misión: encender la vela de la mesa.\n\nMira a la derecha, al borde de la encimera de la cocina, y coge el encendedor azul. Para usarlo, aprieta el gatillo de arriba de tu mando y saldrá la llama. Acércalo a la vela y... ¡magia! Para apagar el encendedor cuando termines, solo tienes que agitar un poco la mano en el aire.";
    }

    // --- FASE 3: VELA ---
    public void MisionVelaCompletada()
    {
        if (faseActual != FaseJuego.Vela) return; // SEMÁFORO
        faseActual = FaseJuego.Tele; // PASAMOS A LA TELE

        if (audioSource && correctSound) audioSource.PlayOneShot(correctSound);
        if (scoreText != null) scoreText.text = "¡Qué luz más bonita! Siguiente misión: ¡Hora de ver la tele!\n\nMira a la mesita de madera que tienes justo delante y coge el mando de la tele. Para encenderla, aprieta el gatillo de arriba de tu mando. Si quieres apagarla, ¡solo tienes que volver a darle al gatillo!";
    }

    // --- FASE 4: TELE ---
    public void MisionTeleCompletada()
    {
        if (faseActual != FaseJuego.Tele) return; // SEMÁFORO
        faseActual = FaseJuego.Sombrero; // PASAMOS AL SOMBRERO

        if (audioSource && correctSound) audioSource.PlayOneShot(correctSound);
        if (scoreText != null) scoreText.text = "¡Qué bien! Ya casi estamos.\n\nMira a la repisa de la chimenea de piedra y elige el sombrero que más te guste. Cógelo con tu mano, súbelo hasta tu cabeza de verdad y suéltalo para ponértelo. ¡A ver qué tal te queda!";
    }

    // --- FASE 5: SOMBRERO ---
    public void OnHatEquipped(SelectEnterEventArgs args)
    {
        GameObject objeto = args.interactableObject.transform.gameObject;

        if (faseActual != FaseJuego.Sombrero)
        {
            // Si intenta ponerse el sombrero antes de tiempo, se lo quitamos y le decimos error
            RechazarObjetoCabeza(objeto);
            return;
        }

        if (objeto.CompareTag("Sombrero"))
        {
            faseActual = FaseJuego.Diana; // PASAMOS A LA DIANA

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
        if (faseActual != FaseJuego.Diana) return; // SEMÁFORO

        puntosDianaActuales += puntosASumar;

        if (scoreText != null) scoreText.text = $"¡Buen tiro! Puntos Diana: {puntosDianaActuales} / {puntosNecesariosDiana}";

        if (puntosDianaActuales >= puntosNecesariosDiana)
        {
            MisionDianaCompletada();
        }
    }

    private void MisionDianaCompletada()
    {
        faseActual = FaseJuego.Basket; // PASAMOS AL BASKET

        if (audioSource && correctSound) audioSource.PlayOneShot(correctSound);
        if (scoreText != null) scoreText.text = "¡Qué puntería tienes! Siguiente misión: ¡a encestar!\n\nBusca la pelota de baloncesto que está en el suelo, justo al lado de la canasta. Cógela con tu mano y lánzala por el aro. Tienes que encestar hasta conseguir la puntuación máxima. ¡A ver qué tal se te da el baloncesto!";
    }

    // --- FASE 7: BALONCESTO ---
    public void BalonPorEncima()
    {
        if (faseActual != FaseJuego.Basket) return; // SEMÁFORO
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
        faseActual = FaseJuego.Lego; // PASAMOS AL LEGO

        if (audioSource && correctSound) audioSource.PlayOneShot(correctSound);
        if (scoreText != null) scoreText.text = "¡Encestas como un profesional! Ahora toca un poco de concentración.\n\nBusca las piezas de construcción sueltas por la mesa. Al coger una pieza, fíjate bien en la plataforma roja: verás una pista brillante indicando su lugar perfecto. ¡Pon cada pieza en su marca para completar el puzzle!";

        piezasLegoEncajadas = 0;
    }

    // --- FASE 8: LEGO ---
    // --- FASE 8: LEGO ---
    public void RegistrarPiezaLego(SelectEnterEventArgs args)
    {
        if (faseActual != FaseJuego.Lego) return; // SEMÁFORO

        piezasLegoEncajadas++;

        if (audioSource && correctSound) audioSource.PlayOneShot(correctSound);
        if (scoreText != null) scoreText.text = $"Piezas colocadas: {piezasLegoEncajadas} / {piezasLegoObjetivo}";

        // --- LA MAGIA PARA QUE SE QUEDE QUIETO Y NO SE PUEDA VOLVER A COGER ---
        GameObject pieza = args.interactableObject.transform.gameObject;

        // 1. Apagamos la gravedad y las físicas. Así no tiembla ni se pelea con la placa roja.
        Rigidbody rb = pieza.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // 2. Apagamos el colisionador. Sin colisionador, las manos del jugador no pueden 
        // volver a detectarlo, así que es imposible que lo vuelvan a coger.
        Collider col = pieza.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        // ¡OJO! Hemos borrado la línea de "piezaAgarrable.enabled = false;" porque eso rompía el socket.

        if (piezasLegoEncajadas >= piezasLegoObjetivo)
        {
            MisionLegoCompletada();
        }
    }

    private void MisionLegoCompletada()
    {
        faseActual = FaseJuego.Terminado; // JUEGO COMPLETADO

        if (audioSource && correctSound) audioSource.PlayOneShot(correctSound);
        if (scoreText != null) scoreText.text = "¡PERFECTO! Como recompensa... ¡La puerta del baño se acaba de abrir!\n\nComo nuestro gran premio final va a ser preparar un riquísimo sándwich, ¡primero hay que lavarse las manos! Entra, coge la pastilla de jabón y frótala bien entre tus manos. Después, ponlas bajo el agua del lavabo un ratito hasta que queden relucientes. Cuando termines, vuelve al salón";

        if (puertaAlBaño != null)
        {
            puertaAlBaño.OpenDoor();
        }
    }

    public void MinijuegoLavaboSuperado()
    {
        Debug.Log("GameManager informado: ¡Manos limpias!");

        // 1. Cambiamos el texto de la pantalla gigante
        if (scoreText != null) // Sustituye textoPantalla por el nombre de tu variable de texto
        {
            scoreText.text = "¡Manos relucientes! Ahora sí, ¡a la cocina!\n\nVamos a preparar un súper sándwich. Fíjate bien en el plato blanco: aparecerá un fantasmita enseñándote qué ingrediente va en cada paso. Primero pon el pan, y ve apilando lo que te pida. ¡Busca todo lo que necesitas en la mesa de al lado y monta el sándwich perfecto!";
        }

    }

    public void MinijuegoSandwichSuperado()
    {
        Debug.Log("GameManager informado: ¡Sándwich terminado! Juego completado.");

        if (scoreText != null)
        {
            scoreText.text = "¡Qué pintaza tiene ese sándwich! \n\n¡ENHORABUENA, HAS SUPERADO EL JUEGO! Has completado absolutamente todos los retos de la habitación. Eres un jugador de primera. A partir de ahora, eres totalmente libre: ¡juega con la pelota, dispara la pistola, monta más Legos o simplemente relájate! ¡Disfruta de tu sala virtual!";
        }

        // Aquí puedes lanzar fuegos artificiales, confeti, o un sonido de victoria si los tienes preparados.
    }
}