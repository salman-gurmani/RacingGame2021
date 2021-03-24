using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class LeaderboardPlayer
{

    public string name;
    public string score;

    public LeaderboardPlayer(string _name, string _score)
    {
        name = _name;
        score = _score;
    }
}

[System.Serializable]
public class MainMenuButton{

    public string titleName;
    public int activityNumber;
    public Sprite img;
    public UnityEvent onClick;

}

[System.Serializable]
public class SimpleIntroductionPage
{
    public AudioClip titleSound;
    public AudioClip mainImgPressSound;

    public Sprite bgSprite;
    public Color mainImgColor = Color.white;
}

public class SerializeClassContainer : MonoBehaviour
{
    //this is dummy class only to hold serialize classes
}
