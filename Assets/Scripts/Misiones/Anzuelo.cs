using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anzuelo : MonoBehaviour
{
    private bool tocaAgua = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Agua")
        {
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
