using Subtegral.DialogueSystem.DataContainers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Edbri : MonoBehaviour
{
    [SerializeField] GameObject gameManagerObject;

    private GameManager gameManager;
    private ManagerDialogos managerDialogos;
    private ActivarDialogo dialogoInfo;
    private NavMeshAgent navmesh;

    public List<DialogueContainer> dialogos;
    public DialogueContainer miProximoDialogo;

    void Start()
    {
        ManagerDialogos.onDialogueEvent += EventoDialogoEdbri;

        gameManager = gameManagerObject.GetComponent<GameManager>();
        managerDialogos = gameManagerObject.GetComponent<ManagerDialogos>();
        navmesh = GetComponent<NavMeshAgent>();

        dialogoInfo = GetComponent<ActivarDialogo>();
        dialogoInfo.dialogoDisponible = false;
        miProximoDialogo = dialogos[0];
    }
    void Update()
    {
        
    }

    void EventoDialogoEdbri()
    {
        //Se ha terminado el dialogo con Brivia que da entrada a este dialogo
        if (managerDialogos.personaje == "Brivia")
        {
            if (managerDialogos.dialogoActual.ExposedProperties[0].PropertyName == "DialogueName")
            {
                if (managerDialogos.dialogoActual.ExposedProperties[0].PropertyValue == "Brivia2")
                {
                    miProximoDialogo = dialogos[0];
                    dialogoInfo.dialogoDisponible = true;
                }
            }
        }

        if (managerDialogos.personaje == "Edbri")
        {
            if (managerDialogos.dialogoActual.ExposedProperties[0].PropertyName == "DialogueName")
            {
                if (managerDialogos.dialogoActual.ExposedProperties[0].PropertyValue == "Edbri1")
                {
                    dialogoInfo.dialogoDisponible = false;
                    Debug.Log("no mas alcalde");
                }
            }
        }
    }

    bool JugadorCerca()
    {
        if (Vector3.Distance(transform.position, gameManager.player.transform.position) <= 2)
        {
            return true;
        }
        else return false;
    }

}
