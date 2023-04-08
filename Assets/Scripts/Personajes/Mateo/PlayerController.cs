using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] int vida;
    [SerializeField] List<Vector3> posicionInicialNiveles;

    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        posicionInicialNiveles = new List<Vector3>();
    }

    void Update()
    {
        if (vida <= 0)
        {
            animator.SetBool("Desmayo", true);
            Reiniciar();
        }

        if (Input.GetKeyDown(gameManager.atacar))
        {
            Atacar();
        }

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            //Si se ha acabado la animación, volver a idle
            animator.SetBool("Attack", false);
        }

        else if (Input.GetKeyDown(gameManager.abrirInventario))
        {
            AbrirInventario();
        }
    }

    private void Atacar()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.Play("Attack"); //reiniciar si ya estaba atacando

        }
        else animator.SetBool("Attack", true);

    }

    private void AbrirInventario()
    {

    }

    private void Reiniciar()
    {
        switch (gameManager.nivelActual)
        {
            case GameManager.Nivel.Bosque:
                transform.position = posicionInicialNiveles[0];
                break;
            case GameManager.Nivel.Isla:
                transform.position = posicionInicialNiveles[1];
                break;
            case GameManager.Nivel.Granja:
                transform.position = posicionInicialNiveles[2];
                break;
        }
    }

    //ATAQUE
    private void OnCollisionEnter(Collision collision)
    {
        //Enemigo ataca jugador
        if(collision.gameObject.tag == "Enemigo")
        {
            vida -= 1; //collision.gameObject.GetComponent<EnemigoController>().dañoAtaque;
            animator.SetBool("Atacado", true);
        }
        
    }
}
