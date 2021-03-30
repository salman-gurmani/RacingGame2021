using UnityEngine;

[RequireComponent(typeof(GameManager))]
[RequireComponent(typeof(SoundManager))]
[RequireComponent(typeof(DB))]

public class Toolbox : MonoBehaviour {
    private static GameManager gameManager;
    private static SoundManager soundManager;
    private static DB db;
    //private static InAppHandler inAppHandler;

    private static MenuHandler menuHandler;
    private static GameplayScript gameplayScript;
    private static HUDListner hudListner;

    public static GameManager GameManager {
        get { return gameManager; }
    }

    public static SoundManager Soundmanager {
        get { return soundManager; }
    }
    
    public static DB DB
    {
        get { return db; }
    }

    //public static InAppHandler InAppHandler
    //{
    //    get { return inAppHandler; }
    //}
    public static MenuHandler MenuHandler
    {
        get { return menuHandler; }
    }

    public static GameplayScript GameplayScript {
        get { return gameplayScript; }
    }

    public static HUDListner HUDListner {
        get { return hudListner; }
    }

    void Awake()
    {
        gameManager = GetComponent<GameManager>();
        soundManager = GetComponent<SoundManager>();
        db = GetComponent<DB>();
       // inAppHandler = GetComponent<InAppHandler>();

        DontDestroyOnLoad(gameObject);
    }

    public static void Set_MenuHandler(MenuHandler _menuHandler)
    {
        menuHandler = _menuHandler;
    }

    public static void Set_GameplayScript (GameplayScript _game) {
        gameplayScript = _game;
    }

    public static void Set_HudListner(HUDListner _hud)
    {
        hudListner = _hud;
    }
}