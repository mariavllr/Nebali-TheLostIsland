using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Subtegral.DialogueSystem.DataContainers;
using Subtegral.DialogueSystem.Runtime;
using TMPro;

public class ManagerDialogos : MonoBehaviour
{
    public GameManager gameManager;
    public DialogueParser dialogueParser;
    public GameObject mateo;

    [Header("Dialogo escena")]
    public GameObject dialogo;
    [SerializeField] TMP_Text textoDialogo;
    [SerializeField] TMP_Text nombreDialogo;

    [Header("No rellenar")]
    public string personaje;
    public DialogueContainer dialogoActual;

    public delegate void OnDialogueEvent();
    public static event OnDialogueEvent onDialogueEvent;

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
        dialogo.SetActive(true);
        //Congelar al personaje
        mateo.GetComponent<PlayerMovement>().canMove = false;

        //Ver qué dialogo es, de qué personaje y según eso actualizar la info
        personaje = dialogueParser.dialogue.DialogueNodeData[0].characterName;
        dialogoActual = dialogueParser.dialogue;
    }

    public void CerrarDialogo()
    {
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
