using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineSwitcher : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] private CinemachineVirtualCamera vcam1; //Menu
    [SerializeField] private CinemachineFreeLook vcam2; //3 persona


    public void SwitchPriority(string state)
    {
        if (state == "Menu")
        {
            //Menu -> 3 persona
            vcam1.Priority = 0;
            vcam2.Priority = 1;
            gameManager.CerrarMenu();
        }

        else if (state == "ThirdPerson")
        {
            //3 persona -> Menu
            vcam1.Priority = 1;
            vcam2.Priority = 0;
        }
    }
}
