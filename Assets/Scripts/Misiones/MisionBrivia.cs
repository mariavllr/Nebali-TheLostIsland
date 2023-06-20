using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MisionBrivia : MonoBehaviour
{
    //Brivia deberá eliminar al maximo de bichos que pueda
    public GameManager gameManager;
    private NavMeshAgent nav;
    private Animator animator;

    [Header("Ataque")]
    [SerializeField] float radioDeteccion;
    [SerializeField] float tiempoEntreAtaques;
    private bool atacando;

    [Header("Enemigos")]
    public int numeroEnemigosDerrotados = 0;
    private GameObject enemigoActual = null;

    [SerializeField] LayerMask enemyLayer;
    [SerializeField] private float rangoAtaque;
    [SerializeField] private int dañoAtaque;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(gameManager.misionActual == GameManager.Mision.Ninguna)
        {
            if(nav.velocity != Vector3.zero) animator.SetBool("IsWalking", true);
            else animator.SetBool("IsWalking", false);

            //Buscar si hay enemigos en un rango. Si los hay, elegir uno aleatorio
            if (enemigoActual == null) BuscarEnemigos();

            //Atacar hasta matarlo
            else
            {
                nav.destination = enemigoActual.transform.position;
                if (!atacando && Vector3.Distance(transform.position, enemigoActual.transform.position) < 3)
                {
                    atacando = true;
                    StartCoroutine(Atacar());
                }

            }
        }
    }

    void BuscarEnemigos()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radioDeteccion, enemyLayer);
        if (hitColliders.Length > 0) enemigoActual = hitColliders[0].gameObject;
        else enemigoActual = null;
    }

    IEnumerator Atacar()
    {
        Debug.Log("ATAQUE");
        // Lanza un rayo desde la posición y la dirección del jugador
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        animator.SetTrigger("Slash");

        Debug.DrawRay(ray.origin, ray.direction * rangoAtaque, Color.red, 0.6f);

        // Comprueba si el rayo colisiona con algún objeto en la capa de los enemigos
        if (Physics.Raycast(ray, out hit, rangoAtaque, enemyLayer, QueryTriggerInteraction.Collide))
        {
            // Obtiene el componente del enemigo
            EnemigoController enemyHealth = hit.collider.gameObject.transform.parent.GetComponentInParent<EnemigoController>();

            // Verifica si el enemigo tiene el componente EnemyHealth
            if (enemyHealth != null)
            {
                // Reduce la vida del enemigo
                if ((enemyHealth.vida - dañoAtaque) == 0) numeroEnemigosDerrotados++;
                enemyHealth.TakeDamage(dañoAtaque);
            }
        }

        yield return new WaitForSeconds(tiempoEntreAtaques);
        atacando = false;

    }
}
