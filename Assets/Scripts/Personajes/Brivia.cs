using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Subtegral.DialogueSystem.DataContainers;
using Subtegral.DialogueSystem.Runtime;
using UnityEngine.AI;

public class Brivia : MonoBehaviour
{
    [SerializeField] GameObject gameManagerObject;

    private GameManager gameManager;
    private ManagerDialogos managerDialogos;
    private ActivarDialogo dialogoInfo;
    private NavMeshAgent navmesh;
    public GameObject espada;

    public List<DialogueContainer> dialogos;
    public DialogueContainer miProximoDialogo;

    [Space]
    [Header("Camino hasta el pueblo")]
    //Camino hasta pueblo
    public List<Transform> waypointsCaminoInicial;
    private bool caminoAPueblo;
    private bool _enMovimiento;
    private bool _fin;
    private int _indice;
    private Transform currentTarget;

    void Start()
    {
        ManagerDialogos.onDialogueEvent += EventoDialogoBrivia;

        gameManager = gameManagerObject.GetComponent<GameManager>();
        managerDialogos = gameManagerObject.GetComponent<ManagerDialogos>();
        navmesh = GetComponent<NavMeshAgent>();
        dialogoInfo = GetComponent<ActivarDialogo>();

        caminoAPueblo = false;
        dialogoInfo.dialogoDisponible = true;
        miProximoDialogo = dialogos[0];
    }

    void Update()
    {
        if (currentTarget != null && caminoAPueblo)
        {
            //Check si se ha llegado a la posicion
            if ((Vector3.Distance(transform.position, currentTarget.position) <= 2f) && _enMovimiento)
            {
                _enMovimiento = false;
                if (_indice < waypointsCaminoInicial.Count - 1) MoverSiguienteWaypoint();
                else
                {
                    caminoAPueblo = false;
                    dialogoInfo.dialogoDisponible = true;
                }
            }
        }
         
    }

    void EventoDialogoBrivia()
    {
        //Se ha terminado un dialogo con Brivia
        if (managerDialogos.personaje == "Brivia")
        {
            if (managerDialogos.dialogoActual.ExposedProperties[0].PropertyName == "DialogueName")
            {
                switch (managerDialogos.dialogoActual.ExposedProperties[0].PropertyValue)
                {
                    case "Inicio":
                        Debug.Log("Inicio de brivia acabado");
                        miProximoDialogo = dialogos[1];

                        caminoAPueblo = true;
                        dialogoInfo.dialogoDisponible = false;
                        CaminarHastaPueblo();
                        break;
                    case "Brivia2":
                        Debug.Log("acaba brivia");

                        if (miProximoDialogo == dialogos[1])
                            Instantiate(espada, transform.position + new Vector3(3f, 0, 0), Quaternion.Euler(new Vector3(-90, -90, 0)));
                        //Este dialogo es si vuelves a hablar con ella en algun momento
                        miProximoDialogo = dialogos[2];
                        break;
                }

            }
        }
    }

    bool JugadorCerca()
    {
        if (Vector3.Distance(transform.position, gameManager.player.transform.position) <= 2)
        {
            return true;
        }
        else return false;
    }

    void CaminarHastaPueblo()
    {
        if (waypointsCaminoInicial.Count != 0 && waypointsCaminoInicial[0] != null)
        {
            //Si el primer punto es válido, moverse hacia ese punto
            currentTarget = waypointsCaminoInicial[0];
            navmesh.destination = currentTarget.position;
            _enMovimiento = true;
        }
    }

    void MoverSiguienteWaypoint()
    {
        _indice++;
        //Si no esta en el ultimo
        if (_indice < waypointsCaminoInicial.Count)
        {
            currentTarget = waypointsCaminoInicial[_indice];
        }
        else
        {
            if (!_fin)
            {
                _fin = true;
            }

            currentTarget = waypointsCaminoInicial[_indice];
        }

        navmesh.destination = currentTarget.position;
        _enMovimiento = true;
    }

}

            
