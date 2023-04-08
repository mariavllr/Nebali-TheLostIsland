using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemigoController : MonoBehaviour
{
    [SerializeField] GameObject player;

    public int vida;
    public int dañoAtaque;
    public float tiempoAtaque;
    public float radioDeteccion;

    private NavMeshAgent nav;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(vida <= 0)
        {
            Destroy(gameObject);
        }

        //Si está cerca del jugador lo sigue
        if(Vector3.Distance(transform.position, player.transform.position) < radioDeteccion)
        {
            nav.destination = player.transform.position;
        }



    }
    private void OnTriggerEnter(Collider other)
    {
        //Jugador ataca enemigo
        if (other.gameObject.tag == "Espada")
        {
            vida -= 1;          
        }
    }

}
