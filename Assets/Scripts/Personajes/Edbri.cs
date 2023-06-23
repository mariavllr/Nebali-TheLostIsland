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
        EncontrarAldeanos.misionAldeanosCompletedEvent += MisionAldeanosCompletada;

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

    private void OnDisable()
    {
        ManagerDialogos.onDialogueEvent -= EventoDialogoEdbri;
        EncontrarAldeanos.misionAldeanosCompletedEvent -= MisionAldeanosCompletada;
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
                        //Misión buscar a todos los aldeanos
                        gameManager.misionActual = GameManager.Mision.ConocerAldeanos;
                        gameManager.MostrarMensaje("Nueva entrada en el diario.");
                        gameManager.EntradaDiario("El alcalde del pueblo me ha dicho que tengo que conocer a todos los aldeanos, pero creo que tampoco hay muchos.");

                        miProximoDialogo = dialogos[1]; //Edbri2, "sigue buscando"
                        break;
                    case "Edbri3":
                        //Mision de Leeba
                        gameManager.misionActual = GameManager.Mision.GatoLeeba;
                        gameManager.MostrarMensaje("Nueva entrada en el diario.");
                        gameManager.EntradaDiario("Edbri ahora quiere que le encuentre un gato perdido a Leeba. Que pereza, ¿por qué me mandan a hacer recados?");

                        dialogoInfo.dialogoDisponible = false;
                        break;
                }
            }
        }
    }

    void MisionAldeanosCompletada()
    {
        miProximoDialogo = dialogos[2]; //Edbri3: mision completada.
        gameManager.MostrarMensaje("¡Has conocido a todos! ve a hablar con el alcalde.");
    }

}
