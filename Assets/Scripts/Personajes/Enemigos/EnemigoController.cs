using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemigoController : MonoBehaviour
{
    [SerializeField] GameObject player;

    [Header("Patrulla")]
    public List<Transform> waypointsPatrulla;
    private Transform waypointActual;

    [Header("Alerta")]
    public float tiempoAlerta;
    public Animator iconAnimator;

    [Header("Ataque")]
    public int vida;
    public int dañoAtaque;
    public float tiempoAtaque;
    public float tiempoDaño;
    public float radioDeteccion;

    private float tiempo = 0;
    private NavMeshAgent nav;
    private MeshRenderer mrenderer;
    private EstadosEnemigo estadoActual;

    private enum EstadosEnemigo
    {
        Patrulla,
        Alerta,
        Ataque
    }

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        mrenderer = GetComponent<MeshRenderer>();

        estadoActual = EstadosEnemigo.Patrulla;
        int rand = Random.Range(0, waypointsPatrulla.Count);
        waypointActual = waypointsPatrulla[rand];
        nav.destination = waypointActual.position;
    }

    void Update()
    {      
        if(vida <= 0)
        {
            Destroy(gameObject);
        }

        switch (estadoActual)
        {
            case EstadosEnemigo.Patrulla:
                Patrulla();
                break;
            case EstadosEnemigo.Alerta:
                Alerta();
                break;
            case EstadosEnemigo.Ataque:
                Ataque();
                break;
            default:
                break;
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

    void Patrulla()
    {
        
        //Si se ha llegado al waypoint, elegir un nuevo waypoint y repetir
        if (Vector3.Distance(transform.position, waypointActual.position) < 2)
        {
            int rand = Random.Range(0, waypointsPatrulla.Count); 
            //Si ha salido el que ya estaba, repite
            while(waypointsPatrulla[rand] == waypointActual)
            {
                rand = Random.Range(0, waypointsPatrulla.Count);
            }

            waypointActual = waypointsPatrulla[rand];
            nav.destination = waypointActual.position;
        }

        //Si está cerca del jugador se alerta
        if (Vector3.Distance(transform.position, player.transform.position) < radioDeteccion &&
            Vector3.Distance(transform.position, player.transform.position) > 0.1f)
        {
            estadoActual = EstadosEnemigo.Alerta;  
        }
    }

    void Alerta()
    {
        nav.isStopped = true;
        iconAnimator.SetBool("StartTalk", true);
        tiempo += Time.deltaTime;

        //Si se sale del rango, vuelve a patrullar
        if(Vector3.Distance(transform.position, player.transform.position) > radioDeteccion)
        {
            iconAnimator.SetBool("StartTalk", false);
            tiempo = 0;
            nav.destination = waypointActual.position;
            nav.isStopped = false;
            estadoActual = EstadosEnemigo.Patrulla;
        }

        //Si pasados unos segundos no sale del rango, ataca
        if (tiempo >= tiempoAlerta)
        {
            iconAnimator.SetBool("StartTalk", false);
            tiempo = 0;
            nav.isStopped = false;
            estadoActual = EstadosEnemigo.Ataque;
        }
    }

    void Ataque()
    {
        
        nav.speed = 6;
        nav.destination = player.transform.position;
    }

}
