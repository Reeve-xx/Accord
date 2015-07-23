using UnityEngine;
using System.Collections;

public class SunCycle : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.Rotate(new Vector3(-0.01f * Time.deltaTime, 0, 0));
	}
}
