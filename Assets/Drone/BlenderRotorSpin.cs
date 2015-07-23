using UnityEngine;
using System.Collections;

namespace Accord {

	public class BlenderRotorSpin : MonoBehaviour {

		public bool spinning = true;
		float speed = 0;
		float speedCap = 30;
		public bool clockwise = true;
		
		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if (spinning) {
				changeSpeed(Time.deltaTime * 70 / 4);
			} else {
				changeSpeed(-Time.deltaTime * 70 / 4);
			}
			if (clockwise) {
				gameObject.transform.Rotate (new Vector3 (0, 0, speed * Time.deltaTime * 70));
			} else {
				gameObject.transform.Rotate (new Vector3 (0, 0, -speed * Time.deltaTime * 70));
			}
		}
		
		public void changeSpeed(float amount) {
			speed += amount;
			//print (speedCap);
			if (speed > speedCap) {
				speed = speedCap;
			}
			if (speed < 0) {
				speed = 0;
			}
		}
		
		public void changeCap(float amount) {
			if (amount < 1) {
				amount = 1;
			}
			if (amount > 8) {
				amount = 8;
			}
			speedCap = amount;
		}
	}
}