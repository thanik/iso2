using UnityEngine;
using System.Collections;

public class ModeSelectController : MonoBehaviour {

	private bool isTransition = false;
	public AudioClip pressedSound;

	public GameObject selectModeCanvas;

	// Use this for initialization
	void Start () {
		GameManager.gameState = 2;
		//menuBg = (MovieTexture) Resources.Load("Movies/menubg");
		if(GameManager.startedTutorial)
		{
			GameVideoPlayer.insertVideo ("Movies/menubg.mov", true);
			GameVideoPlayer.Play();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(!isTransition)
		{
			if(Input.GetButtonDown("Submit"))
			{
				GameManager.stage = 1;
				StartCoroutine( playTransitionScene("MusicSelect"));
			}
		}
	}

	IEnumerator playTransitionScene(string sceneName)
	{
		isTransition = true;
		GetComponent<AudioSource>().clip = pressedSound;
		GetComponent<AudioSource>().Play();
		//VideoPlayer.insertVideo(transitionOutMovie,false);
		//VideoPlayer.Play();
		selectModeCanvas.GetComponent<Animator>().Play("selectedMode",-1,0f);
		yield return new WaitForSeconds(3f);
		GameManager.changeScene(sceneName);
	}
}
