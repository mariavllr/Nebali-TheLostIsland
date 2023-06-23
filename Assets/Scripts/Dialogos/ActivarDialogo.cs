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
    private AudioSource audioSource;
    private bool activarSonido = true;


    void Start()
    {
        animator = talkIcon.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //Si el icono de hablar está activo, no hay un diálogo ya abierto y se pulsa espacio, se abre el diálogo
        if (ActivarIconoConversacion())
        {
            if (gameObject.tag != "Nebali") gameObject.transform.forward = Vector3.Lerp(gameObject.transform.forward, -player.transform.forward, Time.deltaTime * 3f);
            //animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "AppearIcon" && 
            /*if (activarSonido)
            {
                audioSource.Play();
                activarSonido = false;
            }*/


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
                        Leeba leeba = gameObject.GetComponent<Leeba>();
                        managerDialogos.CambiarDialogo(leeba.miProximoDialogo);
                        if (leeba.miProximoDialogo == leeba.dialogos[1])
                        {
                            gameObject.GetComponent<Animator>().SetBool("DoSitups", false);
                            gameObject.GetComponent<Animator>().SetBool("FindCat", true);
                        }

                        gameObject.GetComponent<Animator>().SetBool("IsFishing", false);

                        break;
                    case "NEBALI":
                        managerDialogos.CambiarDialogo(gameObject.GetComponent<Nebali>().miProximoDialogo);
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
            if (gameObject.name == "NEBALI")
            {
                SpriteRenderer icono = transform.GetChild(0).GetComponentInChildren<SpriteRenderer>();

                if (icono.color.a == 0)
                {
                    StartCoroutine(fade(icono, 1f, true));
                }
                return true;
            }
            else
            {
                animator.SetBool("StartTalk", true);
                return true;
            }
        }
        else
        {
            if (gameObject.name == "NEBALI")
            {
                SpriteRenderer icono = transform.GetChild(0).GetComponentInChildren<SpriteRenderer>();

                if (icono.color.a != 0)
                {
                    StartCoroutine(fade(icono, 1f, false));
                }
                return false;
            }
            else
            {
                animator.SetBool("StartTalk", false);
                return false;
            }
        }

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
