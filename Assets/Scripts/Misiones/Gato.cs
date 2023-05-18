using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Gato : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject player;
    private PlayerMovement playerMov;
    public Animator iconAnimator;
    public int radio;

    private NavMeshAgent nav;
    private bool siguiendo = false;

    public delegate void MisionGatoCompletedEvent();
    public static event MisionGatoCompletedEvent misionGatoCompletedEvent;


    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        playerMov = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {

        if(gameManager.misionActual == GameManager.Mision.GatoLeeba)
        {
            if (ActivarIconoInteractuar())
            {
                if (Input.GetKeyDown(gameManager.hablar) && !siguiendo)
                {
                    siguiendo = true;
                    if (misionGatoCompletedEvent != null) misionGatoCompletedEvent();
                    nav.SetDestination(player.transform.position);
                }
            }

            if (playerMov.isMoving && siguiendo)
            {
                nav.SetDestination(player.transform.position);
            }
        }
    }

    private bool ActivarIconoInteractuar()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= radio && !siguiendo)
        {
            iconAnimator.SetBool("StartTalk", true);
            return true;

        }
        else
        {
            iconAnimator.SetBool("StartTalk", false);
            return false;
        }
    }
}
