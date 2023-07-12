using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Subtegral.DialogueSystem.DataContainers;
using Subtegral.DialogueSystem.Runtime;
using TMPro;

public class ManagerDialogos : MonoBehaviour
{
    public GameManager gameManager;
    private CinemachineSwitcher cinemachineSwitcher;
    public DialogueParser dialogueParser;
    public GameObject mateo;

    [Header("Dialogo escena")]
    public GameObject dialogo;
    [SerializeField] TMP_Text textoDialogo;
    [SerializeField] TMP_Text nombreDialogo;

    [Header("Mostrar letra a letra")]
    [SerializeField] private float velocidadDialogo = 0.04f;
    [SerializeField] public GameObject choicesContainer;
    private Coroutine displayLineCoroutine;
    public bool puedeContinuarSiguienteLinea = true;
    private string textoDialogoEntero;

    [Header("Efectos de sonido")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> typingSoundClip;
    [Range(1,5)]
    [SerializeField] private int frequencyLevel = 2;
    [Range(-3, 3)]
    [SerializeField] private float minPitch = 1f;
    [Range(-3, 3)]
    [SerializeField] private float maxPitch = 1.5f;

    [Header("No rellenar")]
    public string personaje;
    public DialogueContainer dialogoActual;

    public delegate void OnDialogueEvent(); // se cierra un dialogo
    public static event OnDialogueEvent onDialogueEvent;

    public delegate void OnDialogueOpenedEvent(); //se abre un dialogo
    public static event OnDialogueOpenedEvent onDialogueOpenedEvent;

    private void Start()
    {
        cinemachineSwitcher = GetComponent<CinemachineSwitcher>();

        DialogueParser.onNextLineEvent += DisplayLineFunction;
    }

    private void OnDisable()
    {
        DialogueParser.onNextLineEvent -= DisplayLineFunction;
    }

    private void Update()
    {
        //Si hay un diálogo activo y se acaba, cerrar
        if (dialogo.activeInHierarchy && textoDialogo.text == "CerrarDialogo")
        {
            CerrarDialogo();
        }

        if (dialogo.activeInHierarchy && textoDialogo.text == "ReiniciarDialogo")
        {
            ReiniciarDialogo();
        }

        //Mostrar el texto entero si hace clic
        if(Input.GetMouseButtonDown(0) && !puedeContinuarSiguienteLinea)
        {
            textoDialogo.text = textoDialogoEntero;
            StopCoroutine(displayLineCoroutine);
            puedeContinuarSiguienteLinea = true;
            choicesContainer.SetActive(true);
        }
    }


    public void CambiarDialogo(DialogueContainer dialogo)
    {
        dialogueParser.dialogue = dialogo;
        dialogueParser.RebootDialogue();

        //Ver qué dialogo es, de qué personaje y según eso actualizar la info
        personaje = dialogueParser.dialogue.DialogueNodeData[0].characterName;
        dialogoActual = dialogueParser.dialogue;
    }
    public void MostrarDialogo()
    {
        if(personaje == "Brivia")
        {
            minPitch = 1f;
            maxPitch = 1.5f;
        }
        else if (personaje == "Leeba")
        {
            minPitch = 0.3f;
            maxPitch = 0.6f;
        }
        else if (personaje == "Edbri")
        {
            minPitch = 0.6f;
            maxPitch = 1f;
        }

        else if (personaje == "Nebali")
        {
            minPitch = 1.5f;
            maxPitch = 2f;
        }


        //Congelar al personaje, fijar la camara y desbloquear el cursor
        cinemachineSwitcher.FijarCamara();
        Cursor.lockState = CursorLockMode.None;
        mateo.GetComponent<PlayerMovement>().canMove = false;

        //Activar el dialogo y mostrarlo línea a línea
        dialogo.SetActive(true);
        displayLineCoroutine = StartCoroutine(DisplayLine(textoDialogo.text));

        //Ver qué dialogo es, de qué personaje y según eso actualizar la info
        personaje = dialogueParser.dialogue.DialogueNodeData[0].characterName;
        dialogoActual = dialogueParser.dialogue;

        if (onDialogueOpenedEvent != null) onDialogueOpenedEvent();
    }

    private void DisplayLineFunction()
    {
        if (dialogo.activeInHierarchy && textoDialogo.text != "CerrarDialogo" && textoDialogo.text != "ReiniciarDialogo")
        {
            if(displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            displayLineCoroutine = StartCoroutine(DisplayLine(textoDialogo.text));
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        textoDialogoEntero = textoDialogo.text;
        textoDialogo.text = "";
        choicesContainer.SetActive(false);
        puedeContinuarSiguienteLinea = false;

        foreach (var letter in line.ToCharArray())
        {
            textoDialogo.text += letter;
            PlayDialogueSound(textoDialogo.text.ToCharArray().Length);
            yield return new WaitForSeconds(velocidadDialogo);
        }

        puedeContinuarSiguienteLinea = true;
        choicesContainer.SetActive(true);
    }

    private void PlayDialogueSound(int numCharactersDisplayed)
    {
        if(numCharactersDisplayed % frequencyLevel == 0)
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(typingSoundClip[Random.Range(0, typingSoundClip.Count)]);
        }
    }


    public void CerrarDialogo()
    {
        cinemachineSwitcher.ReiniciarMovimientoCamara();
        Cursor.lockState = CursorLockMode.Locked;
        dialogo.SetActive(false);
        mateo.GetComponent<PlayerMovement>().canMove = true;
        if (onDialogueEvent != null) onDialogueEvent();
    }

    public void ReiniciarDialogo()
    {
        dialogo.SetActive(false);
        mateo.GetComponent<PlayerMovement>().canMove = true;
        gameManager.gameObject.GetComponent<DialogueParser>().RebootDialogue();
    }
}
