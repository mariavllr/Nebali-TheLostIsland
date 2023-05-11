using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncontrarAldeanos : MonoBehaviour
{
    public GameManager gameManager;
    private ManagerDialogos managerDialogos;

    [Header("Mision buscar aldeanos")]
    private int numAldeanos = 3;
    private int numAldeanosConocidos = 2;
    private List<string> aldeanosConocidos;

    void Start()
    {
        aldeanosConocidos = new List<string>();

        managerDialogos = gameManager.gameObject.GetComponent<ManagerDialogos>();
        aldeanosConocidos.Add("Brivia");
        aldeanosConocidos.Add("Edbri");
        ManagerDialogos.onDialogueEvent += ComprobarAldeanoConocido;
    }

    void Update()
    {
        if (gameManager.misionActual == GameManager.Mision.ConocerAldeanos)
        {
            //Mision completada
            if (numAldeanosConocidos == numAldeanos)
            {
                Debug.Log("Mision aldeanos completada");
                gameManager.misionActual = GameManager.Mision.Ninguna;
            }

        }
    }

    void ComprobarAldeanoConocido()
    {
        if(gameManager.misionActual == GameManager.Mision.ConocerAldeanos)
        {
            string aldeano = managerDialogos.personaje;
            Debug.Log("---ComprobarAldeano---");
            if (!aldeanosConocidos.Contains(aldeano))
            {
                //El aldeano no lo conocia ya
                numAldeanosConocidos++;
                Debug.Log("Nuevo aldeano!");
                Debug.Log("Aldeanos conocidos: " + numAldeanosConocidos + ", Aldeanos totales: " + numAldeanos);
            }
            else Debug.Log("Ya lo conocias");
        }
    }
}
