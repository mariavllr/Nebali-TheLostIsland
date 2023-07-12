using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anzuelo : MonoBehaviour
{
    private bool tocaAgua = false;
    public delegate void FueraAguaEvent();
    public static event FueraAguaEvent onFueraAguaEvent;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Agua")
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).GetComponent<ParticleSystem>().Play();

            tocaAgua = true;
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.mass = 100;
            rb.angularDrag = 10;
            rb.drag = 10;
        }
        else if(collision.gameObject.tag == "Pez")
        {
            return;
        }
        else
        {
            Destroy(gameObject);
            if (onFueraAguaEvent != null) onFueraAguaEvent();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PuntoLanzamiento" && tocaAgua)
        {
            Destroy(gameObject);
        }
    }
}
