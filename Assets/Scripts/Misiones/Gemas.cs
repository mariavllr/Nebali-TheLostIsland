using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gemas : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    public float radio;
    [SerializeField] GameObject containerRoja;
    [SerializeField] GameObject containerAmarilla;
    [SerializeField] GameObject containerVerde;
    [SerializeField] ParticleSystem llama;
    private bool roja, amarilla, verde, final;
    private SpriteRenderer icono;

    public delegate void FinalEvent();
    public static event FinalEvent onFinalEvent;
    void Start()
    {
        roja = false; amarilla = false; verde = false; final = false;
        icono = transform.GetChild(0).GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {

        ColocarGema();

        if (verde && amarilla && roja && !final)
        {
            //Desbloquear fin del juego
            final = true;
            if (onFinalEvent != null) onFinalEvent();
            StartCoroutine(Final());
        }
    }

    void ColocarGema()
    {
        if (Vector3.Distance(gameManager.player.transform.position, transform.position) <= radio)
        {
            if (icono.color.a == 0)
            {
                gameManager.MostrarMensaje("Coloca las gemas para llamar al dios Nebali.");
                StartCoroutine(fade(icono, 1f, true));
            } 

            //Si tiene un objeto
            if (gameManager.objetoEnMano != null && Input.GetKeyDown(gameManager.atacar))
            {
                GameObject gemaColocada;
                switch (gameManager.objetoEnMano.tag)
                {
                    case "GemaVerde":
                        gemaColocada = containerVerde.transform.GetChild(0).gameObject;
                        gemaColocada.SetActive(true);
                        Destroy(gameManager.objetoEnMano);
                        if (gemaColocada.TryGetComponent<ItemObject>(out ItemObject item))
                        {
                            Debug.Log(item.referenceItem.displayName);
                            gameManager.GetComponent<InventorySystem>().Remove(item.referenceItem);
                            gameManager.MostrarMensaje("Gema verde colocada con éxito.");
                            verde = true;
                        }
                        else Debug.Log("error al colocar gema verde");

                        break;
                    case "GemaRoja":
                        gemaColocada = containerRoja.transform.GetChild(0).gameObject;
                        gemaColocada.SetActive(true);
                        Destroy(gameManager.objetoEnMano);
                        if (gemaColocada.TryGetComponent<ItemObject>(out ItemObject item2))
                        {
                            Debug.Log(item2.referenceItem.displayName);
                            gameManager.GetComponent<InventorySystem>().Remove(item2.referenceItem);
                            gameManager.MostrarMensaje("Gema roja colocada con éxito.");
                            roja = true;
                        }
                        else Debug.Log("error al colocar gema roja");
                        break;
                    case "GemaAmarilla":
                        gemaColocada = containerAmarilla.transform.GetChild(0).gameObject;
                        gemaColocada.SetActive(true);
                        Destroy(gameManager.objetoEnMano);
                        if (gemaColocada.TryGetComponent<ItemObject>(out ItemObject item3))
                        {
                            Debug.Log(item3.referenceItem.displayName);
                            gameManager.GetComponent<InventorySystem>().Remove(item3.referenceItem);
                            gameManager.MostrarMensaje("Gema amarilla colocada con éxito.");
                            amarilla = true;
                        }
                        else Debug.Log("error al colocar gema amarilla");
                        break;
                    default:
                        gameManager.MostrarMensaje("Tienes que colocar una gema.");
                        break;
                }
            }
        }

        else
        {
            if(icono.color.a == 1) StartCoroutine(fade(icono, 1f, false));
        }
    }

    IEnumerator Final()
    {
        gameManager.GetComponent<CinemachineSwitcher>().SwitchPriority("VerFinal");
        yield return new WaitForSeconds(2);
        llama.Play();
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
