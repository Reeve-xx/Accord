using UnityEngine;
using System.Collections;
using Accord;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.ThirdPerson;

public class CharacterDroneSwitcher : MonoBehaviour {

	GameObject drone;
	bool droneEnabled = false;
	bool viewingDrone = false;
	float viewPosition = 0;

	void Start () {
		drone = GameObject.Find ("Drone");
		closeTabletView ();
	}

	Camera getDroneCam() {
		Transform child = drone.transform.GetChild (5);
		return child.GetComponent<Camera>();
	}

	Camera getDroneCam2() {
		Transform child = drone.transform.GetChild (6);
		return child.GetComponent<Camera> ();
	}
	
	void Update () {
		if(CrossPlatformInputManager.GetButtonDown ("Fire2")) {
			if(!droneEnabled) {
				openTabletView ();
			} else {
				closeTabletView ();
			}
		}
	}

	void openTabletView() {
		droneEnabled = true;
		getDroneCam2 ().enabled = true;
		transform.GetChild (0).GetComponent<Camera> ().fieldOfView = 25;
		GetComponent<ThirdPersonUserControl> ().enabled = false;
		drone.GetComponent<BlenderDroneController> ().inputEnabled = true;
		transform.GetChild (0).GetComponent<Camera> ().enabled = true;
		transform.GetChild (1).GetComponent<Camera> ().enabled = false;
		//GetComponent<Animator> ().enabled = false;
	}

	void closeTabletView() {
		droneEnabled = false;
		getDroneCam2 ().enabled = false;
		GetComponent<ThirdPersonUserControl> ().enabled = true;
		drone.GetComponent<BlenderDroneController> ().inputEnabled = false;
		transform.GetChild (0).GetComponent<Camera> ().enabled = false;
		transform.GetChild (1).GetComponent<Camera> ().enabled = true;
		//GetComponent<Animator> ().enabled = true;
	}
	
	/**void disableDrone() {
		getDroneCam ().enabled = false;
		getDroneCam2 ().enabled = false;
		getDroneCam2 ().GetComponent<AudioListener> ().enabled = false;
		transform.GetChild (0).GetComponent<AudioListener> ().enabled = true;
		drone.GetComponent<BlenderDroneController> ().inputEnabled = false;
		transform.GetChild (0).GetComponent<Camera> ().fieldOfView = 60;
		transform.GetChild (0).GetComponent<Camera> ().transform.localPosition += transform.GetChild (0).GetComponent<Camera> ().transform.forward * -5;
		//transform.GetChild(0).GetComponent<Camera> ().enabled = true;
		GetComponent<ThirdPersonUserControl> ().enabled = true;
		droneEnabled = false;
		//viewDrone ();
		//print ("Disabling drone");
	}

	void enableDrone() {
		//stopViewDrone ();
		//print ("Enabling drone");
		getDroneCam2 ().enabled = true;
		getDroneCam2 ().GetComponent<AudioListener> ().enabled = true;
		transform.GetChild (0).GetComponent<AudioListener> ().enabled = false;
		drone.GetComponent<BlenderDroneController> ().inputEnabled = true;
		GetComponent<ThirdPersonUserControl> ().enabled = false;
		GetComponent<ThirdPersonCharacter> ().Move (new Vector3 (0, 0, 0), false, false);
		//transform.GetChild(0).GetComponent<Camera> ().enabled = false;
		transform.GetChild (0).GetComponent<Camera> ().fieldOfView = 25;
		transform.GetChild (0).GetComponent<Camera> ().transform.localPosition += transform.GetChild (0).GetComponent<Camera> ().transform.forward * 5;
		droneEnabled = true;
	}

	void slowViewDrone() {
		//print ("Viewing drone");
		viewingDrone = true;
		Rect rect = getDroneCam2 ().rect;
		rect.x = 0.75f;
		rect.y = 0;
		rect.width = 0.2f;
		rect.height = 0;
		getDroneCam2 ().rect = rect;
		getDroneCam2 ().enabled = true;
	}


	void slowViewDrone() {
		viewDrone ();
	}

	void viewDrone() {
		//print ("Viewing drone");
		viewPosition = 250;
		viewingDrone = true;
		Rect rect = getDroneCam2 ().rect;
		rect.x = 0.75f;
		rect.y = 0.05f;
		rect.width = 0.2f;
		rect.height = 0.2f;
		getDroneCam2 ().rect = rect;
		getDroneCam2 ().enabled = true;
	}

	void viewDrone() {
		viewingDrone = true;
		getDroneCam2 ().enabled = true;
		enableDrone ();
	}

	void setViewY() {
		Rect rect = getDroneCam2 ().rect;
		if (viewPosition > 50) {
			rect.height = 0.001f * (viewPosition - 50);
			rect.width = 0.001f * (viewPosition - 50);
			rect.x = 0.75f + (0.2f - rect.width) / 2;
		} else {
			rect.height = 0;
			rect.width = 0.001f * (viewPosition - 50);
			rect.x = 0.75f + (0.2f - rect.width) / 2;
		}
		if (viewPosition > 200) {
			rect.y = 0.001f * (viewPosition - 200);
		} else {
			rect.y = 0;
		}
		getDroneCam2 ().rect = rect;
		//print ("View position: " + viewPosition + ", Height: " + rect.height + ", y: " + rect.y);
	}

	void stopViewDrone() {
		viewingDrone = false;
		getDroneCam2 ().enabled = false;
		stopViewDrone ();
	}

	void stopViewDrone() {
		//print ("Stopping viewing drone");
		viewPosition = 0;
		viewingDrone = false;
		Rect rect = getDroneCam2 ().rect;
		rect.x = 0f;
		rect.y = 0f;
		rect.width = 1f;
		rect.height = 1f;
		getDroneCam2 ().rect = rect;
		getDroneCam2 ().enabled = false;
	}

	void slowStopViewDrone() {
		//print ("Stopping viewing drone slowly");
		viewingDrone = false;
	}

	void slowStopViewDrone() {
		viewingDrone = false;
		stopViewDrone ();
	}*/
}
