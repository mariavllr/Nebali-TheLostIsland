using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Subtegral.DialogueSystem.Runtime;
using Subtegral.DialogueSystem.DataContainers;

public class ActivarDialogo : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject talkIcon;
    public bool dialogoDisponible;
    [SerializeField] GameObject player;
    [SerializeField] float radio;
    [SerializeField] GameObject dialogo;
    [SerializeField] TMP_Text textoDialogo;
    [SerializeField] TMP_Text nombreDialogo;

    [SerializeField] DialogueParser dialogueParser;
    public string personaje;
    public DialogueContainer dialogoActual;

    private Animator animator;

    public delegate void OnDialogueEvent();
    public static event OnDialogueEvent onDialogueEvent;
    void Start()
    {
        animator = talkIcon.GetComponent<Animator>();
    }

    void Update()
    {
        //Si el icono de hablar está activo, no hay un diálogo ya abierto y se pulsa espacio, se abre el diálogo
        if (ActivarIconoConversacion())
        {
            if (Input.GetKeyDown(gameManager.hablar) && !dialogo.activeInHierarchy)
            {
                MostrarDialogo();
            }           
        }

        //Si hay un diálogo activo y se acaba, cerrar
        if (dialogo.activeInHierarchy && textoDialogo.text == "CerrarDialogo" && personaje == gameObject.name)
        {           
            CerrarDialogo();
            if (onDialogueEvent != null) onDialogueEvent();
        }

        if (dialogo.activeInHierarchy && textoDialogo.text == "ReiniciarDialogo" && personaje == gameObject.name)
        {
            CerrarDialogo();
            ReiniciarDialogo();
        }

    }

    private bool ActivarIconoConversacion()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= radio && dialogoDisponible)
        {
            animator.SetBool("StartTalk", true);
            return true;

        }
        else
        {
            animator.SetBool("StartTalk", false);
            return false;
        }
    }

    private void MostrarDialogo()
    {
        dialogo.SetActive(true);
        //Congelar al personaje
        player.GetComponent<PlayerMovement>().canMove = false;
        //Ver qué dialogo es, de qué personaje y según eso actualizar la info
        personaje = dialogueParser.dialogue.DialogueNodeData[0].characterName;
        dialogoActual = dialogueParser.dialogue;
    }

    private void CerrarDialogo()
    {
        dialogo.SetActive(false);
        player.GetComponent<PlayerMovement>().canMove = true;
    }

    private void ReiniciarDialogo()
    {
        gameManager.gameObject.GetComponent<DialogueParser>().RebootDialogue();
    }
}
