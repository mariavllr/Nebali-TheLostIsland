using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    [Header("Vida")]
    [SerializeField] public int vida = 5;
    private int vidaActual;
    public bool dead = false;
    [SerializeField] GameObject vidasContainer;
    [SerializeField] GameObject prefabCorazon;
    [SerializeField] List<Vector3> posicionInicialZonas;

    [Header("Enemigos")]
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] private float rangoAtaque;
    [SerializeField] private int da�oAtaque;

    [Header("Sonido")]
    [SerializeField] AudioSource damageSound;
    [SerializeField] AudioSource attackSound;
    [SerializeField] AudioSource eatSound;
    
    private PlayerMovement playerMov;
    private float tiempo;
    private Animator animator;

    private Coroutine fadeCoroutine;
    private bool puedeSentarse = false;
    private bool comiendo = false;
    private SpriteRenderer icono;

    private GameManager.Zona zonaPrevia;
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
        SitZone();

        //Gestion de vida

        if (vidaActual <= 0)
        {
            animator.SetBool("PassOut", true);
            playerMov.canMove = false;
            StartCoroutine(Reiniciar());
        }

        else
        {
            if (Input.GetKeyDown(gameManager.atacar) && gameManager.tieneObjetoEnMano && gameManager.objetoEnMano != null && gameManager.objetoEnMano.transform.tag == "Espada")
            {
                Atacar();
            }

            if (Input.GetKeyUp(gameManager.atacar) && !comiendo && gameManager.tieneObjetoEnMano && gameManager.objetoEnMano != null && gameManager.objetoEnMano.transform.tag == "Pez")
            {
                ComerPez();
            }
        }

    }

    private void ComerPez()
    {
        GameObject pez = gameManager.objetoEnMano;

        if (vidaActual < vida)
        {
            comiendo = true;           
            //Sumar 1 vida si no esta completo ya
            vidaActual++;
            DibujarCorazones();

            //Borrar del inventario
            if (pez.TryGetComponent<ItemObject>(out ItemObject item))
            {
                Debug.Log(item.referenceItem.displayName);
                gameManager.GetComponent<InventorySystem>().Remove(item.referenceItem);
            }

            //Borrar de la mano
            Destroy(gameManager.objetoEnMano);
            eatSound.Play();
            comiendo = false;
        }     
    }

    private void OnTriggerEnter(Collider other)
    {
        //Coger objeto
        if(other.gameObject.TryGetComponent<ItemObject>(out ItemObject item))
        {
            item.OnHandlePickupItem();
        }

        if (other.gameObject.tag == "SitZone")
        {
            puedeSentarse = true;
            icono = other.transform.parent.GetChild(3).gameObject.GetComponentInChildren<SpriteRenderer>();
        }

        //Zonas
        if (other.gameObject.tag == "ZonaBosque")
        {
            gameManager.zonaActual = GameManager.Zona.Bosque;
        }
        else if (other.gameObject.tag == "ZonaPueblo")
        {
            gameManager.zonaActual = GameManager.Zona.Pueblo;
        }

        else if (other.gameObject.tag == "ZonaGranja")
        {
            zonaPrevia = gameManager.zonaActual;
            gameManager.zonaActual = GameManager.Zona.Granja;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Sentarse
        if (other.gameObject.tag == "SitZone")
        {
            puedeSentarse = false;
            SpriteRenderer icono = other.transform.parent.GetChild(3).gameObject.GetComponentInChildren<SpriteRenderer>();
            StartCoroutine(fade(icono, 1f, false));
        }

        //Zonas
        if (other.gameObject.tag == "ZonaGranja")
        {
            gameManager.zonaActual = zonaPrevia;
            Debug.Log("salir granja, zona actual: " + gameManager.zonaActual);
        }

    }

    void SitZone()
    {
        if (puedeSentarse)
        {
            //icono = other.transform.parent.GetChild(3).gameObject.GetComponentInChildren<SpriteRenderer>();
            fadeCoroutine = StartCoroutine(fade(icono, 1f, true));

            if (Input.GetKeyDown(gameManager.hablar) && !animator.GetBool("Sitting"))
            {
                Debug.Log("sentarse");
                //Si no esta sentado que se siente
                animator.SetBool("Sitting", true);
                StopCoroutine(fadeCoroutine);
                fadeCoroutine = StartCoroutine(fade(icono, 1f, false));
                playerMov.canMove = false;
            }

            else if (Input.GetKeyUp(gameManager.hablar) && animator.GetBool("Sitting") && animator.GetCurrentAnimatorStateInfo(0).IsName("Sitting Idle"))
            {
                Debug.Log("levantarse");
                //Si esta sentado que se levante
                animator.SetBool("Sitting", false);
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Happy Idle"))
            {
                playerMov.canMove = true;
            }
        }
    }

    IEnumerator fade(SpriteRenderer MyRenderer, float duration, bool fadeIn)
    {
        float counter = 0;
        //Get current color
        Color spriteColor = MyRenderer.material.color;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha;
            //Fade from 1 to 0 or viceversa
            if (fadeIn) alpha = Mathf.Lerp(0, 1, counter / duration);
            else alpha = Mathf.Lerp(1, 0, counter / duration);

            //Change alpha only
            MyRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            //Wait for a frame
            yield return null;
        }
    }

    private void Atacar()
    {
        GameObject espada = gameManager.objetoEnMano;

        // Lanza un rayo desde la posici�n y la direcci�n del jugador
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * rangoAtaque, Color.red, 0.5f);

        // Comprueba si el rayo colisiona con alg�n objeto en la capa de los enemigos
        if (Physics.Raycast(ray, out hit, rangoAtaque, enemyLayer, QueryTriggerInteraction.Collide))
        {
            // Obtiene el componente del enemigo
            EnemigoController enemyHealth = hit.collider.GetComponentInParent<EnemigoController>();

            // Verifica si el enemigo tiene el componente EnemyHealth
            if (enemyHealth != null)
            {
                // Reduce la vida del enemigo
                enemyHealth.TakeDamage(da�oAtaque);
                espada.transform.GetChild(espada.transform.childCount-1).gameObject.GetComponent<ParticleSystem>().Play();
            }
        }
        
        animator.SetTrigger("Attack");
        attackSound.Play();

        //Ejecutar vfx random de la espada, excepto el ultimo que es el hit
        
        espada.transform.GetChild(Random.Range(0, espada.transform.childCount-1)).gameObject.GetComponent<ParticleSystem>().Play();

    }

    private IEnumerator Reiniciar()
    {
        vidaActual = vida;
        dead = true;
        yield return new WaitForSeconds(5);
        gameObject.GetComponent<CharacterController>().enabled = false;

        DibujarCorazones();

        switch (gameManager.zonaActual)
        {
            case GameManager.Zona.Bosque:
                transform.localPosition = posicionInicialZonas[0];
                break;
            case GameManager.Zona.Pueblo:
                transform.localPosition = posicionInicialZonas[1];
                break;
            case GameManager.Zona.Granja:
                transform.localPosition = posicionInicialZonas[2];
                break;
            default:
                transform.localPosition = posicionInicialZonas[0];
                Debug.Log("Error, no esta en ninguna zona");
                break;
        }

        animator.SetBool("PassOut", false);
        playerMov.canMove = true;
        dead = false;
        gameObject.GetComponent<CharacterController>().enabled = true;

    }

    private void DibujarCorazones()
    {
        //Borrar container
        for (int i = 0; i < vidasContainer.transform.childCount; i++)
        {
            Destroy(vidasContainer.transform.GetChild(i).gameObject);
        }

        //Redibujar
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
            if (tiempo >= enemigo.tiempoAtaque && vidaActual > 0)
            {
                animator.SetTrigger("Hit");
                damageSound.Play();
                vidaActual -= enemigo.da�oAtaque;
                tiempo = 0;
                Destroy(vidasContainer.transform.GetChild(0).gameObject);
            } 
        }
    }
}

