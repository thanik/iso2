using UnityEngine;
using System.Collections;

public class LaneHighlighter : MonoBehaviour {

	public string buttonName;
	
	// Update is called once per frame
	void Update () {
		GetComponent<SpriteRenderer>().enabled = Input.GetButton(buttonName);
	}
}
