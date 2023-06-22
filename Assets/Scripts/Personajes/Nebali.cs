using Subtegral.DialogueSystem.DataContainers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nebali : MonoBehaviour
{
    private ActivarDialogo dialogoInfo;
    public DialogueContainer dialogoFinal;
    public DialogueContainer miProximoDialogo;
    void Start()
    {
        dialogoInfo = GetComponent<ActivarDialogo>();
        dialogoInfo.dialogoDisponible = false;
        miProximoDialogo = dialogoFinal;

        Gemas.onFinalEvent += EventoDialogoFinal;
    }

    void Update()
    {
        
    }

    void EventoDialogoFinal()
    {
        dialogoInfo.dialogoDisponible = true;
    }
}
