using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActivarDialogo : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject talkIcon;
    [SerializeField] GameObject player;
    [SerializeField] float radio;
    [SerializeField] GameObject dialogo;
    [SerializeField] TMP_Text textoDialogo;

    private Animator animator;
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
                MostrarDialogo();
            }           
        }

        //Si hay un diálogo activo y se acaba, cerrar
        if(dialogo.activeInHierarchy && textoDialogo.text == "CerrarDialogo")
        {
            CerrarDialogo();
        }

    }

    private bool ActivarIconoConversacion()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= radio)
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

    private void MostrarDialogo()
    {
        dialogo.SetActive(true);
        //Congelar al personaje
        player.GetComponent<PlayerMovement>().canMove = false;
    }

    private void CerrarDialogo()
    {
        dialogo.SetActive(false);
        player.GetComponent<PlayerMovement>().canMove = true;

        //Reiniciar dialogo

    }
}
