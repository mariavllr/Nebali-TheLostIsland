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

    public Nivel nivelActual;

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

    public enum Nivel
    {
        Menu,
        Bosque,
        Isla,
        Granja
    }

    void Start()
    {
        nivelActual = Nivel.Menu;
       // canciones = new List<AudioClip>();
        playerMov = player.GetComponentInChildren<PlayerMovement>();

    }

    void Update()
    {
        if(nivelActual == Nivel.Menu)
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
        nivelActual = Nivel.Bosque;
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
        switch (nivelActual)
        {
            case Nivel.Menu:
                audioSource.clip = canciones[0];
                audioSource.Play();
                break;
            case Nivel.Bosque:
                audioSource.clip = canciones[1];
                audioSource.Play();
                break;
            case Nivel.Isla:
                audioSource.clip = canciones[2];
                audioSource.Play();
                break;
            case Nivel.Granja:
                audioSource.clip = canciones[3];
                audioSource.Play();
                break;
        }
    }
}
