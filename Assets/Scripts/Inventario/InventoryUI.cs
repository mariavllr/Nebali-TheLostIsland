using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] InventorySystem inventorySystem;
    [SerializeField] GameObject container;
    [SerializeField] TextMeshProUGUI descripcion;
    [SerializeField] Image foto;

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

            obj.GetComponent<Button>().onClick.AddListener(delegate { GivePlayerObjectSelected(item); });
        }       
    }

    void GivePlayerObjectSelected(InventoryItem item)
    {
        GameObject objeto = item.data.prefab;

        //Mostrar descripcion y foto
        descripcion.text = item.data.description;
        foto.enabled = true;
        foto.sprite = item.data.icon;


        //Comprobar si el jugador tenía un objeto en la mano (que no es el mismo) y quitarlo de ser así
        if(gameManager.tieneObjetoEnMano && gameManager.objetoEnMano != objeto)
        {
            Destroy(gameManager.objetoEnMano);
            gameManager.tieneObjetoEnMano = false;
        }
        //Poner objeto en la mano del jugador
        GameObject selectedObject = Instantiate(objeto, gameManager.mano.transform);
        DestroyImmediate(selectedObject.GetComponent<ItemObject>());
        //selectedObject.transform.localRotation = Quaternion.identity;
        selectedObject.transform.localPosition = Vector3.zero;
    }
}
