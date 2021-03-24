using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIImageAnimation : MonoBehaviour
{

    Image imgComponent;
    public Sprite[] img;

    int curImg;

    float curtime;
    public float delay = 0.1f;

    private void Start()
    {
        imgComponent = this.GetComponent<Image>();

        curImg = 0;
    }

    private void Update()
    {
        curtime -= Time.deltaTime;

        if (curtime <= 0) {

            curImg++;
            if (curImg >= img.Length) {
                curImg = 0;
            }

            imgComponent.sprite = img[curImg];
            imgComponent.SetNativeSize();
            curtime = delay;
        }
    }

}
