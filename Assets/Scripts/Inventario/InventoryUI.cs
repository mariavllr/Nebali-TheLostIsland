using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] InventorySystem inventorySystem;
    [SerializeField] GameObject container;

    [Header("El lugar donde tiene que colocarse el objeto en el jugador")]
    [SerializeField] GameObject playerObjectContainer; 

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

            UIInventoryItemSlot slot = obj.GetComponent<UIInventoryItemSlot>();
            slot.Set(item);

            obj.GetComponentInChildren<Button>().onClick.AddListener(delegate { GivePlayerObjectSelected(item.data.prefab); });
        }       
    }

    void GivePlayerObjectSelected(GameObject objeto)
    {
        //Ver qué objeto (GameObject) es el que ha hecho clic
        Debug.Log(objeto.name);

        //Comprobar si el jugador tenía un objeto en la mano y quitarlo de ser así
        if(playerObjectContainer.transform.childCount != 0)
        {
            Destroy(playerObjectContainer.transform.GetChild(0).gameObject);       
        }
        //Poner objeto en la mano del jugador
        GameObject selectedObject = Instantiate(objeto, playerObjectContainer.transform);
        DestroyImmediate(selectedObject.GetComponent<ItemObject>());
        //selectedObject.transform.localRotation = Quaternion.identity;
        selectedObject.transform.localPosition = Vector3.zero;

        //Cerrar inventario
        gameManager.CerrarInventario();
    }
}
