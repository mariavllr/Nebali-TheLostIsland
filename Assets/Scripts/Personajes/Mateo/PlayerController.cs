using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] int vida;
    [SerializeField] GameObject vidasContainer;
    [SerializeField] List<Vector3> posicionInicialNiveles;

    private float tiempo;

    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        posicionInicialNiveles = new List<Vector3>();
        tiempo = 0;
        GameManager.onInventoryOpenedEvent += OnInventoryChanged;
    }

    void Update()
    {
        tiempo += Time.deltaTime;

        //Gestion de vida

        if (vida <= 0)
        {
           // animator.SetBool("Desmayo", true);
          //  Reiniciar();
        }

        //Ataque

        if (Input.GetKeyDown(gameManager.atacar))
        {
            Atacar();
        }

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            //Si se ha acabado la animación, volver a idle
            animator.SetBool("Attack", false);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //Coger objeto
        if(other.gameObject.TryGetComponent<ItemObject>(out ItemObject item))
        {
            Debug.Log("Jugador colisiona con objeto");
            item.OnHandlePickupItem();
        }
    }

    void OnInventoryChanged()
    {
        
    }

    private void Atacar()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.Play("Attack"); //reiniciar si ya estaba atacando

        }
        else animator.SetBool("Attack", true);

    }

    private void Reiniciar()
    {
        switch (gameManager.zonaActual)
        {
            case GameManager.Zona.Bosque:
                transform.position = posicionInicialNiveles[0];
                break;
            case GameManager.Zona.Isla:
                transform.position = posicionInicialNiveles[1];
                break;
            case GameManager.Zona.Granja:
                transform.position = posicionInicialNiveles[2];
                break;
        }
    }

    //ATAQUE
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Enemigo ataca jugador
        if (hit.gameObject.tag == "Enemigo")
        {
            EnemigoController enemigo = hit.gameObject.GetComponent<EnemigoController>();
            if (tiempo >= enemigo.tiempoAtaque && vida != 0)
            {              
                vida -= enemigo.dañoAtaque;
                tiempo = 0;
                // animator.SetBool("Atacado", true);
                Destroy(vidasContainer.transform.GetChild(0).gameObject);
            } 
            
        }
    }
}

