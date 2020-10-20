using UnityEngine;
using System.Collections;

public class GameOverController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameManager.gameState = 7;
	}
	
	// Update is called once per frame
	void Update () {
		
		StartCoroutine(waitForReset());
	}

	IEnumerator waitForReset()
	{
		yield return new WaitForSeconds(4f);
		GameManager.transitionFromGameOver = true;
		GameManager.resetSession();
	}
}
