using UnityEngine;
using System.Collections;

public class Conversation : MonoBehaviour {

	public int width = 10000;
	public int height = 10000;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnGUI() {
		GUI.Label (new Rect (Screen.width / 2 - width / 2, Screen.height * 0.9f - height, Screen.width, Screen.height), "Gryph: What the hell, man?");
	}
}
