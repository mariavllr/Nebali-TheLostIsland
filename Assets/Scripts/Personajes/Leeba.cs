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
    private Animator animator;

    public List<DialogueContainer> dialogos;
    public DialogueContainer miProximoDialogo;

    public GameObject ca�a;
    public bool misionGatoCompletada = false;


    void Start()
    {
        ManagerDialogos.onDialogueEvent += EventoDialogoLeeba;
        Gato.misionGatoCompletedEvent += MisionGatoCompletada;

        gameManager = gameManagerObject.GetComponent<GameManager>();
        managerDialogos = gameManagerObject.GetComponent<ManagerDialogos>();
        animator = GetComponent<Animator>();

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
                        //Misi�n buscar a todos los aldeanos
                        dialogoInfo.dialogoDisponible = true;
                        break;
                    case "Edbri3":
                        //Mision gato leeba
                        animator.SetBool("DoSitups", true);
                        dialogoInfo.dialogoDisponible = false;
                        ca�a.SetActive(false);
                        break;
                }
            }
        }

        if(managerDialogos.personaje == "Leeba")
        {
            animator.SetBool("IsFishing", true);

            if (managerDialogos.dialogoActual.ExposedProperties[0].PropertyName == "DialogueName")
            {
                switch (managerDialogos.dialogoActual.ExposedProperties[0].PropertyValue)
                {
                    case "Leeba1":
                        dialogoInfo.dialogoDisponible = false;
                        break;
                    case "Leeba2":
                        //Mision gato completada
                        dialogoInfo.dialogoDisponible = false;
                        gameManager.misionActual = GameManager.Mision.Ninguna;
                        gameObject.GetComponent<Animator>().SetBool("DoSitups", false);
                        gameObject.GetComponent<Animator>().SetBool("FindCat", false);
                        gameObject.GetComponent<Animator>().SetBool("IsFishing", true);
                        ca�a.SetActive(true);
                        break;
                }
            }
        }
    }

    void MisionGatoCompletada()
    {
        gameManager.MostrarMensaje("Has encontrado al gato! Llevaselo a Leeba");
        dialogoInfo.dialogoDisponible = true;
        miProximoDialogo = dialogos[1]; //Dialogo de mision completada

        misionGatoCompletada = true;
    }
}
