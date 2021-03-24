using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeColor : MonoBehaviour {

//	public TextMesh example;
	public Color newCol;
	public float fadeTime;

    void Update ()
	{		
		this.GetComponent <SpriteRenderer>().color = Color.Lerp (this.GetComponent <SpriteRenderer>().color, newCol, fadeTime * Time.deltaTime);

//		example.color.a = newCol.a;
	}

}
