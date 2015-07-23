using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

namespace Accord {
	public class BlenderDroneController : MonoBehaviour {

		float xAngle = 0;
		float yAngle = 0;
		float zAngle = 0;
		float rotorSpinSpeed = 4;
		public bool inputEnabled = true;
		public float velocityDampers = 8;
		public float angularVelocityDampers = 5;
		public float moveSpeed = 6;
		public float stabilizers = 3;
		public float tilt = 6;
		public float minOpacity = 0.15f;
		public float cloakSpeed = 2;
		float currentOpacity;
		float timeMultiplier;
		bool enginesCut;

		// TODO: Tilt should use angular momentum instead of a manual rotation.

		void Start () {
			currentOpacity = minOpacity;
		}
		
		void Update () {
			timeMultiplier = Time.deltaTime * 70;
			enginesCut = CrossPlatformInputManager.GetButton ("Fire3");
			Rigidbody rigidbody = gameObject.GetComponent<Rigidbody> ();
			
			float xInput = CrossPlatformInputManager.GetAxis ("Horizontal");
			float zInput = CrossPlatformInputManager.GetAxis ("Vertical");
			bool upInput = CrossPlatformInputManager.GetButton ("Jump");
			float xRot = CrossPlatformInputManager.GetAxis("Mouse X");
			bool cameraInput = CrossPlatformInputManager.GetButtonDown ("Fire2");

			if (!inputEnabled) {
				xInput = 0;
				zInput = 0;
				upInput = false;
				enginesCut = false;
				xRot = 0;
				cameraInput = false;
			}

			setOpacity (rigidbody);
			
			if(cameraInput) {
				//changeCameras();
			}
			
			if (enginesCut) {
				cutEngines (rigidbody);
			} else {
				startEngines (rigidbody);
				setRotorSpeeds (xInput, zInput, xRot, upInput);
				applyPhysics(rigidbody, xInput, zInput, xRot, upInput);
			}
		}


		// Cloaking

		void setOpacity(Rigidbody rigidbody) {
			if (moving (rigidbody)) {
				reduceOpacity();
			} else {
				increaseOpacity();
			}
			applyOpacity ();
		}
		
		bool moving(Rigidbody rigidbody) {
			return (Mathf.Abs (rigidbody.velocity.x) < 4f && Mathf.Abs (rigidbody.velocity.y) < 6f && Mathf.Abs (rigidbody.velocity.z) < 4f) || enginesCut;
		}
		
		void reduceOpacity() {
			currentOpacity -= 0.02f;
			if (enginesCut || !inputEnabled) {
				setCurrentOpacityMin (minOpacity / 4);
			} else {
				setCurrentOpacityMin (minOpacity / 2);
			}
		}
		
		void increaseOpacity() {
			currentOpacity += 0.02f;
			setCurrentOpacityMax(minOpacity * 2);
		}

		void setCurrentOpacityMax(float max) {
			if (currentOpacity > max) {
				currentOpacity = max;
			}
		}
		
		void setCurrentOpacityMin(float min) {
			if (currentOpacity < min) {
				currentOpacity = min;
			}
		}

		void applyOpacity() {
			for(int i = 0; i <= 4; i++) {
				reduceAlpha(gameObject.transform.GetChild (i).GetComponent<Renderer> ());
			}
		}

		void reduceAlpha(Renderer renderer) {
			Color color = renderer.material.color;
			if (color.a < currentOpacity) {
				color = modifyAlpha(color, 0.005f * cloakSpeed);
			}
			if (color.a > currentOpacity) {
				color = modifyAlpha(color, -0.005f * cloakSpeed);
			}
			if (color.a > currentOpacity && enginesCut) {
				color = modifyAlpha(color, -0.005f * cloakSpeed);
			}
			renderer.material.color = color;
		}

		Color modifyAlpha(Color color, float amount) {
			color.a += amount * timeMultiplier;
			if(color.a > currentOpacity) {
				color.a = currentOpacity;
			}
			if(color.a < currentOpacity) {
				color.a = currentOpacity;
			}
			return color;
		}


		// Cameras

		void changeCameras() {
			Transform camera = gameObject.transform.GetChild (5);
			Transform camera2 = gameObject.transform.GetChild (6);
			if (camera.GetComponent<Camera> ().enabled) {
				camera.GetComponent<Camera> ().enabled = false;
				camera2.GetComponent<Camera> ().enabled = true;
			} else {
				camera.GetComponent<Camera> ().enabled = true;
				camera2.GetComponent<Camera> ().enabled = false;
			}
		}


		// Rotors

		void cutEngines(Rigidbody rigidbody) {
			rigidbody.useGravity = true;
			spinRotors (false);
		}

		void startEngines(Rigidbody rigidbody) {
			rigidbody.useGravity = false;
			spinRotors (true);
		}

