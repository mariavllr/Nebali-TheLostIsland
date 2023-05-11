using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    private PlayerMovement playerMov;

    [Header("Controles")]
    public KeyCode hablar;
    public KeyCode atacar;
    public KeyCode abrirInventario;

    public Zona zonaActual;
    public Mision misionActual;

    /*[Header("Misiones")]
    public List<bool> misionesCompletadas;*/

    [Header("Musica")]
    [SerializeField] List<AudioClip> canciones;
    [SerializeField] AudioSource audioSource;


    [Header("UI")]
    [SerializeField] GameObject menuInicial;
    [SerializeField] GameObject inventario;
    [SerializeField] TextMeshProUGUI textmesh;
    [SerializeField] float segundosMensajeUI;

    public delegate void OnInventoryOpenedEvent();
    public static event OnInventoryOpenedEvent onInventoryOpenedEvent;

    //Zona donde puede spawnear el personaje si el enemigo le mata
    public enum Zona
    {
        Menu,
        Bosque,
        Isla,
        Granja
    }

    //Misiones secundarias
    public enum Mision
    {
        ConocerAldeanos,
        GatoLeeba,
        Brivia,
        Ninguna
    }



    void Start()
    {
        zonaActual = Zona.Menu;
        misionActual = Mision.Ninguna;
       // canciones = new List<AudioClip>();
        playerMov = player.GetComponentInChildren<PlayerMovement>();

    }

    void Update()
    {
        if(zonaActual == Zona.Menu)
        {
            playerMov.canMove = false;
        }

        //Inventario

        if (Input.GetKeyDown(abrirInventario) && !inventario.activeInHierarchy)
        {
            AbrirInventario();
        }
        else if (Input.GetKeyDown(abrirInventario) && inventario.activeInHierarchy)
        {
            CerrarInventario();
        }
    }

    public void MostrarMensaje(string msg)
    {
        textmesh.text = msg;
        StartCoroutine(BorrarMensaje());
    }

    IEnumerator BorrarMensaje()
    {
        yield return new WaitForSeconds(segundosMensajeUI);
        textmesh.text = "";
    }

    public void CerrarMenu()
    {
        menuInicial.SetActive(false);
        zonaActual = Zona.Bosque;
        playerMov.canMove = true;
        CambiarCancion();
    }

    public void AbrirInventario()
    {
        
        inventario.SetActive(true);
        playerMov.canMove = false;
        if (onInventoryOpenedEvent != null) onInventoryOpenedEvent();
    }

    public void CerrarInventario()
    {
        inventario.SetActive(false);
        playerMov.canMove = true;
    }

    void CambiarCancion()
    {
        switch (zonaActual)
        {
            case Zona.Menu:
                audioSource.clip = canciones[0];
                audioSource.Play();
                break;
            case Zona.Bosque:
                audioSource.clip = canciones[1];
                audioSource.Play();
                break;
            case Zona.Isla:
                audioSource.clip = canciones[2];
                audioSource.Play();
                break;
            case Zona.Granja:
                audioSource.clip = canciones[3];
                audioSource.Play();
                break;
        }
    }
}
