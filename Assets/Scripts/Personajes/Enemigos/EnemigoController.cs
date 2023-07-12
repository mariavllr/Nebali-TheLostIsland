using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemigoController : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject player;
    private PlayerMovement playerMov;
    [SerializeField] GameObject modelo;

    [Header("Patrulla")]
    public List<Transform> waypointsPatrulla;
    private Transform waypointActual;

    [Header("Alerta")]
    public float tiempoAlerta;
    public Animator iconAnimator;

    [Header("Ataque")]
    public bool muerto = false;
    public int vida;
    public int dañoAtaque; //El daño que hará al otro cada vez que le toque
    public float tiempoAtaque; //El tiempo que tiene que esperar entre cada ataque (sino al tocarlo quitaría todos los corazones de una)
    public float tiempoDaño; //El tiempo que se queda atontado cuando le pegan
    public float radioDeteccion; //Radio donde detecta al jugador
    public float radioDeteccionAtaque; //Radio donde si se sale deja de atacar
    public float fuerzaImpulsoAtaque; //Lo que se impulsa al jugador cuando le ataca
    public float fuerzaImpulsoRecibeDaño; //Lo que se echa hacia atrás cuando le pegan

    [Header("VFX")]
    [SerializeField] ParticleSystem explosion;
    [SerializeField] AudioSource hitSound;
    [SerializeField] AudioSource deadSound;

    private Coroutine ataqueCoroutine;
    [SerializeField] private Color colorAtacado;
    private Color colorOriginal;

    private float tiempo = 0;
    private NavMeshAgent nav;
    [SerializeField] private MeshRenderer mrenderer;
    private Rigidbody rb;
    public EstadosEnemigo estadoActual;

    public enum EstadosEnemigo
    {
        Patrulla,
        Alerta,
        Ataque
    }

    void Start()
    {
        ManagerDialogos.onDialogueOpenedEvent += EnDialogo;

        nav = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        playerMov = player.GetComponent<PlayerMovement>();

        estadoActual = EstadosEnemigo.Patrulla;
        int rand = Random.Range(0, waypointsPatrulla.Count);
        waypointActual = waypointsPatrulla[rand];
        nav.destination = waypointActual.position;

        colorOriginal = mrenderer.material.GetColor("_MainColor");
    }

    private void OnDisable()
    {
        ManagerDialogos.onDialogueOpenedEvent -= EnDialogo;
    }

    void Update()
    {
        if (!muerto)
        {
            if(vida == 0)
            {
                StopAllCoroutines();
                StartCoroutine(Morir());
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

    }

    void EnDialogo()
    {
        //Cuando el personaje se pone a hablar los personajes pasan de él
        waypointActual = waypointsPatrulla[Random.Range(0, waypointsPatrulla.Count)];
        nav.destination = waypointActual.position;
        estadoActual = EstadosEnemigo.Patrulla;
    }


    public void TakeDamage(int daño)
    {
        StartCoroutine(Daño(daño));
    }

    IEnumerator Morir()
    {
        deadSound.Play();
        muerto = true;
        nav.enabled = false;

        string zona = transform.parent.tag;

        switch (zona)
        {
            case "ZonaBosque":
                gameManager.enemigosBosque--;
                //if(gameManager.enemigosBosque == 0) gameManager.MostrarMensaje(" bosque.");
                gameManager.MostrarMensaje("Te quedan " + gameManager.enemigosBosque + " Boo's en el bosque.");
                break;
            case "ZonaPueblo":
                gameManager.enemigosPueblo--;
                gameManager.MostrarMensaje("Te quedan " + gameManager.enemigosPueblo + " Boo's en el pueblo.");
                break;
            case "ZonaGranja":
                gameManager.enemigosGranja--;
                gameManager.MostrarMensaje("Te quedan " + gameManager.enemigosGranja + " Boo's en la granja.");
                break;
            default:
                Debug.Log("Error! No es de ninguna zona el enemigo");
                break;
        }
        
        Destroy(modelo);
        explosion.gameObject.SetActive(true);
        explosion.Play();
        
        GetComponent<SphereCollider>().enabled = false;

        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    IEnumerator Daño(int dañoRecibido)
    {
        hitSound.Play();
        vida -= dañoRecibido;
        mrenderer.material.SetColor("_MainColor", colorAtacado);
        rb.AddForce(-transform.forward * fuerzaImpulsoRecibeDaño, ForceMode.Impulse);

        yield return new WaitForSeconds(tiempoDaño);

        mrenderer.material.SetColor("_MainColor", colorOriginal);
    }

    void Patrulla()
    {
        
        //Si se ha llegado al waypoint, elegir un nuevo waypoint y repetir
        if (Vector3.Distance(transform.position, waypointActual.position) < 3)
        {
            int rand = Random.Range(0, waypointsPatrulla.Count); 
            //Si ha salido el que ya estaba, repite
            if(waypointsPatrulla.Count > 1)
            {
                while (waypointsPatrulla[rand] == waypointActual)
                {
                    rand = Random.Range(0, waypointsPatrulla.Count);
                }
                waypointActual = waypointsPatrulla[rand];
                nav.destination = waypointActual.position;
            }
        }

        //Si está cerca del jugador se alerta ( y no esta muerto ni conversando)
        if (CercaJugador(radioDeteccion) && playerMov.canMove)
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

        if (player.GetComponent<PlayerController>().dead)
        {
            waypointActual = waypointsPatrulla[Random.Range(0, waypointsPatrulla.Count)];
            nav.destination = waypointActual.position;
            estadoActual = EstadosEnemigo.Patrulla;
        }
    }

    void Ataque()
    {
        if (!muerto && CercaJugador(radioDeteccionAtaque) && playerMov.canMove)
        {
            nav.speed = 7;
            nav.destination = player.transform.position;

            if (ataqueCoroutine == null)
            {
                ataqueCoroutine = StartCoroutine(EjecutarUnAtaque());
            }

            if (player.GetComponent<PlayerController>().dead)
            {
                waypointActual = waypointsPatrulla[Random.Range(0, waypointsPatrulla.Count)];
                nav.destination = waypointActual.position;
                estadoActual = EstadosEnemigo.Patrulla;
            }
        }

        else
        {
            waypointActual = waypointsPatrulla[Random.Range(0, waypointsPatrulla.Count)];
            nav.destination = waypointActual.position;
            estadoActual = EstadosEnemigo.Patrulla;
        } 
    }

    bool CercaJugador(float radio)
    {
        if (Vector3.Distance(transform.position, player.transform.position) < radio &&
            Vector3.Distance(transform.position, player.transform.position) > 0.1f && !player.GetComponent<PlayerController>().dead)
        {
            return true;
        }
        else return false;
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
