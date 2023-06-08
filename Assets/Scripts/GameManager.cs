using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    private PlayerMovement playerMov;
    private CinemachineSwitcher cinemachineSwitcher;

    [Header("Controles")]
    public KeyCode hablar;
    public KeyCode atacar;
    public KeyCode abrirInventario;
    public KeyCode abrirMapa;
    public KeyCode pausar;

    public Zona zonaActual;
    public Mision misionActual;

    [Header("Inventario")]
    public GameObject objetoEnMano; //El objeto que tiene el jugador en la mano
    [SerializeField] private GameObject mano;
    public bool tieneObjetoEnMano = false;

    /*[Header("Misiones")]
    public List<bool> misionesCompletadas;*/

    [Header("Musica")]
    [SerializeField] List<AudioClip> canciones;
    [SerializeField] AudioSource audioSource;

    [Header("Mapa")]
    [SerializeField] GameObject cameraMapa;

    [Header("UI")]  
    [SerializeField] GameObject inventario;
    [SerializeField] GameObject containerCorazones;
    [SerializeField] TextMeshProUGUI textmesh;
    [SerializeField] float segundosMensajeUI;

    [Header("Menu principal")]
    [SerializeField] GameObject menuInicial;

    [Header("Menu pausa")]
    [SerializeField] GameObject canvasPausa;
    [SerializeField] GameObject opciones;

    [SerializeField] List<GameObject> tiposOpciones; //0 sonido 1 graficos 2 controles

    [Header("Opciones: Sonido")]
    [SerializeField] AudioMixer audioMixer;

    [SerializeField] Slider sliderMusica;
    [SerializeField] Slider sliderEfectos;

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
        cinemachineSwitcher = GetComponent<CinemachineSwitcher>();
    }

    void Update()
    {
        if(zonaActual == Zona.Menu)
        {
            containerCorazones.SetActive(false);
            playerMov.canMove = false;
        }

        else
        {
            containerCorazones.SetActive(true);

            //Inventario

            if (mano.transform.childCount != 0)
            {
                objetoEnMano = mano.transform.GetChild(0).gameObject;
                tieneObjetoEnMano = true;
            }

            else tieneObjetoEnMano = false;

            if (Input.GetKeyDown(abrirInventario) && !inventario.activeInHierarchy)
            {
                AbrirInventario();
            }
            else if (Input.GetKeyDown(abrirInventario) && inventario.activeInHierarchy)
            {
                CerrarInventario();
            }

            //Mapa

            if (Input.GetKeyDown(abrirMapa) && !cameraMapa.activeInHierarchy)
            {
                AbrirMapa();
            }
            else if (Input.GetKeyDown(abrirMapa) && cameraMapa.activeInHierarchy)
            {
                CerrarMapa();
            }

            //Pausa

            if (Input.GetKeyDown(pausar) && !canvasPausa.activeInHierarchy)
            {
                Debug.Log("pausa");
                PausarJuego();
            }
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
        Cursor.lockState = CursorLockMode.Locked;
        CambiarCancion();
    }

    public void AbrirInventario()
    {
        
        inventario.SetActive(true);
        cinemachineSwitcher.FijarCamara();
        Cursor.lockState = CursorLockMode.None;
        playerMov.canMove = false;
        if (onInventoryOpenedEvent != null) onInventoryOpenedEvent();
    }

    public void CerrarInventario()
    {
        inventario.SetActive(false);
        cinemachineSwitcher.ReiniciarMovimientoCamara();
        Cursor.lockState = CursorLockMode.Locked;
        playerMov.canMove = true;
    }

    void AbrirMapa()
    {
        cameraMapa.SetActive(true);
        playerMov.canMove = false;
    }

    void CerrarMapa()
    {
        cameraMapa.SetActive(false);
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

    //---MENU: OPCIONES----


    //----PAUSA----
    //Funciones para los botones del men� de pausa.

    void PausarJuego()
    {
        cinemachineSwitcher.FijarCamara();
        playerMov.canMove = false;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        canvasPausa.SetActive(true);
    }
    public void Reanudar()
    {
        cinemachineSwitcher.ReiniciarMovimientoCamara();
        playerMov.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        canvasPausa.SetActive(false);
    }

    public void Opciones()
    {
        canvasPausa.transform.GetChild(0).gameObject.SetActive(false);
        opciones.SetActive(true);
    }

    public void Salir()
    {
        Application.Quit();
    }

    public void MenuPrincipal()
    {
        cinemachineSwitcher.SwitchPriority("ThirdPerson");

        zonaActual = Zona.Menu;
        menuInicial.SetActive(true);
        canvasPausa.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        CambiarCancion();   
    }


    //----PAUSA: OPCIONES----
    //Funciones para las opciones del juego

    public void Sonido()
    {
        tiposOpciones[0].SetActive(true);
        tiposOpciones[1].SetActive(false);
        tiposOpciones[2].SetActive(false);
    }

    public void VolumenMusica()
    {
        if(sliderMusica.value == 0) audioMixer.SetFloat("musicaVolume", -80);
        else audioMixer.SetFloat("musicaVolume", 20f * Mathf.Log10(sliderMusica.value));
    }

    public void VolumenEfectos()
    {
        if (sliderEfectos.value == 0) audioMixer.SetFloat("efectosVolume", -80);
        else audioMixer.SetFloat("efectosVolume", 20f * Mathf.Log10(sliderEfectos.value));
    }

    public void Graficos()
    {
        tiposOpciones[0].SetActive(false);
        tiposOpciones[1].SetActive(true);
        tiposOpciones[2].SetActive(false);
    }

    public void Controles()
    {
        tiposOpciones[0].SetActive(false);
        tiposOpciones[1].SetActive(false);
        tiposOpciones[2].SetActive(true);
    }

    public void VolverMenuPausa()
    {
        opciones.SetActive(false);
        canvasPausa.transform.GetChild(0).gameObject.SetActive(true);
    }


}
