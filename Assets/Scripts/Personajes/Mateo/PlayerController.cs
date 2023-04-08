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

        else if (Input.GetKeyDown(gameManager.abrirInventario))
        {
            AbrirInventario();
        }
    }

    private void Atacar()
    {
        animator.SetBool("Atacando", true);

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

    private void OnCollisionEnter(Collision collision)
    {
        //Ser atacado
        if(collision.gameObject.tag == "Enemigo")
        {
            vida -= 1; //collision.gameObject.GetComponent<EnemigoController>().dañoAtaque;
            animator.SetBool("Atacado", true);
        }
        
    }
}
