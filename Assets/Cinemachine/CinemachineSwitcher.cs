using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineSwitcher : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] private CinemachineVirtualCamera vcamMenu; //Menu
    [SerializeField] private CinemachineFreeLook vcam3persona; //3 persona
    [SerializeField] private CinemachineVirtualCamera vcamGemaBosque;
    [SerializeField] private CinemachineVirtualCamera vcamGemaPueblo;
    [SerializeField] private CinemachineVirtualCamera vcamGemaGranja;
    [SerializeField] private CinemachineVirtualCamera vcamFinal;

    //Se le llama en el Menu, cuando se pulsa el boton Comenzar
    public void SwitchPriority(string state)
    {
        if (state == "Menu")
        {
            //Menu -> 3 persona
            vcamMenu.Priority = 0;
            vcam3persona.Priority = 1;
            gameManager.CerrarMenu();
        }

        else if (state == "ThirdPerson")
        {
            Debug.Log("Cambio a camara menu");
            //3 persona -> Menu
            vcamMenu.Priority = 1;
            vcam3persona.Priority = 0;
        }

        else if (state == "VerGemaBosque")
        {
            StartCoroutine(CamaraGemas(vcamGemaBosque, 4));
        }

        else if (state == "VerGemaPueblo")
        {
            StartCoroutine(CamaraGemas(vcamGemaPueblo, 4));
        }

        else if (state == "VerGemaGranja")
        {
            StartCoroutine(CamaraGemas(vcamGemaGranja, 4));
        }

        else if (state == "VerFinal")
        {
            StartCoroutine(CamaraGemas(vcamFinal, 4));
        }
    }

    IEnumerator CamaraGemas(CinemachineVirtualCamera camaraGema, int segundos)
    {
        camaraGema.Priority = 2;
        yield return new WaitForSeconds(segundos);
        camaraGema.Priority = 0;
    }

    public void FijarCamara()
    {
        vcam3persona.m_YAxis.m_MaxSpeed = 0;
        vcam3persona.m_XAxis.m_MaxSpeed = 0;
    }

    public void ReiniciarMovimientoCamara()
    {
        vcam3persona.m_YAxis.m_MaxSpeed = 1;
        vcam3persona.m_XAxis.m_MaxSpeed = 100;
    }
}
