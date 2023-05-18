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

    public void Set(InventoryItem item)
    {
        imagen.sprite = item.data.icon;
        nombre.text = item.data.displayName;
        if(item.stackSize < 1)
        {
            itemObj.SetActive(false);
            return;

        }
        stackNum.text = item.stackSize.ToString();

    }
}
