using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoController : MonoBehaviour
{
    [SerializeField] float vida;
    [SerializeField] float dañoAtaque;

    void Start()
    {
        
    }

    void Update()
    {
        if(vida <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //Jugador ataca enemigo
        if (other.gameObject.tag == "Espada")
        {
            Debug.Log("Atacado!");
            vida -= 1;
        }
    }

}
