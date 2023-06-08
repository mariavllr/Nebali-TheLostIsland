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
    public int da�oAtaque; //El da�o que har� al otro cada vez que le toque
    public float tiempoAtaque; //El tiempo que tiene que esperar entre cada ataque (sino al tocarlo quitar�a todos los corazones de una)
    public float tiempoDa�o; //El tiempo que se queda atontado cuando le pegan
    public float radioDeteccion; //Radio donde detecta al jugador
    public float fuerzaImpulsoAtaque; //Lo que se impulsa al jugador cuando le ataca
    public float fuerzaImpulsoRecibeDa�o; //Lo que se echa hacia atr�s cuando le pegan

    private Coroutine ataqueCoroutine;
    [SerializeField] private Color colorAtacado;
    private Color colorOriginal;

    private float tiempo = 0;
    private NavMeshAgent nav;
    [SerializeField] private MeshRenderer mrenderer;
    private Rigidbody rb;
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
        rb = GetComponent<Rigidbody>();

        estadoActual = EstadosEnemigo.Patrulla;
        int rand = Random.Range(0, waypointsPatrulla.Count);
        waypointActual = waypointsPatrulla[rand];
        nav.destination = waypointActual.position;

        colorOriginal = mrenderer.material.GetColor("_MainColor");
    }

    void Update()
    {      
        if(vida <= 0)
        {
            nav.enabled = false;
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


    public void TakeDamage(int da�o)
    {
        StartCoroutine(Da�o(da�o));
    }

    IEnumerator Da�o(int da�oRecibido)
    {
        Debug.Log("da�o");
        vida -= da�oRecibido;
        mrenderer.material.SetColor("_MainColor", colorAtacado);
        rb.AddForce(-transform.forward * fuerzaImpulsoRecibeDa�o, ForceMode.Impulse);

        yield return new WaitForSeconds(tiempoDa�o);

        mrenderer.material.SetColor("_MainColor", colorOriginal);
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

        //Si est� cerca del jugador se alerta
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
        nav.speed = 7;
        nav.destination = player.transform.position;
        
        if (ataqueCoroutine == null)
        {
            ataqueCoroutine = StartCoroutine(EjecutarUnAtaque());
        }
    }

    IEnumerator EjecutarUnAtaque()
    {
        switch (Random.Range(0, 2))
        {
            case 0:
                //No hace nada
                break;
            case 1:
                SprintHaciaJugador();
                break;
            case 2:
                Salto();
                break;
            default:
                break;
        }


        yield return new WaitForSeconds(tiempoAtaque);
        ataqueCoroutine = null;
    }

    //Ataques
    void SprintHaciaJugador()
    {
        rb.AddForce(transform.forward * fuerzaImpulsoAtaque, ForceMode.Impulse);
    }

    void Salto()
    {
        Vector3 vectorSalto = new Vector3(transform.forward.x, transform.forward.y + 50, transform.forward.z);
        rb.AddForce(transform.up * fuerzaImpulsoAtaque, ForceMode.Impulse);
    }

}
