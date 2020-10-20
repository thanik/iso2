using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AttractScreenController : MonoBehaviour {

	public Text startText;
	public Image tutorialDialog;
	public Image arrowSelection;

	private bool tutorialDialogShown = false;
	private bool playTutorial = true;
	private bool isTransition = false;

	private bool isLogoAnimated = false;

	private float bpm = 130.0f;
	private float firstBeatDelay = 3.24697647339103f;
	private float timePerBeat;
	public float lastBeat;

	/* logo light */
	public GameObject logoText;
	public GameObject logo;

	// Use this for initialization
	void Start () {
		GameManager.gameState = 0;
		//transitionOutMovie = (MovieTexture) Resources.Load("Movies/attract-screen-transition");
		if(!GameManager.transitionFromGameOver)
		{
			GameVideoPlayer.insertVideo ("Movies/menubg.mov", true);
			GameVideoPlayer.Play();
		}
		timePerBeat = 60f / bpm;
		lastBeat = firstBeatDelay;
		StartCoroutine(isLogoAnimatedDelay());
	}

	void FixedUpdate() 
	{
		/* light effect */
		if(GetComponent<AudioSource>().time >= lastBeat + timePerBeat)
		{
			logoText.GetComponentInChildren<Animator>().Play("BeatLightFlash",-1,0f);
			if(isLogoAnimated)
			{
				logo.transform.Find("light").GetComponent<Animator>().Play("BeatLightFlash",-1,0f);
			}
			lastBeat = GetComponent<AudioSource>().time;

		}

		/* loop the song */
		if(GetComponent<AudioSource>().time >= 57.110f)
		{
			GetComponent<AudioSource>().Stop();
			GetComponent<AudioSource>().time = 0;
			lastBeat = 0;
			GetComponent<AudioSource>().Play();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(!isTransition)
		{
			if(Input.GetButtonDown("Submit"))
			{
				if(tutorialDialogShown)
				{
					if(playTutorial)
					{
						GameManager.gameState = 1;
						StartCoroutine(playTransitionScene("Tutorial"));
					}
					else
					{
						GameManager.gameState = 2;
						StartCoroutine(playTransitionScene("ModeSelect"));
					}
				}
				else
				{
					if(GameManager.credits > 0 && GameManager.gameState == 0)
					{
						GameManager.credits--;

						// show tutorial dialog
						tutorialDialogShown = true;
						tutorialDialog.GetComponent<Animator>().Play("TutorialDialogFlyIn");

					}
				}
			}

			if(tutorialDialogShown)
			{
				if(Input.GetButtonDown("fxleft") || Input.GetButtonDown("lane5") || Input.GetButtonDown("lane6") || Input.GetButtonDown("lane7"))
				{
					playTutorial = true;
				}

				if(Input.GetButtonDown("fxright") || Input.GetButtonDown("lane2") || Input.GetButtonDown("lane3") || Input.GetButtonDown("lane4"))
				{
					playTutorial = false;
				}

				if(playTutorial)
				{
					arrowSelection.rectTransform.anchoredPosition = new Vector2(-199f,-30.00004f);
				}
				else
				{
					arrowSelection.rectTransform.anchoredPosition = new Vector2(206.9999f,-30.00004f);
				}
			}

			/*if(Input.GetButtonDown("Submit"))
			{

				if(GameManager.credits > 0 && GameManager.gameState == 0)
				{
					GameManager.credits--;
					playTutorial = false;
					GameManager.gameState = 2;
					StartCoroutine(playTransitionScene("ModeSelect"));
				}
			}*/
		}

		if(tutorialDialogShown)
		{
			startText.text = "";
		}
		else
		{
			if(GameManager.coins < GameManager.coinsPerCredit)
			{
				startText.text = "INSERT COIN";
			}
			if(GameManager.coins > 0 && (GameManager.coins / GameManager.coinsPerCredit < 1))
			{
				startText.text = "INSERT MORE COINS";
			}
			if(GameManager.credits > 0)
			{
				startText.text = "PRESS START";
			}
			
		}

		if(isTransition)
		{
			startText.text = "";
		}
	}

	IEnumerator isLogoAnimatedDelay()
	{
		yield return new WaitForSeconds(3.1f);
		isLogoAnimated = true;
	}

	IEnumerator playTransitionScene(string sceneName)
	{
		tutorialDialog.GetComponent<Animator>().Play("TutorialDialogFlyOut");
		isTransition = true;


		for(float i = 1f; i > -1; i-=0.1f)
		{
			Color fading = new Color(1f,1f,1f,i);
			logo.GetComponent<SpriteRenderer>().color = fading;
			logo.GetComponentInChildren<SpriteRenderer>().color = fading;
			GetComponent<AudioSource>().volume = i;
			yield return new WaitForEndOfFrame();
		}

		GameManager.changeScene(sceneName);
	}
}
