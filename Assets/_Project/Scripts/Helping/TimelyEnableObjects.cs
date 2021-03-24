using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelyEnableObjects : MonoBehaviour
{
    public enum State { 
    
        ONETIME,
        PINGPONG,
        LOOP_ON_OFF,
        LOOP_CHANGECOLOR
    };

    public State state;


    public GameObject[] objects;
    public float timeToNextStep;

    public Color[] myColors;
    int colorCur = 0;

    
    int cursor;

    float time;
    int pingpongCount = 1;



    private void Start()
    {
        time = timeToNextStep;
    }

    void Update()
    {
        time -= Time.deltaTime;
        
        if (time <= 0) {
            
            switch (state)
            {
                case State.ONETIME:

                    if (cursor >= objects.Length)
                    {
                        break;
                    }

                    objects[cursor].SetActive(!objects[cursor].activeSelf); //toggle the active state of that object
                    cursor++;

                    break; 

                case State.PINGPONG:

                    if (cursor >= objects.Length)
                    {
                        cursor = 0;
                    }
                    
                    if (pingpongCount % 2 == 1) {
                        objects[cursor].SetActive(!objects[cursor].activeSelf); //toggle the active state of that object
                    }

                    if (pingpongCount % 2 == 0)
                    {
                        objects[cursor].SetActive(!objects[cursor].activeSelf); //toggle the active state of that object
                        cursor++;
                    }

                    pingpongCount++;

                    break;

                case State.LOOP_ON_OFF:

                    if (cursor >= objects.Length) {
                        cursor = 0;
                        
                        colorCur++;                        
                        if (colorCur >= myColors.Length) {
                            colorCur = 0;
                        }
                    }

                    if(objects[cursor].GetComponent<SpriteRenderer>())
                        objects[cursor].GetComponent<SpriteRenderer>().color = myColors[colorCur];

                    objects[cursor].SetActive(!objects[cursor].activeSelf); //toggle the active state of that object
                    cursor++;

                    break;

                case State.LOOP_CHANGECOLOR:

                    if (cursor >= objects.Length)
                    {
                        cursor = 0;

                        colorCur++;
                        if (colorCur >= myColors.Length)
                        {
                            colorCur = 0;
                        }
                    }

                    objects[cursor].GetComponent<SpriteRenderer>().color = myColors[colorCur];
                    objects[cursor].SetActive(true); //toggle the active state of that object

                    cursor++;

                    break;
            }

            time = timeToNextStep;
        }

    }
}
