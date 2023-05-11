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
        //Se ha terminado dialogo con Brivia
        if (managerDialogos.personaje == "Brivia")
        {
            if (managerDialogos.dialogoActual.ExposedProperties[0].PropertyName == "DialogueName")
            {
                switch (managerDialogos.dialogoActual.ExposedProperties[0].PropertyValue)
                {
                    case "Brivia2":
                        miProximoDialogo = dialogos[0]; //Edbri1
                        dialogoInfo.dialogoDisponible = true;
                        break;
                }
            }
        }

        //Se ha acabado dialogo con Edbri
        if (managerDialogos.personaje == "Edbri")
        {
            if (managerDialogos.dialogoActual.ExposedProperties[0].PropertyName == "DialogueName")
            {
                switch (managerDialogos.dialogoActual.ExposedProperties[0].PropertyValue)
                {
                    case "Edbri1":
                        //Misi�n buscar a todos los aldeanos
                        gameManager.misionActual = GameManager.Mision.ConocerAldeanos;
                        Debug.Log("Mision aldeanos comienza");

                        miProximoDialogo = dialogos[1]; //Edbri2, "sigue buscando"
                        break;
                }
            }
        }
    }

}