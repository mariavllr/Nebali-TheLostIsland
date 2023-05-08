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
    private DialogueContainer miProximoDialogo;
    private bool interaccionAcabada;
    void Start()
    {
        gameManager = gameManagerObject.GetComponent<GameManager>();
        managerDialogos = gameManagerObject.GetComponent<ManagerDialogos>();
        navmesh = GetComponent<NavMeshAgent>();
        ActivarDialogo.onDialogueEvent += EventoDialogoEdbri;
        dialogoInfo = GetComponent<ActivarDialogo>();
        dialogoInfo.dialogoDisponible = false;
        interaccionAcabada = false;
    }
    void Update()
    {
       /* if (JugadorCerca() && interaccionAcabada)
        {
            managerDialogos.CambiarDialogo(miProximoDialogo);
        }*/
    }

    void EventoDialogoEdbri()
    {
        //Se ha terminado el dialogo con Brivia que da entrada a este dialogo
        if (dialogoInfo.personaje == "Brivia")
        {
            if (dialogoInfo.dialogoActual.ExposedProperties[0].PropertyName == "DialogueName")
            {
                if (dialogoInfo.dialogoActual.ExposedProperties[0].PropertyValue == "Brivia2")
                {
                    Debug.Log("Empieza alcalde");
                    managerDialogos.CambiarDialogo(dialogos[0]);
                    dialogoInfo.dialogoDisponible = true;
                }
            }
        }

        if (dialogoInfo.personaje == "Edbri")
        {

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
