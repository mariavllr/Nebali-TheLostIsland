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

    public GameObject caña;
    public GameObject objetoCaña_Jugador;
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

    private void OnDisable()
    {
        ManagerDialogos.onDialogueEvent -= EventoDialogoLeeba;
        Gato.misionGatoCompletedEvent -= MisionGatoCompletada;
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
                    case "Edbri3":
                        //Mision gato leeba
                        objetoCaña_Jugador.SetActive(true);
                        caña.SetActive(false);

                        animator.SetBool("DoSitups", true);
                        dialogoInfo.dialogoDisponible = false;
                        
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
                        gameManager.misionActual = GameManager.Mision.Ninguna;
                        gameObject.GetComponent<Animator>().SetBool("DoSitups", false);
                        gameObject.GetComponent<Animator>().SetBool("FindCat", false);
                        gameObject.GetComponent<Animator>().SetBool("IsFishing", true);
                        caña.SetActive(true);
                        gameManager.MostrarMensaje("Nueva entrada en el diario.");
                        gameManager.EntradaDiario("He encontrado el gato de Leeba y se ha puesto muy feliz. Parece que no es tan cascarrabias...");
                        ElegirDialogoRandom();

                        break;
                    default: //Si es Leeba 3, 4 o 5 (dialogo[2] [3] o [4])
                        ElegirDialogoRandom();
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

    private DialogueContainer ElegirDialogoRandom()
    {
        int random = Random.Range(2, 5); //Dialogo 2, 3 o 4

        return dialogos[random];
    }
}
