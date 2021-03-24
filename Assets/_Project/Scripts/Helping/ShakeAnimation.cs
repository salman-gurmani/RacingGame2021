using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeAnimation : MonoBehaviour{

    public float rotateLimit = 10;
    public float delayInTransition = 0.05f;
    public float animationDuration = 0.6f;
    public float scaleVal = 0.8f;

    private bool isRight = false;
    private Vector3 initialScale;
    private float t = 1;
    private float time = 0;

    private void OnEnable() {

        initialScale = this.transform.localScale;
        this.transform.localScale = new Vector3( scaleVal, scaleVal, scaleVal );

        t = delayInTransition;
        time = animationDuration;

    }

    void Update() {

        time -= Time.deltaTime; // main time

        if ( time > 0 ) {
        
            t -= Time.deltaTime; // animation step itme
        
            if (t <= 0) {
            
                if(isRight){

                    this.transform.rotation = Quaternion.Euler(new Vector3(0,0,rotateLimit));
                    isRight = false;
                }
                else {
                
                    this.transform.rotation = Quaternion.Euler(new Vector3(0,0,-rotateLimit));
                    isRight = true;
                }

                t = delayInTransition;
            }
        }
        else {

            this.transform.localScale = initialScale;
            
            this.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
            this.enabled = false;
        }



    }
}
