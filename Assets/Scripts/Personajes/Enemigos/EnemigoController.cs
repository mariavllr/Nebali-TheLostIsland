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
    public float tiempoDaño;
    public float radioDeteccion;

    private float tiempo = 0;

    private NavMeshAgent nav;
    private MeshRenderer mrenderer;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        mrenderer = GetComponent<MeshRenderer>();

    }

    void Update()
    {      
        if(vida <= 0)
        {
            Destroy(gameObject);
        }

        //Si está cerca del jugador lo sigue
        if(Vector3.Distance(transform.position, player.transform.position) < radioDeteccion &&
            Vector3.Distance(transform.position, player.transform.position) > 0.1f)
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
           StartCoroutine(Daño());
        }
    }

    IEnumerator Daño()
    {
        mrenderer.material.color = Color.red;

        yield return new WaitForSeconds(tiempoDaño);

        mrenderer.material.color = Color.white;



    }

}
