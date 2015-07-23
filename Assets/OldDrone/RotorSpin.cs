using UnityEngine;
using System.Collections;

namespace Accord {
	public class RotorSpin : MonoBehaviour {

		public bool spinning = true;
		float speed = 0;
		float speedCap = 20;
		public bool clockwise = true;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if (spinning) {
				changeSpeed(Time.deltaTime * 70);
			} else {
				changeSpeed(-Time.deltaTime * 70);
			}
			if (clockwise) {
				gameObject.transform.Rotate (new Vector3 (0, -speed * Time.deltaTime * 70, 0));
			} else {
				gameObject.transform.Rotate (new Vector3 (0, speed * Time.deltaTime * 70, 0));
			}
		}

		public void changeSpeed(float amount) {
			speed += amount;
			if (speed > speedCap) {
				speed = speedCap;
			}
			if (speed < 0) {
				speed = 0;
			}
		}

		public void changeCap(float amount) {
			speedCap = amount;
		}
	}
}
