using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameTesterController : MonoBehaviour {

	public Text randomEffectorText;
	public Text ghostEffectorText;
	public Text mirrorEffectorText;
	public Text mutedKeySoundText;
	public Text liftEffectorText;
	public Text liftValueText;
	public Text speedModifierText;
	public Text sceneLoadingText;
	public Text songLoadingText;


	private bool isLoaded = false;
	private bool allowLoad = false;
	private AsyncOperation loadLevelAsync;
	private bool isTransitioning = false;

	IEnumerator loadResources()
	{
		GameManager.notes.Clear();
		NotesData.loadSongData("maid_master",GameManager.notes);
		//ResourceRequest movieResource = Resources.LoadAsync("Movies/menubg", typeof(MovieTexture));
		//while(!movieResource.isDone)
		//{
		//	//Debug.Log("LoadMovieProgress: " + movieResource.progress.ToString());
		//	yield return null;
		//}
		//GameManager.movieToBePlayed = (MovieTexture) movieResource.asset;

		//ResourceRequest songResource = Resources.LoadAsync("Songs/" + GameManager.gonnaPlay.songFilename, typeof(AudioClip));
		ResourceRequest songResource = Resources.LoadAsync("Songs/MaidBattleSC", typeof(AudioClip));
		while(!songResource.isDone)
		{
			songLoadingText.text = "LoadSongProgress: " + songResource.progress.ToString("0.000");
			yield return null;
		}
		GameManager.songToBePlayed = (AudioClip) songResource.asset;

		loadLevelAsync = GameManager.loadSceneAsync("Play");
		loadLevelAsync.allowSceneActivation = false;
		while(loadLevelAsync.progress < 0.9f)
		{
			sceneLoadingText.text = "LoadLvProgress: " + loadLevelAsync.progress.ToString("0.000");
			yield return null;
		}
		isLoaded = true;

		while(!allowLoad)
		{
			//Debug.Log("LoadLvProgress: " + loadLevelAsync.progress.ToString());
			yield return null;
		}
		loadLevelAsync.allowSceneActivation = true;
	}

	// Use this for initialization
	void Start () {
		StartCoroutine(loadResources());
	}
	
	// Update is called once per frame
	void Update () {
		/* update text */
		speedModifierText.text = GameManager.speedModifier.ToString("0.0") + "x";
		liftValueText.text = GameManager.liftValue.ToString("0.00");
		if(GameManager.ghostEffector)
		{
			ghostEffectorText.color = new Color(1f,1f,1f);
		}
		else
		{
			ghostEffectorText.color = new Color(0.392f,0.392f,0.392f);

		}

		if(GameManager.mutedEffector)
		{
			mutedKeySoundText.color = new Color(1f,1f,1f);
		}
		else
		{
			mutedKeySoundText.color = new Color(0.392f,0.392f,0.392f);
		}

		if(GameManager.randomEffector)
		{
			randomEffectorText.color = new Color(1f,1f,1f);
		}
		else
		{
			randomEffectorText.color = new Color(0.392f,0.392f,0.392f);
		}

		if(GameManager.mirrorEffector)
		{
			mirrorEffectorText.color = new Color(1f,1f,1f);
		}
		else
		{
			mirrorEffectorText.color = new Color(0.392f,0.392f,0.392f);
		}

		if(GameManager.liftValue != 0)
		{
			liftEffectorText.color = new Color(1f,1f,1f);
			liftValueText.color = new Color(1f,1f,1f);
		}
		else
		{
			liftEffectorText.color = new Color(0.392f,0.392f,0.392f);
			liftValueText.color = new Color(0.392f,0.392f,0.392f);
		}

		if(!Input.GetButton("fxleft") && Input.GetButtonDown("lane6") && !isTransitioning)
		{
			if(GameManager.speedModifier < 10)
			{
				GameManager.speedModifier += 0.5f;
			}
			else if(GameManager.speedModifier == 10f)
			{
				GameManager.speedModifier = 30f;
			}
		}
		if(!Input.GetButton("fxleft") && Input.GetButtonDown("lane5") && !isTransitioning)
		{
			if(GameManager.speedModifier == 30f)
			{
				GameManager.speedModifier = 10f;
			}
			else if(GameManager.speedModifier > 1)
			{
				GameManager.speedModifier -= 0.5f;
			}

		}

		if(Input.GetButton("fxleft") && !isTransitioning)
		{
			if(Input.GetButtonDown("lane6"))
			{
				if(GameManager.liftValue < 7)
				{
					GameManager.liftValue += 0.25f;
				}
			}
			if(Input.GetButtonDown("lane5"))
			{
				if(GameManager.liftValue > -7)
				{
					GameManager.liftValue -= 0.25f;
				}
			}
		}

		if(Input.GetButtonDown("lane2") && !isTransitioning)
		{
			if(GameManager.ghostEffector)
			{
				GameManager.ghostEffector = false;
			}
			else
			{
				GameManager.ghostEffector = true;
			}
		}

		if(Input.GetButtonDown("lane3") && !isTransitioning)
		{
			if(GameManager.randomEffector)
			{
				GameManager.randomEffector = false;
			}
			else
			{
				GameManager.randomEffector = true;
			}
		}

		if(Input.GetButtonDown("fxright") && !isTransitioning)
		{
			if(GameManager.mutedEffector)
			{
				GameManager.mutedEffector = false;
			}
			else
			{
				GameManager.mutedEffector = true;
			}
		}

		if(Input.GetButtonDown("lane4") && !isTransitioning)
		{
			if(GameManager.mirrorEffector)
			{
				GameManager.mirrorEffector = false;
			}
			else
			{
				GameManager.mirrorEffector = true;
			}
		}


		if(Input.GetButtonDown("Submit"))
		{
			if(isLoaded)
			{

				allowLoad = true;
			}
		}
	}
}
