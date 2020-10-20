using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public AudioSource audioSrc;
	private float fixedGameTime;
	private int noteCount;

	/* beat counter for light effect, etc.
	public float lastBeat;
	public float timePerBeat;*/

	/* judge counter */
	public int perfectCount = 0;
	public int greatCount = 0;
	public int fineCount = 0;
	public int missCount = 0;
	public bool isFailed = false;

	/*public static float bpm;
	public static float firstBeatDelay;*/
	public bool isPlaying;
	private bool overlayPlayed;
	private bool isTutorialEnded = false;
	//public static float spawnTime;

	public ComboController comboController;
	public ScoreController scoreController;
	public HealthController healthController;

	public GameObject overlay;

	/* static gameobject */
	public GameObject gear;
	public GameObject gear_lighting;
	public GameObject gear_masking;

	public Text songTitle;
	public Text artist;
	public Text difficulty;

	// Use this for initialization
	void Start () 
	{
		/* reset all value */
		isPlaying = false;
		resetVars();

		if(GameManager.gameState == 1)
		{
			/* if the gameState is tutorial */
			audioSrc.clip = (AudioClip) Resources.Load("Songs/tutorial", typeof(AudioClip));
			SongData.loadSongData("tutorial", GameManager.notes);
			GameVideoPlayer.setMaterial("movie");
			GameVideoPlayer.insertVideo("Movies/generic.mp4",true);
		}
		else
		{
			audioSrc.clip = GameManager.songToBePlayed;
			GameVideoPlayer.setMaterial("movie");
			GameVideoPlayer.insertVideo("Movies/" + GameManager.ToBePlayed.movieFilename,false);
		}

		noteCount = getNoteCount(GameManager.notes); /* get note count with hold note addition into noteCount (used in update score) */
		insertNotesToEachLane();


		//MoviePlayer.loadVideo(SessionController.gonnaPlay.movieFilename);
		//bpm = Gam.gonnaPlay.bpm;
		//firstBeatDelay = SessionController.gonnaPlay.firstBeatOffset;
		//timePerBeat = 60f / bpm;

		/* setting up effector */
		if(GameManager.ghostEffector)
		{
			gear.GetComponent<SpriteRenderer>().enabled = false;
			gear_lighting.GetComponent<SpriteRenderer>().enabled = false;
		}
		else
		{
			gear.GetComponent<SpriteRenderer>().enabled = true;
			gear_lighting.GetComponent<SpriteRenderer>().enabled = true;

		}

		if(GameManager.liftValue != 0)
		{
			Vector3 newPosition = new Vector3(gear.transform.position.x, gear.transform.position.y + GameManager.liftValue, gear.transform.position.z);
			gear.transform.position = newPosition;
			newPosition = new Vector3(gear_lighting.transform.position.x, gear_lighting.transform.position.y + GameManager.liftValue, gear_lighting.transform.position.z); 
			gear_lighting.transform.position = newPosition;
			newPosition = new Vector3(gear_masking.transform.position.x, gear_masking.transform.position.y + GameManager.liftValue, gear_masking.transform.position.z); 
			gear_masking.transform.position = newPosition;

			for(int i=1; i < 9; i++)
			{
				GameObject lane = GameObject.Find("Lane" + i + "Controller");
				newPosition = new Vector3(lane.transform.position.x, lane.transform.position.y + GameManager.liftValue, lane.transform.position.z); 
				lane.transform.position = newPosition;
			}
			for(int i=1; i < 9; i++)
			{
				GameObject highlightlane = GameObject.Find("lane" + i + "_highlight");
				newPosition = new Vector3(highlightlane.transform.position.x, highlightlane.transform.position.y + GameManager.liftValue, highlightlane.transform.position.z); 
				highlightlane.transform.position = newPosition;
			}
			GameObject boom_fx_right = GameObject.Find("boom_fx_right");
			newPosition = new Vector3(boom_fx_right.transform.position.x, boom_fx_right.transform.position.y + GameManager.liftValue, boom_fx_right.transform.position.z); 
			boom_fx_right.transform.position = newPosition;

			GameObject boom_fx_left = GameObject.Find("boom_fx_left");
			newPosition = new Vector3(boom_fx_left.transform.position.x, boom_fx_left.transform.position.y + GameManager.liftValue, boom_fx_left.transform.position.z); 
			boom_fx_left.transform.position = newPosition;

			newPosition = new Vector3(healthController.transform.position.x, healthController.transform.position.y + GameManager.liftValue, healthController.transform.position.z);
			healthController.transform.position = newPosition;
		}

		if(GameManager.gameState == 1)
		{
			songTitle.text = "As Time Goes By...";
			artist.text = "phil_wc";
			difficulty.text = "";
		}
		else
		{
			songTitle.text = GameManager.ToBePlayed.songName;
			artist.text = GameManager.ToBePlayed.artistName;
			switch(GameManager.difficulty)
			{
			case 0:
				difficulty.text = "Easy";
				break;
			case 1:
				difficulty.text = "Normal";
				break;
			case 2:
				difficulty.text = "Hard";
				break;
			case 3:
				difficulty.text = "Master";
				break;
			}
		}



		/* Start Playing */
		if(GameManager.notes[0].time < getNoteSpawnTime())
		{
			fixedGameTime = 0 - (getNoteSpawnTime() + 1);
			GameVideoPlayer.Pause();
			StartCoroutine(playAudioVideoDelayed());
		}
		else
		{
			audioSrc.Play();
			GameVideoPlayer.Play();
		}
			
		isPlaying = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		UpdateSmoothAudioTime();
	
	}

	void Update()
	{
		
		if (isPlaying && fixedGameTime > 0)
		{
			/* gear light visualizer */
			gear_lighting.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,GetComponent<AudioSpectrum>().MeanLevels[0] * 17);
		}

		if(GameManager.gameState == 1)
		{
			/* check if song is ended or not */
			if(fixedGameTime >= 95f)
			{
				if(!isTutorialEnded)
				{
				StartCoroutine(changeToModeSelect());
				}
			}
		}
		else
		{
			if(fixedGameTime >= GameManager.notes[GameManager.notes.Count - 1].time + GameManager.notes[GameManager.notes.Count - 1].length + 0.25f)
			{
				/* show overlay */
				if(perfectCount == getNoteCount(GameManager.notes))
				{
					if(!overlayPlayed)
					{
						overlay.GetComponent<Animator>().Play("allPerfectOverlay",-1,0f);
						overlayPlayed = true;
					}
				}
				else if(comboController.getRealMaxCombo() == getNoteCount(GameManager.notes))
				{
					if(!overlayPlayed)
					{
						overlay.GetComponent<Animator>().Play("fullComboOverlay",-1,0f);
						overlayPlayed = true;
					}
				}
			}


			/* check if song is ended or not */
			if(fixedGameTime >= audioSrc.clip.length + 2f)
			{
				audioSrc.Stop();
				isPlaying = false;
				savePlayResult();
				/* change scene to result */
				GameManager.changeScene("Result");
			}
		}
	}

	/* this function reset all the track-related variables. */
	public void resetVars()
	{
		/* judge counter */
		perfectCount = 0;
		greatCount = 0;
		fineCount = 0;
		missCount = 0;
		healthController.resetHealth();
		fixedGameTime = 0f;
		audioSrc.time = 0f;
		isFailed = false;
	}

	/* this function inserts all the notes data into each list of LaneController then instantiates all the notes. */
	void insertNotesToEachLane()
	{
		/* insert notes for each lanes */
		if(GameManager.randomEffector)
		{
			List<int> alreadyLane = new List<int>();
			HashSet<int> alreadyLaneHash = new HashSet<int>();
			for(int i = 0; i < 8; i++)
			{
				int curValue = Random.Range(1, 9);
				while (alreadyLaneHash.Contains(curValue)) {
					curValue = Random.Range(1, 9);
				}
				alreadyLane.Add(curValue);
				alreadyLaneHash.Add(curValue);
			}

			for(int i = 0; i < 8; i++)
			{
				Debug.Log(alreadyLane[i]);
			}


			foreach(Note element in GameManager.notes)
			{
				switch(alreadyLane[element.lane - 1])
				{
				case 1:
					this.transform.Find("Lane1Controller").GetComponent<LaneController>().laneNotes.Add(element);

					break;
				case 2:
					this.transform.Find("Lane2Controller").GetComponent<LaneController>().laneNotes.Add(element);

					break;
				case 3:
					this.transform.Find("Lane3Controller").GetComponent<LaneController>().laneNotes.Add(element);

					break;
				case 4:
					this.transform.Find("Lane4Controller").GetComponent<LaneController>().laneNotes.Add(element);

					break;
				case 5:
					this.transform.Find("Lane5Controller").GetComponent<LaneController>().laneNotes.Add(element);

					break;
				case 6:
					this.transform.Find("Lane6Controller").GetComponent<LaneController>().laneNotes.Add(element);

					break;
				case 7:
					this.transform.Find("Lane7Controller").GetComponent<LaneController>().laneNotes.Add(element);

					break;
				case 8:
					this.transform.Find("Lane8Controller").GetComponent<LaneController>().laneNotes.Add(element);
					break;

				}
			}
		}
		else if(GameManager.mirrorEffector)
		{
			foreach(Note element in GameManager.notes)
			{
				switch(element.lane)
				{
				case 1:
					this.transform.Find("Lane8Controller").GetComponent<LaneController>().laneNotes.Add(element);

					break;
				case 2:
					this.transform.Find("Lane5Controller").GetComponent<LaneController>().laneNotes.Add(element);

					break;
				case 3:
					this.transform.Find("Lane6Controller").GetComponent<LaneController>().laneNotes.Add(element);

					break;
				case 4:
					this.transform.Find("Lane7Controller").GetComponent<LaneController>().laneNotes.Add(element);

					break;
				case 5:
					this.transform.Find("Lane2Controller").GetComponent<LaneController>().laneNotes.Add(element);

					break;
				case 6:
					this.transform.Find("Lane3Controller").GetComponent<LaneController>().laneNotes.Add(element);

					break;
				case 7:
					this.transform.Find("Lane4Controller").GetComponent<LaneController>().laneNotes.Add(element);

					break;
				case 8:
					this.transform.Find("Lane1Controller").GetComponent<LaneController>().laneNotes.Add(element);
					break;

				}
			}
		}
		else
		{
			foreach(Note element in GameManager.notes)
			{
				switch(element.lane)
				{
				case 1:
					this.transform.Find("Lane1Controller").GetComponent<LaneController>().laneNotes.Add(element);

					break;
				case 2:
					this.transform.Find("Lane2Controller").GetComponent<LaneController>().laneNotes.Add(element);

					break;
				case 3:
					this.transform.Find("Lane3Controller").GetComponent<LaneController>().laneNotes.Add(element);

					break;
				case 4:
					this.transform.Find("Lane4Controller").GetComponent<LaneController>().laneNotes.Add(element);

					break;
				case 5:
					this.transform.Find("Lane5Controller").GetComponent<LaneController>().laneNotes.Add(element);

					break;
				case 6:
					this.transform.Find("Lane6Controller").GetComponent<LaneController>().laneNotes.Add(element);

					break;
				case 7:
					this.transform.Find("Lane7Controller").GetComponent<LaneController>().laneNotes.Add(element);

					break;
				case 8:
					this.transform.Find("Lane8Controller").GetComponent<LaneController>().laneNotes.Add(element);
					break;

				}
			}
		}
		foreach(LaneController lane in this.transform.GetComponentsInChildren<LaneController>())
		{
			lane.instantiateNotes();
		}
	}

	/* this function saves play result to play data list in GameManager */
	private void savePlayResult()
	{
		int comboBonus = Mathf.RoundToInt(((float)comboController.getRealMaxCombo() / (float)noteCount) * 50000);
		PlayData played = new PlayData();

		played.perfectCount = perfectCount;
		played.greatCount = greatCount;
		played.fineCount = fineCount;
		played.missCount = missCount;
		played.maxCombo = comboController.getMaxCombo();
		played.maxRealCombo = comboController.getRealMaxCombo();
		played.mainScore = scoreController.score;
		played.comboBonusScore = comboBonus;
		played.score = scoreController.score + comboBonus;
		played.isFailed = isFailed;

		played.songID = GameManager.ToBePlayed.songID;
		played.difficulty = GameManager.difficulty;
		played.allNoteCount = getNoteCount(GameManager.notes);

		GameManager.playDataList.Add(played);
	}

	/* this function returns a number of notes (hold note counts as 2 notes KeyUp and KeyDown) */
	public int getNoteCount(List<Note> notes)
	{
		int count = 0;
		foreach(Note note in notes)
		{
			if(note.length > 0)
			{
				count += 2;
			}
			else
			{
				count++;
			}
		}
		return count;
	}

	/* this function will run if the health of player = 0 */
	public void GameOver()
	{
		if(GameManager.gameState != 1)
		{
			isFailed = true;
			/* show stage failed header */
			if(!overlayPlayed)
			{
				overlay.GetComponent<Animator>().Play("StageFailOverlay",-1,0f);
				overlayPlayed = true;
			}

			if(GameManager.stage == 3)
			{
				audioSrc.Stop();
				isPlaying = false;
				GameVideoPlayer.Pause();
				savePlayResult();

				/* change scene to result (delayed) */
				StartCoroutine(showResultDelayed());
			}
		}


	}

	/* this function returns the time that a pyramid note collides with the judgement guide in seconds */
	public float getNoteSpawnTime()
	{
		if(GameManager.speedModifier == 0.5f)
		{
			return 8.330095f;
		}
		else if(GameManager.speedModifier == 1f)
		{
			return 4.16f;
		}
		else if(GameManager.speedModifier == 1.5f)
		{
			return 2.769998f;
		}
		else if(GameManager.speedModifier == 2f)
		{
			return 2.079998f;
		}
		else if(GameManager.speedModifier == 2.5f)
		{
			return 1.669999f;
		}
		else if(GameManager.speedModifier == 3f)
		{
			return 1.389999f;
		}
		else if(GameManager.speedModifier == 3.5f)
		{
			return 1.189999f;
		}
		else if(GameManager.speedModifier == 4f)
		{
			return 1.039999f;
		}
		else if(GameManager.speedModifier == 4.5f)
		{
			return 0.9299994f;
		}
		else if(GameManager.speedModifier == 5f)
		{
			return 0.8399995f;
		}
		else if(GameManager.speedModifier == 5.5f)
		{
			return 0.7599996f;
		}
		else if(GameManager.speedModifier == 6f)
		{
			return 0.6999996f;
		}
		else if(GameManager.speedModifier == 6.5f)
		{
			return 0.6399997f;
		}
		else if(GameManager.speedModifier == 7f)
		{
			return 0.5999997f;
		}
		else if(GameManager.speedModifier == 7.5f)
		{
			return 0.5599998f;
		}
		else if(GameManager.speedModifier == 8f)
		{
			return 0.5199998f;
		}
		else if(GameManager.speedModifier == 8.5f)
		{
			return 0.4899998f;
		}
		else if(GameManager.speedModifier == 9f)
		{
			return 0.4699998f;
		}
		else if(GameManager.speedModifier == 9.5f)
		{
			return 0.4399998f;
		}
		else if(GameManager.speedModifier == 10f)
		{
			return 0.4199999f;
		}
		else if(GameManager.speedModifier == 30f)
		{
			return 0.14f;
		}
		else
		{
			return 0f;
		}
	}

	public float getScreenLatency()
	{
		return 0.01f;
	}

	/* this function updates the score to ScoreController */
	public void updateScore()
	{
		scoreController.setScore(perfectCount, greatCount, fineCount, noteCount);
	}

	/* this function will play video delayed */
	IEnumerator playAudioVideoDelayed()
	{
		while(fixedGameTime < 0)
		{
			yield return new WaitForEndOfFrame();
		}
		GameVideoPlayer.seekVideo(0f);
		audioSrc.Play();
		GameVideoPlayer.Play();
	}

	IEnumerator showResultDelayed()
	{
		yield return new WaitForSeconds(2f);
		GameManager.changeScene("Result");
	}

	IEnumerator changeToModeSelect()
	{
		isTutorialEnded = true;
		for(float i = 1f; i > 0f; i-= 0.1f)
		{
			GetComponent<AudioSource>().volume = i;
			yield return new WaitForSeconds(0.02f);
		}

		audioSrc.Stop();
		isPlaying = false;
		/* change scene to ModeSelect */
		GameVideoPlayer.setMaterial("menubg");
		GameManager.changeScene("ModeSelect");
	}

	#region Time & GameTime

	public float getCurrentFixedGameTime()
	{
		return fixedGameTime;
	}

	protected void UpdateSmoothAudioTime()
	{
		//Smooth audio time is used because the audio.time has smaller discreet steps and therefore the notes wont move
		//as smoothly. This uses Time.deltaTime to progress the audio time
		fixedGameTime += Time.fixedDeltaTime;

		//Sometimes the audio jumps or lags, this checks if the smooth audio time is off and corrects it
		//making the notes jump or lag along with the audio track
		if( IsSmoothAudioTimeOff() )
		{
			CorrectSmoothAudioTime();
		}
	}

	protected bool IsSmoothAudioTimeOff()
	{
		//Negative SmoothAudioTime means the songs playback is delayed
		if( fixedGameTime < 0f )
		{
			return false;
		}

		//Dont check this at the end of the song
		if( fixedGameTime > audioSrc.clip.length - 3f )
		{
			return false;
		}

		//Check if my smooth time and the actual audio time are of by 0.001
		return Mathf.Abs( fixedGameTime - audioSrc.time ) > 0.001f;

	}

	protected void CorrectSmoothAudioTime()
	{
		fixedGameTime = audioSrc.time;
	}
	#endregion
}
