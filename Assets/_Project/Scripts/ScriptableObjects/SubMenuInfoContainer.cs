using UnityEngine;


[CreateAssetMenu(fileName = "subMenu" , menuName = ("SubMenuInfoContainer"))]
public class SubMenuInfoContainer : ScriptableObject
{
    public string mainTitle;
    public Sprite mainImg;
    public bool mainSetNative = false;
    public bool savePrefs = true;

    //public string resourceFolderPath;

    [Header("Titles")]
    public string[] btnTitle;
    [Header("Images")]
    public bool imgSetNative = false;
    public Color[] imgColor = { Color.white, Color.white, Color.white };
    public Sprite[] btnImg;
    [Header("Activities")]
    public string folderPath;
    public string [] activity;


}

