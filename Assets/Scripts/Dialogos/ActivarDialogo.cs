using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Subtegral.DialogueSystem.Runtime;
using Subtegral.DialogueSystem.DataContainers;

public class ActivarDialogo : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    private Animator animator;
    public ManagerDialogos managerDialogos;

    [Header("Icono conversacion")]
    [SerializeField] GameObject talkIcon;
    public GameObject dialogo;
    public bool dialogoDisponible;
    [SerializeField] GameObject player;
    [SerializeField] float radio;


    void Start()
    {
        animator = talkIcon.GetComponent<Animator>();
    }

    void Update()
    {
        //Si el icono de hablar está activo, no hay un diálogo ya abierto y se pulsa espacio, se abre el diálogo
        if (ActivarIconoConversacion())
        {
            if (Input.GetKeyDown(gameManager.hablar) && !dialogo.activeInHierarchy)
            {
                //Que personaje es?
                switch (gameObject.name)
                {
                    case "Brivia":
                        managerDialogos.CambiarDialogo(gameObject.GetComponent<Brivia>().miProximoDialogo);
                        break;
                    case "Edbri":
                        managerDialogos.CambiarDialogo(gameObject.GetComponent<Edbri>().miProximoDialogo);
                        break;
                    case "Leeba":
                        managerDialogos.CambiarDialogo(gameObject.GetComponent<Leeba>().miProximoDialogo);
                        break;
                }
                
                managerDialogos.MostrarDialogo();
            }           
        }
    }

    private bool ActivarIconoConversacion()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= radio && dialogoDisponible)
        {
            animator.SetBool("StartTalk", true);
            return true;

        }
        else
        {
            animator.SetBool("StartTalk", false);
            return false;
        }
    }

   
}
