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
    private bool roja, amarilla, verde;
    void Start()
    {
        player = gameManager.player;
        roja = false; amarilla = false; verde = false;
    }

    void Update()
    {

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && gameManager.tieneObjetoEnMano)
        {
            //enseñar icono

            if (Input.GetKeyDown(gameManager.hablar))
            {
                switch (gameManager.objetoEnMano.tag)
                {
                    case "GemaVerde":
                        break;
                    case "GemaRoja":
                        break;
                    case "GemaAmarilla":
                        break;
                }

                
            }
            else gameManager.MostrarMensaje("Te faltan gemas...");

        }
    }
}
