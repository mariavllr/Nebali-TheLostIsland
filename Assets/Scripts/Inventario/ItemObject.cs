using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    GameManager gameManager;
    private InventorySystem inventory;
    public InventoryItemData referenceItem;

    public delegate void OnGemasEvent();
    public static event OnGemasEvent onGemasEvent;

    public void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>() ;
        inventory = gameManager.GetComponent<InventorySystem>();
    }
    public void OnHandlePickupItem()
    {
        inventory.Add(referenceItem);
        if(!gameManager.tieneGemas && (gameObject.tag == "GemaAmarilla" || gameObject.tag == "GemaRoja" || gameObject.tag == "GemaVerde"))
        {
            gameManager.tieneGemas = true;           
            if (onGemasEvent != null) onGemasEvent();
            gameManager.MostrarMensaje("¡Has conseguido una gema! Ve a hablar con Brivia");
        }
        else gameManager.MostrarMensaje(referenceItem.displayName + " se ha añadido al inventario.");
        Destroy(gameObject);     

    }

}
