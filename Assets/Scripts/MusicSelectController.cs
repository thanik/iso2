using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MusicSelectController : MonoBehaviour {

	public bool effectorOptionsDialogShown = false;

	public int selectedItem = 0;
	public int selectedDifficulty = 0;
	public int firstItemIndex = 0;

	public int optionIndex = 0;
	public List<GameObject> optionList;
	public List<GameObject> difficultyList;
	public GameObject selectedBar;
	public GameObject selectedDifficultyBar;
	public GameObject jacketImage;
	public GameObject effectorPanel;
	public Image vertexImg;
	public Image blackScreen;

	private bool isTransitioning = false;
	private bool isPreviewing = false;
	private float previewDelay = 0f;

	/* effector values */
	public Text speedText;
	public Text liftText;
	public Image randomEffect;
	public Image ghostEffect;
	public Image mirrorEffect;
	public Image muteEffect;

	/* effector preview */
	public Text speedTextPreview;
	public Text liftTextPreview;
	public Image randomEffectPreview;
	public Image ghostEffectPreview;
	public Image mirrorEffectPreview;
	public Image muteEffectPreview;

	public List<Sprite> randomImg;
	public List<Sprite> ghostImg;
	public List<Sprite> mirrorImg;
	public List<Sprite> muteImg;

	public List<Sprite> vertexImgList;

	/* values for loading resource */
	private bool isLoaded = false;
	private bool allowLoad = false;
	private AsyncOperation loadLevelAsync;

	public Image arrow_up;
	public Image arrow_down;
	// Use this for initialization
	void Start () {
		GameVideoPlayer.Play();
		StartCoroutine(startMenuAnimation());
	}
	
	// Update is called once per frame
	void Update () {
		/* all button event */
		if(effectorOptionsDialogShown)
		{
			if(Input.GetButtonDown("Submit") && !isTransitioning)
			{
				effectorPanel.GetComponent<Animator>().Play("effectorDialogFlyOut",-1,0f);
				effectorOptionsDialogShown = false;
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

			if(Input.GetButtonDown("lane4") && !isTransitioning)
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

			if(Input.GetButtonDown("lane2") && !isTransitioning)
			{
				if(GameManager.randomEffector)
				{
					GameManager.randomEffector = false;
				}
				else
				{
					GameManager.randomEffector = true;
				}

				if(GameManager.mirrorEffector == true)
				{
					GameManager.mirrorEffector = false;
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

			if(Input.GetButtonDown("lane3") && !isTransitioning)
			{
				if(GameManager.mirrorEffector)
				{
					GameManager.mirrorEffector = false;
				}
				else
				{
					GameManager.mirrorEffector = true;
				}

				if(GameManager.randomEffector == true)
				{
					GameManager.randomEffector = false;
				}
			}
		}
		else
		{
			if(Input.GetButtonDown("lane6") && !isTransitioning)
			{
				/* down */
				if(selectedItem < 7 && selectedItem >= 0)
				{
					selectedItem++;
				}
				else if(selectedItem == 7 && (firstItemIndex + 8 < SongLibrary.database.Count))
				{
					firstItemIndex++;
				}

				previewDelay = 0f;
				isPreviewing = false;
				StartCoroutine(fadeAndLoadPreviewSong());
			}
			if(Input.GetButtonDown("lane5") && !isTransitioning)
			{
				/* up */
				if(selectedItem <= 7 && selectedItem > 0)
				{
					selectedItem--;
				}
				else if(selectedItem == 0 && (firstItemIndex - 1 >= 0))
				{
					firstItemIndex--;
				}
				previewDelay = 0f;
				isPreviewing = false;
				StartCoroutine(fadeAndLoadPreviewSong());
			}

			if(Input.GetButtonDown("lane3") && !isTransitioning)
			{

				if(selectedDifficulty < 3 && selectedDifficulty >= 0)
				{
					selectedDifficulty++;
				}
			}

			if(Input.GetButtonDown("lane2") && !isTransitioning)
			{
				if(selectedDifficulty < 4 && selectedDifficulty > 0)
				{
					selectedDifficulty--;
				}
			}

			/* select song! */
			if(Input.GetButtonDown("Submit") && !isTransitioning)
			{
				GameVideoPlayer.setLooping(false);
				/* load note data */
				GameManager.ToBePlayed = SongLibrary.database[firstItemIndex + selectedItem];
				GameManager.notes.Clear();
				switch(selectedDifficulty)
				{
				case 0:
					SongData.loadSongData(GameManager.ToBePlayed.easyChartFilename, GameManager.notes);
					GameManager.difficulty = 0;
					break;
				case 1:
					SongData.loadSongData(GameManager.ToBePlayed.normalChartFilename, GameManager.notes);
					GameManager.difficulty = 1;
					break;
				case 2:
					SongData.loadSongData(GameManager.ToBePlayed.hardChartFilename, GameManager.notes);
					GameManager.difficulty = 2;
					break;
				case 3:
					SongData.loadSongData(GameManager.ToBePlayed.masterChartFilename, GameManager.notes);
					GameManager.difficulty = 3;
					break;
				}
				/* load song and movie */
				StartCoroutine(loadResources());
				StartCoroutine(transitionToGame());

			}

			if(Input.GetButton("fxleft") && Input.GetButton("fxright"))
			{
				effectorOptionsDialogShown = true;
				effectorPanel.GetComponent<Animator>().Play("effectorDialogFlyIn",-1,0f);

			}
		}

		/* value updating */
	
		if(GameManager.randomEffector)
		{
			randomEffect.GetComponent<Image>().sprite = randomImg[1];
			randomEffectPreview.GetComponent<Image>().sprite = randomImg[1];
		}
		else
		{
			randomEffect.GetComponent<Image>().sprite = randomImg[0];
			randomEffectPreview.GetComponent<Image>().sprite = randomImg[0];
		}

		if(GameManager.ghostEffector)
		{
			ghostEffect.GetComponent<Image>().sprite = ghostImg[1];
			ghostEffectPreview.GetComponent<Image>().sprite = ghostImg[1];
		}
		else
		{
			ghostEffect.GetComponent<Image>().sprite = ghostImg[0];
			ghostEffectPreview.GetComponent<Image>().sprite = ghostImg[0];
		}

		if(GameManager.mutedEffector)
		{
			muteEffect.GetComponent<Image>().sprite = muteImg[1];
			muteEffectPreview.GetComponent<Image>().sprite = muteImg[1];
		}
		else
		{
			muteEffect.GetComponent<Image>().sprite = muteImg[0];
			muteEffectPreview.GetComponent<Image>().sprite = muteImg[0];
		}

		if(GameManager.mirrorEffector)
		{
			mirrorEffect.GetComponent<Image>().sprite = mirrorImg[1];
			mirrorEffectPreview.GetComponent<Image>().sprite = mirrorImg[1];
		}
		else
		{
			mirrorEffect.GetComponent<Image>().sprite = mirrorImg[0];
			mirrorEffectPreview.GetComponent<Image>().sprite = mirrorImg[0];
		}

		speedText.text = GameManager.speedModifier.ToString("0.0");
		liftText.text = GameManager.liftValue.ToString("0.00");
		speedTextPreview.text = GameManager.speedModifier.ToString("0.0");
		liftTextPreview.text = GameManager.liftValue.ToString("0.00");

		vertexImg.sprite = vertexImgList[GameManager.stage - 1];

		/* selection update */
		optionIndex = 0;
		foreach(GameObject option in optionList)
		{
			if(firstItemIndex + optionIndex < SongLibrary.database.Count)
			{
				option.transform.Find("songTitle").GetComponent<Text>().text = SongLibrary.database[firstItemIndex + optionIndex].songName;
				option.transform.Find("artist").GetComponent<Text>().text = SongLibrary.database[firstItemIndex + optionIndex].artistName;
			}
			optionIndex++;
		}

		switch(selectedItem)
		{
		case 0:
			selectedBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(-0.5000305f,212f);
			break;
		case 1:
			selectedBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(-0.5000305f,151f);
			break;
		case 2:
			selectedBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(-0.5000305f,90f);
			break;
		case 3:
			selectedBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(-0.5000305f,29f);
			break;
		case 4:
			selectedBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(-0.5000305f,-32f);
			break;
		case 5:
			selectedBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(-0.5000305f,-91.9f);
			break;
		case 6:
			selectedBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(-0.5000305f,-152.9f);
			break;
		case 7:
			selectedBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(-0.5000305f,-213.9f);
			break;
		}

		switch(selectedDifficulty)
		{
		case 0:
			selectedDifficultyBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(385.9999f,23.20003f);
			break;
		case 1:
			selectedDifficultyBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(385.9999f,-59.7f);
			break;
		case 2:
			selectedDifficultyBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(385.9999f,-142.4f);
			break;
		case 3:
			selectedDifficultyBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(385.9999f,-225f);
			break;
		}

		if(firstItemIndex + selectedItem < SongLibrary.database.Count)
		{
			/* update songinfo to right side */
			jacketImage.GetComponent<Image>().sprite = (Sprite) Resources.Load("SongArt/" + SongLibrary.database[firstItemIndex + selectedItem].songJacketFilename, typeof(Sprite));
				
			string stars = "";
			difficultyList[0].transform.Find("lvText").GetComponent<Text>().text = "Lv." + SongLibrary.database[firstItemIndex + selectedItem].easyLv;
			for(int i = 0; i < SongLibrary.database[firstItemIndex + selectedItem].easyLv; i++)
			{
				stars += "★";
			}
			difficultyList[0].transform.Find("stars").GetComponent<Text>().text = stars;
			difficultyList[0].transform.Find("artist").GetComponent<Text>().text = "by " + SongLibrary.database[firstItemIndex + selectedItem].easyChartDesigner;

			stars = "";
			difficultyList[1].transform.Find("lvText").GetComponent<Text>().text = "Lv." + SongLibrary.database[firstItemIndex + selectedItem].normalLv;
			for(int i = 0; i < SongLibrary.database[firstItemIndex + selectedItem].normalLv; i++)
			{
				stars += "★";
			}
			difficultyList[1].transform.Find("stars").GetComponent<Text>().text = stars;
			difficultyList[1].transform.Find("artist").GetComponent<Text>().text = "by " + SongLibrary.database[firstItemIndex + selectedItem].normalChartDesigner;

			stars = "";
			difficultyList[2].transform.Find("lvText").GetComponent<Text>().text = "Lv." + SongLibrary.database[firstItemIndex + selectedItem].hardLv;
			for(int i = 0; i < SongLibrary.database[firstItemIndex + selectedItem].hardLv; i++)
			{
				stars += "★";
			}
			difficultyList[2].transform.Find("stars").GetComponent<Text>().text = stars;
			difficultyList[2].transform.Find("artist").GetComponent<Text>().text = "by " + SongLibrary.database[firstItemIndex + selectedItem].hardChartDesigner;

			stars = "";
			difficultyList[3].transform.Find("lvText").GetComponent<Text>().text = "Lv." + SongLibrary.database[firstItemIndex + selectedItem].masterLv;
			for(int i = 0; i < SongLibrary.database[firstItemIndex + selectedItem].masterLv; i++)
			{
				stars += "★";
			}
			difficultyList[3].transform.Find("stars").GetComponent<Text>().text = stars;
			difficultyList[3].transform.Find("artist").GetComponent<Text>().text = "by " + SongLibrary.database[firstItemIndex + selectedItem].masterChartDesigner;
		}

		/* song preview delaying */
		if(!isPreviewing && previewDelay < 0.4)
		{
			previewDelay += Time.deltaTime;
		}

		/* song list arrow */
		if(firstItemIndex + 8 < SongLibrary.database.Count)
		{
			arrow_down.enabled = true;
		}
		else
		{
			arrow_down.enabled = false;
		}

		if(firstItemIndex > 0)
		{
			arrow_up.enabled = true;
		}
		else
		{
			arrow_up.enabled = false;
		}
	}

	IEnumerator startMenuAnimation()
	{
		vertexImg.GetComponent<Animator>().Play("vertexImgFlyIn");
		yield return new WaitForSeconds(1.35f);
		foreach(GameObject option in optionList)
		{
			option.GetComponent<Animator>().Play("subMenuShowIn");
			yield return new WaitForSeconds(0.05f);
		}
		foreach(GameObject option in difficultyList)
		{
			option.GetComponent<Animator>().Play("subMenuShowIn");
			yield return new WaitForSeconds(0.05f);
		}
		selectedBar.GetComponent<Animator>().Play("subMenuShowIn");
		yield return new WaitForSeconds(0.05f);
		selectedDifficultyBar.GetComponent<Animator>().Play("subMenuShowIn");
		StartCoroutine(fadeAndLoadPreviewSong());
	}

	IEnumerator fadeAndLoadPreviewSong()
	{
		ResourceRequest previewSong = Resources.LoadAsync("Songs/" + SongLibrary.database[firstItemIndex + selectedItem].songFilename);
		while(!previewSong.isDone)
		{
			yield return new WaitForEndOfFrame();
		}
		GetComponent<Animator>().Play("musicPreviewFadeOut");

		yield return new WaitForSeconds(0.35f);

		GetComponent<AudioSource>().clip = (AudioClip) previewSong.asset;
		GetComponent<AudioSource>().time = 60f;
		while(previewDelay < 0.4)
		{
			yield return new WaitForEndOfFrame();
		}
		GetComponent<AudioSource>().Play();
		GetComponent<Animator>().Play("musicPreviewFadeIn");
	}

	IEnumerator loadResources()
	{
		isTransitioning = true;
		blackScreen.gameObject.SetActive(true);
		blackScreen.GetComponent<Animator>().Play("fadeToBlackScreen",-1,0f);
		GetComponent<Animator>().Play("musicPreviewFadeOut");
		/*ResourceRequest movieResource = Resources.LoadAsync("Movies/" + GameManager.ToBePlayed.movieFilename, typeof(MovieTexture));
		while(!movieResource.isDone)
		{
			//Debug.Log("LoadMovieProgress: " + movieResource.progress.ToString());
			yield return new WaitForEndOfFrame();
		}
		GameManager.movieToBePlayed = (MovieTexture) movieResource.asset;*/

		ResourceRequest songResource = Resources.LoadAsync("Songs/" + GameManager.ToBePlayed.songFilename, typeof(AudioClip));
		while(!songResource.isDone)
		{
			yield return new WaitForEndOfFrame();
		}
		GameManager.songToBePlayed = (AudioClip) songResource.asset;

		loadLevelAsync = GameManager.loadSceneAsync("Play");
		loadLevelAsync.allowSceneActivation = false;
		while(loadLevelAsync.progress < 0.9f)
		{
			yield return new WaitForEndOfFrame();
		}
		isLoaded = true;

		while(!allowLoad)
		{
			//Debug.Log("LoadLvProgress: " + loadLevelAsync.progress.ToString());
			yield return new WaitForEndOfFrame();
		}
		loadLevelAsync.allowSceneActivation = true;
	}

	IEnumerator transitionToGame()
	{
		while(!isLoaded)
		{
			yield return new WaitForEndOfFrame();
		}
		if(!GameManager.ghostEffector)
		{
			blackScreen.GetComponent<Animator>().Play("transitionToGame",-1,0f);
		}
		yield return new WaitForSeconds(1f);
		allowLoad = true;
		GameManager.gameState = 4;
	}
}
