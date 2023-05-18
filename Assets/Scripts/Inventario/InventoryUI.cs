using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject itemPrefab;
    [SerializeField] InventorySystem inventorySystem;
    [SerializeField] GameObject container;

    void Start()
    {       
        GameManager.onInventoryOpenedEvent += OnInventoryOpened;
    }

    void OnInventoryOpened()
    {   
        //Limpiar inventario
        foreach(Transform t in container.transform)
        {
            Destroy(t.gameObject);
        }
        //Dibujar inventario      

        foreach (InventoryItem item in inventorySystem.inventory)
        {
            GameObject obj = Instantiate(itemPrefab);
            obj.transform.SetParent(container.transform, false);
            Debug.Log("Se ha añadido al inventario un objeto");

            obj.GetComponent<Button>().onClick.AddListener(GivePlayerObjectSelected);

            UIInventoryItemSlot slot = obj.GetComponent<UIInventoryItemSlot>();
            slot.Set(item);
        }       
    }

    void GivePlayerObjectSelected()
    {
        //Cerrar inventario

        //Ver qué objeto (GameObject) es el que ha hecho clic

        //Comprobar si el jugador tenía un objeto en la mano y ocultarlo de ser así

        //Poner objeto en la mano del jugador

    }
}
