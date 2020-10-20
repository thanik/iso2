using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NoteMovementCalibration : MonoBehaviour {

	private float time;
	private GameObject judgementRight;
	private bool isCalibrated = false;
	// Use this for initialization
	void Start () {
		time = 0;
	}

	// Update is called once per frame
	void FixedUpdate () {
		time += Time.fixedDeltaTime;
	}

	void OnTriggerEnter2D(Collider2D note)
	{
		Debug.Log("from: " + this.gameObject.name + " time: " + time + " object: " + note.gameObject.name);
		if(!isCalibrated)
		{
			//GameManager.spawnTime = time;

			isCalibrated = true;
		}
	}
}
