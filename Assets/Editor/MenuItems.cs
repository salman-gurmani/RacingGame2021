using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuItems {
	public static string scenePath = "Assets/-Scenes/Intro.unity";

	[MenuItem("Idee/Play Intro #Z")]
    [System.Obsolete]
    private static void NewMenuOption1(){
		
		EditorApplication.SaveCurrentSceneIfUserWantsTo(); 
		EditorApplication.OpenScene(scenePath);
		EditorApplication.isPlaying = true;
	}

	[MenuItem("Idee/Clear PlayerPrefs")]
	private static void NewMenuOption2(){
	
		PlayerPrefs.DeleteAll ();
	}
}
