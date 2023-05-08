using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Subtegral.DialogueSystem.DataContainers;
using Subtegral.DialogueSystem.Runtime;

public class ManagerDialogos : MonoBehaviour
{
    public List<DialogueContainer> dialogos;
    public DialogueParser dialogueParser;
    void Start()
    {

    }

    void Update()
    {
        
    }

    public void CambiarDialogo(DialogueContainer dialogo)
    {
        dialogueParser.dialogue = dialogo;
        dialogueParser.RebootDialogue();
    }
}
