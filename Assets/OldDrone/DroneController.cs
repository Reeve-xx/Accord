using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

namespace Accord {
	public class DroneController : MonoBehaviour {

		float x = 0;
		float y = 0;
		float z = 0;

		void Start () {
			backRightRotor().clockwise = false;
			frontRightRotor().clockwise = false;
		}
		
		void Update () {
			print (Time.deltaTime);
			Rigidbody rigidbody = gameObject.GetComponent<Rigidbody> ();

			float xInput = CrossPlatformInputManager.GetAxis ("Horizontal");
			float zInput = CrossPlatformInputManager.GetAxis ("Vertical");
			bool upInput = CrossPlatformInputManager.GetButton ("Jump");
			bool downInput = CrossPlatformInputManager.GetButton ("Fire3");

			if(CrossPlatformInputManager.GetButtonDown ("Fire1")) {
				changeCameras();
			}


			if (downInput) {
				rigidbody.useGravity = true;
				spinRotors (false);
				return;
			}

			setXYZRotation();

			setRotorSpeeds (xInput, zInput, upInput);


			rigidbody.useGravity = false;
			spinRotors (true);


			rigidbody.angularVelocity += new Vector3 (-rigidbody.angularVelocity.x / 15 * Time.deltaTime * 70, -rigidbody.angularVelocity.y / 15 * Time.deltaTime * 70, -rigidbody.angularVelocity.z / 15 * Time.deltaTime * 70);
			if (rigidbody.angularVelocity.x < 2 && rigidbody.angularVelocity.y < 2 && rigidbody.angularVelocity.z < 2) {
				rotateZ (zInput);
				rotateX (xInput);
			}

			var forces = Vector3.zero;
			forces += gameObject.transform.forward * 64 * zInput * Time.deltaTime * 70;
			forces += gameObject.transform.up * 48 * -xInput * Time.deltaTime * 70;

			forces += new Vector3 (0, -forces.y, 0);
			forces += new Vector3 (-2 * rigidbody.velocity.x * Time.deltaTime * 70, -rigidbody.velocity.y * Time.deltaTime * 70, -2 * rigidbody.velocity.z * Time.deltaTime * 70);
			if (upInput) {
				forces += gameObject.transform.right * 20 * Time.deltaTime * 70;
			}
			rigidbody.AddForce (forces);

			float xRot = CrossPlatformInputManager.GetAxis("Mouse X");

			rotateY (xRot);
		}

		void setRotorSpeeds(float xInput, float zInput, bool upInput) {
			int upBonus = 0;
			if (upInput) {
				upBonus = 2;
			}
			backLeftRotor ().changeCap (20 + zInput * 3 + xInput * 2 + upBonus);
			backRightRotor ().changeCap (20 + zInput * 3 + -xInput * 2 + upBonus);
			frontLeftRotor ().changeCap (20 + -zInput * 3 + xInput * 2 + upBonus);
			frontRightRotor ().changeCap (20 + -zInput * 3 + -xInput * 2 + upBonus);
		}

		void spinRotors(bool spin) {
			backRightRotor().spinning = spin;
			backLeftRotor().spinning = spin;
			frontRightRotor().spinning = spin;
			frontLeftRotor().spinning = spin;
		}

		RotorSpin backRightRotor() {
			Transform frontRight = gameObject.transform.GetChild(0);
			Transform frontRightBlade = frontRight.GetChild(1);
			return frontRightBlade.GetComponent<RotorSpin>();
		}

		RotorSpin backLeftRotor() {
			Transform frontLeft = gameObject.transform.GetChild (1);
			Transform frontLeftBlade = frontLeft.GetChild (1);
			return frontLeftBlade.GetComponent<RotorSpin> ();
		}

		RotorSpin frontRightRotor() {
			Transform backLeft = gameObject.transform.GetChild (3);
			Transform backLeftBlade = backLeft.GetChild (1);
			return backLeftBlade.GetComponent<RotorSpin> ();
		}

		RotorSpin frontLeftRotor() {
			Transform backRight = gameObject.transform.GetChild(2);
			Transform backRightBlade = backRight.GetChild(1);
			return backRightBlade.GetComponent<RotorSpin>();
		}

		void setXYZRotation() {
			y = gameObject.transform.localEulerAngles.y;
			if (y > 180) {
				y = y - 360;
			}
			x = gameObject.transform.localEulerAngles.x;
			if (x > 180) {
				x = x - 360;
			}
			z = gameObject.transform.localEulerAngles.z - (float) 90;
			if (z > 180) {
				z = z - 360;
			}
			print ("x: " + x + ", y: " + y + ", z: " + z);
		}

		void rotateZ(float zInput) {
			gameObject.transform.Rotate (new Vector3(0, -zInput * Time.deltaTime * 70 / 2, 0));
			gameObject.transform.Rotate (new Vector3 (0, x / 30 * Time.deltaTime * 70, 0));
		}

		void rotateX(float xInput) {
			gameObject.transform.Rotate (new Vector3(0, 0, -xInput * Time.deltaTime * 70 / 4));
			gameObject.transform.Rotate (new Vector3 (0, 0, -z / 30 * Time.deltaTime * 70));
		}

		void changeCameras() {
				Transform camera = gameObject.transform.GetChild (4);
				camera.GetComponent<Camera> ().enabled = !camera.GetComponent<Camera> ().enabled;
				Transform camera2 = gameObject.transform.GetChild (5);
				camera2.GetComponent<Camera> ().enabled = !camera2.GetComponent<Camera> ().enabled;
		}

		void rotateY(float horizontalInput) {
			gameObject.transform.Rotate (new Vector3 (horizontalInput, 0, 0));
		}
	}
}
