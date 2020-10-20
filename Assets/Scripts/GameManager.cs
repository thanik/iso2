using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	private static GameManager _instance;

	public static int credits = 0;
	public static int coins = 0;
	public static int stage = 1;
	public static int mode;
	public static bool freeMode = false;
	public static int coinsPerCredit = 1;
	public static int gameState = 0; /* 0 = attract mode, 1 = tutorial, 2 = mode select, 3 = song select, 4 = playing, 5 = result, 6 = all result, 7 = game over */
	public static float speedModifier = 2f;
	public static bool randomEffector = false;
	public static bool ghostEffector = false;
	public static bool mutedEffector = false;
	public static bool mirrorEffector = false;
	public static float liftValue = 0f;

	public static bool startedTutorial = false;
	public static bool transitionFromGameOver = false;

	/* Thr track to be played data cache */
	public static SongData ToBePlayed;
	public static int difficulty;
	public static List<Note> notes = new List<Note>();
	public static AudioClip songToBePlayed;

	/* play data */
	public static List<PlayData> playDataList = new List<PlayData>();

	public Text creditText;

	public static void changeScene(string SceneName)
	{
		SceneManager.LoadScene(SceneName,LoadSceneMode.Single);
	}

	public static AsyncOperation loadSceneAsync(string SceneName)
	{
		return SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Single);
	}

	/* this function resets the play session-related variables and clears the data cache. */
	public static void resetSession()
	{
		stage = 1;
		playDataList.Clear();
		notes.Clear();
		ToBePlayed = null;
		difficulty = 0;
		gameState = 0;
		randomEffector = false;
		ghostEffector = false;
		mutedEffector = false;
		mirrorEffector = false;
		startedTutorial = false;
		transitionFromGameOver = false;
		liftValue = 0f;
		speedModifier = 2f;
		SceneManager.LoadScene("AttractScreen");
	}

	// Use this for initialization
	void Start () 
	{
		SongLibrary.loadSongDatabase();
		resetSession();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetButtonDown("coin"))
		{
			GetComponent<AudioSource>().Play();
			coins++;
			if(coins >= coinsPerCredit)
			{
				coins -= coinsPerCredit;
				credits++;
			}
		}

		creditText.text = "CREDITS: " + credits + " (" + coins + "/" + coinsPerCredit + ")";
	}

	void Awake()
	{
		if(!_instance)
		{
			_instance = this;
		}
		else
		{
			Destroy(this.gameObject);
		}
		DontDestroyOnLoad(this.gameObject);
	}
}
