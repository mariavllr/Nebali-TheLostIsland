using Subtegral.DialogueSystem.DataContainers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Nebali : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMov;
    [SerializeField] ManagerDialogos managerDialogos;
    private ActivarDialogo dialogoInfo;
    public DialogueContainer dialogoFinal;
    [HideInInspector] public DialogueContainer miProximoDialogo;

    [SerializeField] Image fadingImage;
    void Start()
    {
        dialogoInfo = GetComponent<ActivarDialogo>();
        dialogoInfo.dialogoDisponible = false;
        miProximoDialogo = dialogoFinal;

        Gemas.onFinalEvent += EventoDialogoFinal;
        ManagerDialogos.onDialogueEvent += EventoDialogoNebali;
    }

    void Update()
    {
        
    }

    void EventoDialogoFinal()
    {
        //Este evento se lanza cuando se colocan las 3 gemas en la torre
        dialogoInfo.dialogoDisponible = true;
    }

    void EventoDialogoNebali()
    {
        //Este evento se lanza cuando se acaba un diálogo

        //Hacer fade out y reiniciar juego
        if(managerDialogos.personaje == "Nebali") StartCoroutine(RebootGame(fadingImage, 4));
    }

    IEnumerator RebootGame(Image img, float duration)
    {
        playerMov.canMove = false;
        float counter = 0;
        //Get current color
        Color spriteColor = img.color;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha;
            //Fade from 1 to 0 or viceversa
            alpha = Mathf.Lerp(0, 1, counter / duration);
            //Change alpha only
            img.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            //Wait for a frame
            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnDisable()
    {
        Gemas.onFinalEvent -= EventoDialogoFinal;
        ManagerDialogos.onDialogueEvent -= EventoDialogoNebali;
    }
}
