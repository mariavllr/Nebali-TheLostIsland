using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gemas : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    private GameObject player;

    [SerializeField] GameObject containerRoja;
    [SerializeField] GameObject containerAmarilla;
    [SerializeField] GameObject containerVerde;
    [SerializeField] ParticleSystem llama;
    private bool roja, amarilla, verde, final;

    public delegate void FinalEvent();
    public static event FinalEvent onFinalEvent;
    void Start()
    {
        player = gameManager.player;
        roja = false; amarilla = false; verde = false; final = false;
    }

    void Update()
    {

        if(verde && amarilla && roja && !final)
        {
            //Desbloquear fin del juego
            final = true;
            if (onFinalEvent != null) onFinalEvent();
            StartCoroutine(Final());
        }
    }

    IEnumerator Final()
    {
        gameManager.GetComponent<CinemachineSwitcher>().SwitchPriority("VerFinal");
        yield return new WaitForSeconds(2);
        llama.Play();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player" && gameManager.tieneObjetoEnMano)
        {
            //enseñar icono

            if (Input.GetKeyDown(gameManager.hablar) && gameManager.objetoEnMano != null)
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
                        gameManager.MostrarMensaje("No tienes ninguna gema en la mano.");
                        break;
                }

                
            }
        }
    }
}