		void setRotorSpeeds(float xInput, float zInput, float xRot, bool upInput) {
			int upBonus = 0;
			if (upInput) {
				upBonus = 2;
			}
			backLeftRotor ().changeCap (rotorSpinSpeed + zInput * 2 + (xInput + xRot / 8) * 4 / 3 + upBonus);
			backRightRotor ().changeCap (rotorSpinSpeed + zInput * 2 + -(xInput + xRot / 8) * 4 / 3 + upBonus);
			frontLeftRotor ().changeCap (rotorSpinSpeed + -zInput * 2 + (xInput + xRot / 8) * 4 / 3 + upBonus);
			frontRightRotor ().changeCap (rotorSpinSpeed + -zInput * 2 + -(xInput + xRot / 8) * 4 / 3 + upBonus);
		}

		void spinRotors(bool spin) {
			backRightRotor().spinning = spin;
			backLeftRotor().spinning = spin;
			frontRightRotor().spinning = spin;
			frontLeftRotor().spinning = spin;
		}
		
		BlenderRotorSpin backRightRotor() {
			Transform rotor = gameObject.transform.GetChild(4);
			return rotor.GetComponent<BlenderRotorSpin>();
		}
		
		BlenderRotorSpin backLeftRotor() {
			Transform rotor = gameObject.transform.GetChild (3);
			return rotor.GetComponent<BlenderRotorSpin> ();
		}
		
		BlenderRotorSpin frontLeftRotor() {
			Transform rotor = gameObject.transform.GetChild(1);
			return rotor.GetComponent<BlenderRotorSpin>();
		}
		
		BlenderRotorSpin frontRightRotor() {
			Transform rotor = gameObject.transform.GetChild (2);
			return rotor.GetComponent<BlenderRotorSpin> ();
		}


		// Movement

		void applyPhysics(Rigidbody rigidbody, float xInput, float zInput, float xRot, bool upInput) {
			setEuelerAngles();

			dampenAngularVelocity(rigidbody, xInput, zInput, xRot);

			if (!spinning(rigidbody)) {
				rotateFromInput(xInput + xRot / 8, zInput);
				stabilize();
			}
			
			applyMovementForces(rigidbody, xInput, zInput, upInput);
			
			mouseSpin (xRot);
		}

		bool spinning(Rigidbody rigidbody) {
			return rigidbody.angularVelocity.x > 2 || rigidbody.angularVelocity.y > 2 || rigidbody.angularVelocity.z > 2;
		}

		void rotateFromInput(float xInput, float zInput) {
			gameObject.transform.Rotate (new Vector3(0, 0, -xInput * timeMultiplier / 24 * tilt));
			gameObject.transform.Rotate (new Vector3(zInput * timeMultiplier / 16 * tilt, 0, 0));
		}

		void stabilize() {
			gameObject.transform.Rotate (new Vector3 (0, 0, -zAngle / (100 / stabilizers) * timeMultiplier));
			gameObject.transform.Rotate (new Vector3 (-xAngle / (100 / stabilizers) * timeMultiplier, 0, 0));
		}

		void setEuelerAngles() {
			xAngle = gameObject.transform.localEulerAngles.x;
			if (xAngle > 180) {
				xAngle = xAngle - 360;
			}
			yAngle = gameObject.transform.localEulerAngles.y;
			if (yAngle > 180) {
				yAngle = yAngle - 360;
			}
			zAngle = gameObject.transform.localEulerAngles.z;
			if (zAngle > 180) {
				zAngle = zAngle - 360;
			}
			//print ("x: " + x + ", y: " + y + ", z: " + z);
		}

		void dampenAngularVelocity(Rigidbody rigidbody, float xInput, float zInput, float xRot) {
			rigidbody.angularVelocity += new Vector3 (-rigidbody.angularVelocity.x / angularVelocityDampers * timeMultiplier, -rigidbody.angularVelocity.y / angularVelocityDampers * timeMultiplier, -rigidbody.angularVelocity.z / angularVelocityDampers * timeMultiplier);
		}

		void applyMovementForces(Rigidbody rigidbody, float xInput, float zInput, bool upInput) {
			var forces = Vector3.zero;
			forces += gameObject.transform.forward * moveSpeed * 8 * zInput * timeMultiplier;
			forces += gameObject.transform.right * moveSpeed * 6 * xInput * timeMultiplier;
			forces = removeVerticalVector (forces);
			forces = applyVelocityDampers (rigidbody, forces);
			if (upInput) {
				forces += gameObject.transform.up * moveSpeed * 5 * timeMultiplier;
			}
			rigidbody.AddForce (forces);
		}

		Vector3 removeVerticalVector(Vector3 forces) {
			forces += new Vector3 (0, -forces.y, 0);
			return forces;
		}

		Vector3 applyVelocityDampers(Rigidbody rigidbody, Vector3 forces) {
			forces += new Vector3 (-velocityDampers / 2 * rigidbody.velocity.x * timeMultiplier, -velocityDampers / 2 * rigidbody.velocity.y * timeMultiplier, -velocityDampers / 2 * rigidbody.velocity.z * timeMultiplier);
			return forces;
		}
		
		void mouseSpin(float horizontalInput) {
			gameObject.transform.Rotate (new Vector3 (0, horizontalInput, 0));
		}
	}
}