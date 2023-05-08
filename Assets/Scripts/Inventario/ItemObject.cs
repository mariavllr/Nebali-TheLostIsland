using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    GameManager gameManager;
    private InventorySystem inventory;
    public InventoryItemData referenceItem;
    public void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>() ;
        inventory = gameManager.GetComponent<InventorySystem>();
    }
    public void OnHandlePickupItem()
    {
        Debug.Log("Se ejecuta OnHandlePickupItem");
        inventory.Add(referenceItem);

        Destroy(gameObject);
        Debug.Log("Se destruye el objeto");

        gameManager.MostrarMensaje(referenceItem.displayName + " se ha añadido al inventario.");

    }

}
