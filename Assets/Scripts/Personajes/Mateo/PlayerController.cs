using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    [Header("Vida")]
    [SerializeField] public int vida = 5;
    private int vidaActual;
    [SerializeField] GameObject vidasContainer;
    [SerializeField] GameObject prefabCorazon;
    [SerializeField] List<Vector3> posicionInicialZonas;

    [Header("Enemigos")]
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] private float rangoAtaque;
    [SerializeField] private int dañoAtaque;
    
    private PlayerMovement playerMov;
    private float tiempo;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        playerMov = GetComponent<PlayerMovement>();
        tiempo = 0;
        vidaActual = vida;

        DibujarCorazones();
    }

    void Update()
    {
        tiempo += Time.deltaTime;

        //Gestion de vida

        if (vidaActual <= 0)
        {
            animator.SetBool("PassOut", true);
            playerMov.canMove = false;
            StartCoroutine(Reiniciar());
        }

        else
        {
            if (Input.GetKeyDown(gameManager.atacar) && gameManager.tieneObjetoEnMano && gameManager.objetoEnMano.transform.tag == "Espada")
            {
                Atacar();
            }

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                //Si se ha acabado la animación, volver a idle
                animator.SetBool("Attack", false);
            }
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

    private void Atacar()
    {
        // Lanza un rayo desde la posición y la dirección del jugador
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * rangoAtaque, Color.red, 0.5f);

        // Comprueba si el rayo colisiona con algún objeto en la capa de los enemigos
        if (Physics.Raycast(ray, out hit, rangoAtaque, enemyLayer))
        {
            // Obtiene el componente del enemigo
            EnemigoController enemyHealth = hit.collider.GetComponentInParent<EnemigoController>();

            // Verifica si el enemigo tiene el componente EnemyHealth
            if (enemyHealth != null)
            {
                // Reduce la vida del enemigo
                enemyHealth.TakeDamage(dañoAtaque);
            }
        }
        
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animator.Play("Attack"); //reiniciar si ya estaba atacando

        }
        else animator.SetTrigger("Attack"); 

        

    }

    private IEnumerator Reiniciar()
    {
        vidaActual = vida;

        yield return new WaitForSeconds(5);

        DibujarCorazones();

        switch (gameManager.zonaActual)
        {
            case GameManager.Zona.Bosque:
                transform.localPosition = posicionInicialZonas[0];
                break;
            case GameManager.Zona.Pueblo:
                transform.position = posicionInicialZonas[1];
                break;
            case GameManager.Zona.Granja:
                transform.position = posicionInicialZonas[2];
                break;
            default:
                break;
        }

        animator.SetBool("PassOut", false);
        playerMov.canMove = true;

    }

    private void DibujarCorazones()
    {
        for (int i = 0; i < vidaActual; i++)
        {
            Instantiate(prefabCorazon, vidasContainer.transform);
        }
    }

    //take damage
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Enemigo ataca jugador
        if (hit.gameObject.tag == "Enemigo")
        {
            EnemigoController enemigo = hit.gameObject.GetComponent<EnemigoController>();
            if (tiempo >= enemigo.tiempoAtaque && vidaActual != 0)
            {              
                vidaActual -= enemigo.dañoAtaque;
                tiempo = 0;
                Destroy(vidasContainer.transform.GetChild(0).gameObject);
                animator.SetTrigger("Hit");
            } 
        }
    }
}

