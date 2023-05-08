using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject itemPrefab;
    [SerializeField] InventorySystem inventorySystem;
    [SerializeField] GameObject container;
    [SerializeField] GameObject selectedItemIcon;
    public int playerSelectedSlot;

    private int slotIndex;
    private GameObject selectedObject;
    void Start()
    {       
        GameManager.onInventoryOpenedEvent += OnInventoryOpened;
        playerSelectedSlot = 1;
        slotIndex = 0;
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
            slotIndex++;
            GameObject obj = Instantiate(itemPrefab);
            obj.transform.SetParent(container.transform, false);
            Debug.Log("Se ha añadido al inventario un objeto");

            UIInventoryItemSlot slot = obj.GetComponent<UIInventoryItemSlot>();
            slot.Set(item, slotIndex);

            //Item seleccionado
            if(slotIndex == playerSelectedSlot)
            {
                GameObject selector = Instantiate(selectedItemIcon, obj.transform, false);
                selectedObject = item.data.prefab;
            }
        }       
    }

    private void Update()
    {
        //Escoger item
        if (Input.GetKey(KeyCode.RightArrow))
        {
            playerSelectedSlot++;
            selectedObject = inventorySystem.inventory[playerSelectedSlot].data.prefab;
        }
    }
}
