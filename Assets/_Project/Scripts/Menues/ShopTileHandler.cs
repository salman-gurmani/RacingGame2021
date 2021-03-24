using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopTileHandler : MonoBehaviour
{
    public Button buyBtn;
    public Image productImg;
    
    public Color defaultColor;
    public Color purchasedColor;
    public Color selectedColor;

    ShopListner handler;
    int itemNum = 0;

}
