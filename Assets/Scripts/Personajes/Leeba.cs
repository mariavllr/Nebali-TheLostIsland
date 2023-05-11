using Subtegral.DialogueSystem.DataContainers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leeba : MonoBehaviour
{
    [SerializeField] GameObject gameManagerObject;

    private GameManager gameManager;
    private ManagerDialogos managerDialogos;
    private ActivarDialogo dialogoInfo;
    //private NavMeshAgent navmesh;

    public List<DialogueContainer> dialogos;
    public DialogueContainer miProximoDialogo;


    void Start()
    {
        ManagerDialogos.onDialogueEvent += EventoDialogoLeeba;

        gameManager = gameManagerObject.GetComponent<GameManager>();
        managerDialogos = gameManagerObject.GetComponent<ManagerDialogos>();
       // navmesh = GetComponent<NavMeshAgent>();

        dialogoInfo = GetComponent<ActivarDialogo>();
        dialogoInfo.dialogoDisponible = false;
        miProximoDialogo = dialogos[0];
    }
    void Update()
    {
        
    }

    void EventoDialogoLeeba()
    {
        //Se ha acabado dialogo con Edbri
        if (managerDialogos.personaje == "Edbri")
        {
            if (managerDialogos.dialogoActual.ExposedProperties[0].PropertyName == "DialogueName")
            {
                switch (managerDialogos.dialogoActual.ExposedProperties[0].PropertyValue)
                {
                    case "Edbri1":
                        //Misión buscar a todos los aldeanos
                        dialogoInfo.dialogoDisponible = true;
                        break;
                }
            }
        }
    }
}
