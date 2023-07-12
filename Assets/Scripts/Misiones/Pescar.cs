using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pescar : MonoBehaviour
{
    public GameManager gameManager;
    private Animator animator;

    public GameObject anzueloPrefab;
    public GameObject inicioLanzamiento;

    [Header("Propiedades del anzuelo")]
    private bool elegirImpulso = true;
    private bool lanzado = false;
    private GameObject anzueloLanzado;

    public Slider sliderImpulso;
    private float tiempoImpulso;
    private float valorImpulso;
    public float maxImpulso;
    private int clicks = 0;

    public List<GameObject> peces;


    void Start()
    {
        animator = GetComponent<Animator>();
        Anzuelo.onFueraAguaEvent += FueraDelAgua;
    }

    private void OnDisable()
    {
        Anzuelo.onFueraAguaEvent -= FueraDelAgua;
    }

    void Update()
    {
        //Si tiene la caña en la mano, entonces puede pescar.
        if (gameManager.tieneObjetoEnMano)
        {
            if(gameManager.objetoEnMano != null && gameManager.objetoEnMano.tag == "CanaPescar")
            {
                if (Input.GetKeyDown(gameManager.atacar))
                {
                                clicks++;
                                if (elegirImpulso && anzueloLanzado == null && clicks == 1)
                                {
                                    sliderImpulso.gameObject.SetActive(true);
                                    elegirImpulso = false;
                                }

                                else if (anzueloLanzado != null && lanzado && !elegirImpulso && clicks == 3)
                                {
                                    RecogerCanya();
                                }
                }

                            if (sliderImpulso.gameObject.activeInHierarchy && !lanzado)
                            {
                                CalcularSliderImpulso();
                            }

                          /*  if (anzueloLanzado == null && clicks == 2)
                            {
                                lanzado = false;
                                elegirImpulso = true;
                                sliderImpulso.gameObject.SetActive(false);
                                clicks = 0;
                                animator.SetBool("Pescar", false);
                            }*/
            }
            
        }
    }

    private void CalcularSliderImpulso()
    {
        tiempoImpulso += Time.deltaTime;
        valorImpulso = Mathf.PingPong(tiempoImpulso, 1); //This returns each impulseTime seconds a value between 0 and 1 and reverse
        sliderImpulso.value = valorImpulso;

        if (Input.GetKeyDown(gameManager.atacar) && !elegirImpulso && !lanzado && anzueloLanzado == null && clicks == 2)
        {
            Debug.Log("Lanzado");
            LanzarCanya();
        }
    }

    private void LanzarCanya()
    {
        animator.SetTrigger("LanzarCanya");
        animator.SetBool("Pescar", true);
        lanzado = true;

        StartCoroutine(LanzarAnzuelo());
        
    }

    IEnumerator LanzarAnzuelo()
    {
        yield return new WaitForSeconds(2);
        Vector3 dir = transform.forward;
        GameObject anzuelo = Instantiate(anzueloPrefab, inicioLanzamiento.transform.position, transform.rotation);
        Rigidbody rb = anzuelo.AddComponent<Rigidbody>();
        anzuelo.AddComponent<Anzuelo>();

        rb.AddForce(maxImpulso * valorImpulso * dir);
        anzueloLanzado = anzuelo;
        //poner que la variable de anzuelo lanzado solo se lea cuando acabe esta corrutina
    }

    private void RecogerCanya()
    {
        animator.SetBool("Pescar", false);
        
        lanzado = false;
        sliderImpulso.gameObject.SetActive(false);

        anzueloLanzado.GetComponent<Collider>().isTrigger = true;

        Rigidbody rb = anzueloLanzado.GetComponent<Rigidbody>();
        rb.AddForce((inicioLanzamiento.transform.position - anzueloLanzado.transform.position) * 400, ForceMode.Impulse);
        rb.mass = 1;
        rb.angularDrag = 0.05f;
        rb.drag = 0;
        rb.useGravity = false;

        elegirImpulso = true;
        clicks = 0;

        SacarPezAleatorio();

    }

    void FueraDelAgua()
    {
        animator.SetBool("Pescar", false);
        lanzado = false;
        elegirImpulso = true;
        sliderImpulso.gameObject.SetActive(false);
        clicks = 0;
    }

    private void SacarPezAleatorio()
    {
        int randomPez = UnityEngine.Random.Range(0, peces.Count);

        if(peces[randomPez] != null && anzueloLanzado != null)
        {
            Vector3 pos = new Vector3(anzueloLanzado.transform.localPosition.x, anzueloLanzado.transform.localPosition.y + 1f, anzueloLanzado.transform.localPosition.z);

            GameObject pez = Instantiate(peces[randomPez], pos, anzueloLanzado.transform.localRotation);
            Rigidbody rb = pez.AddComponent<Rigidbody>();
            rb.mass = 1;
            rb.angularDrag = 0.05f;
            rb.drag = 0;
            rb.AddForce((inicioLanzamiento.transform.position - anzueloLanzado.transform.position) * 2, ForceMode.Impulse);
        }
        else
        {
            Debug.Log("Error pesca: no existe pez o anzuelo");
        }
    }
}
