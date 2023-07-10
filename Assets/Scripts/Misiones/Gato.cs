using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Gato : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject player;
    private PlayerMovement playerMov;
    //public Animator iconAnimator;
    public SpriteRenderer icono;
    public int radio;

    private NavMeshAgent nav;
    private bool siguiendo = false;
    private Animator animator;

    public delegate void MisionGatoCompletedEvent();
    public static event MisionGatoCompletedEvent misionGatoCompletedEvent;


    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        playerMov = player.GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
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

        if(nav.velocity != Vector3.zero)
        {
            animator.SetBool("Following", true);
            
        }
        else
        {
            animator.SetBool("Following", false);
        }
    }

    private bool ActivarIconoInteractuar()
    {
        if (gameManager.misionActual == GameManager.Mision.GatoLeeba)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= radio && !siguiendo && gameManager.tieneObjetoEnMano && gameManager.objetoEnMano.tag == "Pez")
            {
                //iconAnimator.SetBool("StartTalk", true);               
                if (icono.color.a == 0) StartCoroutine(fade(icono, 1f, true));
                return true;

            }
            else
            {
                //iconAnimator.SetBool("StartTalk", false);
                if(icono.color.a == 1) StartCoroutine(fade(icono, 1f, false));
                return false;
            }
        }

        return false;
        
    }

    IEnumerator fade(SpriteRenderer MyRenderer, float duration, bool fadeIn)
    {
        float counter = 0;
        //Get current color
        Color spriteColor = MyRenderer.material.color;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha;
            //Fade from 1 to 0 or viceversa
            if (fadeIn) alpha = Mathf.Lerp(0, 1, counter / duration);
            else alpha = Mathf.Lerp(1, 0, counter / duration);

            //Change alpha only
            MyRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            //Wait for a frame
            yield return null;
        }
    }
}
