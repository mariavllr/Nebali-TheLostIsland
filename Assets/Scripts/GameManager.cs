using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player;

    [Header("Controles")]
    public KeyCode hablar;
    public KeyCode atacar;
    public KeyCode abrirInventario;

    public Nivel nivelActual;

    public enum Nivel
    {
        Bosque,
        Isla,
        Granja
    }

    void Start()
    {
        nivelActual = Nivel.Bosque;
    }

    void Update()
    {
        
    }
}
