using UnityEngine;
using System.Collections;

public class PhoneArm : MonoBehaviour {

	Animator m_Animator;

	// Use this for initialization
	void Start () {
		m_Animator = GetComponent<Animator>();
		m_Animator.enabled = false;
		Transform result = gameObject.transform;
		print (result);
		for (int i = 0; i < 7; i++) {
			if(i == 3 || i == 6) {
				result = getChild (result, 2);
			} else {
				result = getChild (result, 0);
			}
		}
		result.Rotate(new Vector3(90, 0, 0));
		
		Transform result2 = result;
		for (int i = 0; i < 2; i++) {
			result2 = getChild (result2, 0);
		}
		result2.Rotate (new Vector3 (180, 0, 20));
		print (result2);
	}
	
	// Update is called once per frame
	void Update () {

	}

	Transform getChild(Transform source, int i) {
		return source.transform.GetChild (i);
	}
}
