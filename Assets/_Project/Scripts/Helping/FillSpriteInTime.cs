using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FillSpriteInTime : MonoBehaviour
{

    Image img;
    public float fillSpeed = 0.1f;

    private void Start()
    {
        img = this.GetComponent<Image>();
    }

    private void Update()
    {
        if (img.fillAmount < 1) {
            img.fillAmount = img.fillAmount + fillSpeed;
        }
        
    }
}
