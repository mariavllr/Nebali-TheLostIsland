using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player;
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
    }

    public void CerrarMenu()
    {
        menuInicial.SetActive(false);
        nivelActual = Nivel.Bosque;
        playerMov.canMove = true;
        CambiarCancion();
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
