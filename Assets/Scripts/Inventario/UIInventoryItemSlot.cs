using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInventoryItemSlot : MonoBehaviour
{
    [SerializeField] Image imagen;
    [SerializeField] TextMeshProUGUI nombre;
    [SerializeField] GameObject itemObj;
    [SerializeField] TextMeshProUGUI stackNum;
    public int slotNumber;

    public void Set(InventoryItem item, int slot)
    {
        imagen.sprite = item.data.icon;
        nombre.text = item.data.displayName;
        if(item.stackSize < 1)
        {
            itemObj.SetActive(false);
            return;

        }
        stackNum.text = item.stackSize.ToString();

        slotNumber = slot;
    }
}
