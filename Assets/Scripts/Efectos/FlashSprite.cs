using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashSprite : MonoBehaviour
{
    private Image image;
    void Start()
    {
        image = GetComponent<Image>();
        InvokeRepeating("FlashImage", 0, 0.5f);
    }

    void Update()
    {
        
    }

    private void FlashImage()
    {
        if (image.enabled) image.enabled = false;
        else image.enabled = true;
    }
}
