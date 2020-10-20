using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LaneController : MonoBehaviour {

	public List<Note> laneNotes = new List<Note>();
	private float pressedTime;
	private float pressedLength;
	private float absNoteOffset;
	private int noteIndex;
	private int noteSpawningIndex;

	public GameController gameController;
	public HealthController healthController;
	public ComboController comboController;
	public boomFXController boomFXleft;
	public boomFXController boomFXright;
	public int lane;
	public string buttonName;
	public GameObject note; //a prefab of note
	public Text EarlyLate;
	public Animator judgementAnimator;
	public Material DrawInStencilMaterial;

	private bool isJudged;
	private bool isMissed;
	private Color transparentColor = new Color(1f,1f,1f,0f);

	private Sprite[] noteTypeSprites = new Sprite[3];
	private List<GameObject> laneNoteGameObject = new List<GameObject>();

	private int i = 0;


	/* this function will return true if the player can press the note in time even if it's judged as near */
	public bool judgeNote()
	{
		if(noteIndex < laneNotes.Count)
		{
			//pressedTime = gameController.getCurrentGameTime();
			pressedTime = gameController.getCurrentFixedGameTime() - 0.01f;

			float noteOffset = pressedTime - laneNotes[noteIndex].time;
			absNoteOffset = Mathf.Abs(noteOffset);

			if (absNoteOffset > 0.160 && absNoteOffset <= 0.20)
			{
				judgementAnimator.Play("miss",-1,0f);
				comboController.setFont("miss");
				comboController.resetCombo();
				if(laneNotes[noteIndex].length > 0)
				{
					gameController.missCount += 2;
				}
				else
				{
					gameController.missCount++;
				}
				//int test = noteIndex;
				//GameObject hitNote = GameObject.Find(lane + "," + test.ToString());

				//gameController.updateScore();
				healthController.decreaseHealth(9);
				//Destroy(hitNote);
				laneNoteGameObject[noteIndex].SetActive(false);
				isMissed = true;
				if(isFloatPositive(noteOffset))
				{
					EarlyLate.text = "LATE";
				}
				else
				{
					EarlyLate.text = "EARLY";
				}
				return true;
			}
			else if(absNoteOffset <= 0.160 && absNoteOffset > 0.085)
			{
				judgementAnimator.Play("fine",-1,0f);
				comboController.setFont("fine");
				comboController.addCombo(true);
				gameController.fineCount++;
				//int test = noteIndex;
				playHitSound();
				//GameObject hitNote = GameObject.Find(lane + "," + test.ToString());
				gameController.updateScore();
				//healthController.increaseHealth(1);
				playHitNoteAnimation(lane);
				//Destroy(hitNote);
				if(laneNotes[noteIndex].length == 0)
				{
					laneNoteGameObject[noteIndex].SetActive(false);
				}
				isMissed = false;
				if(isFloatPositive(noteOffset))
				{
					EarlyLate.text = "LATE";
				}
				else
				{
					EarlyLate.text = "EARLY";
				}
				return true;
			}
			else if (absNoteOffset <= 0.085 && absNoteOffset > 0.045)
			{
				judgementAnimator.Play("great",-1,0f);
				comboController.setFont("great");
				comboController.addCombo(true);
				gameController.greatCount++;
				//int test = noteIndex;
				playHitSound();
				//GameObject hitNote = GameObject.Find(lane + "," + test.ToString());
				gameController.updateScore();
				if(!gameController.isFailed)
				{
					healthController.increaseHealth(1);
				}
				playHitNoteAnimation(lane);
				//Destroy(hitNote);
				if(laneNotes[noteIndex].length == 0)
				{
					laneNoteGameObject[noteIndex].SetActive(false);
				}
				isMissed = false;
				if(isFloatPositive(noteOffset))
				{
					EarlyLate.text = "LATE";
				}
				else
				{
					EarlyLate.text = "EARLY";
				}
				return true;
			}
			else if (absNoteOffset <= 0.045 && absNoteOffset >= 0)
			{
				judgementAnimator.Play("perfect",-1,0f);
				comboController.setFont("perfect");
				comboController.addCombo(true);
				gameController.perfectCount++;
				//int test = noteIndex;
				playHitSound();
				//GameObject hitNote = GameObject.Find(lane + "," + test.ToString());
				gameController.updateScore();
				if(!gameController.isFailed)
				{
					healthController.increaseHealth(3);
				}
				playHitNoteAnimation(lane);
				//Destroy(hitNote);
				if(laneNotes[noteIndex].length == 0)
				{
					laneNoteGameObject[noteIndex].SetActive(false);
				}
				isMissed = false;
				EarlyLate.text = "";
				return true;
			}
			else
			{
				//GameObject.Find("accurate").GetComponent<Text>().text = "";
				isMissed = false;
				return false;
			}
		}
		else
		{
			isMissed = false;
			return false;
		}
	}

	/* this function preallocates and instantiates notes GameObject into a list */
	public void instantiateNotes()
	{
		int i = 0;
		foreach(Note noteData in laneNotes)
		{
			GameObject spawnNote = null;
			if(lane > 0 && lane <= 4)
			{
				switch(lane)
				{
				case 1:
					spawnNote = (GameObject) Instantiate(note,new Vector3(-10.66f,4.12f + GameManager.liftValue,-1f),Quaternion.identity);
					break;
				case 2:
					spawnNote = (GameObject) Instantiate(note,new Vector3(-11.854f,3.56f + GameManager.liftValue,-2f),Quaternion.identity);
					break;
				case 3:
					spawnNote = (GameObject) Instantiate(note,new Vector3(-10.694f,4.21f + GameManager.liftValue,-2f),Quaternion.identity);
					break;
				case 4:
					spawnNote = (GameObject) Instantiate(note,new Vector3(-9.564f,4.86f + GameManager.liftValue,-2f),Quaternion.identity);
					break;
				default:
					break;
				}
				//spawnNote.name = lane + "," + noteSpawningIndex.ToString();
				spawnNote.name = lane + "," + i;
				spawnNote.GetComponent<NoteMovementLeft>().lane = laneNotes[noteSpawningIndex].lane;
				spawnNote.GetComponentInChildren<NineSlice>().height = GameManager.speedModifier * noteData.length * 17;
				float r = GameManager.speedModifier * noteData.length * 17;
				float n = 0.5755f;
				spawnNote.transform.Find("tail").transform.localPosition = new Vector3(r * 0.866723f * -0.5f * n - 0.5f, r * n * 0.498790f * 0.5f, 1f);
				if(lane == 1 && noteData.length == 0)
				{
					spawnNote.GetComponentInChildren<NineSlice>().height = 3;
				}
				//spawnNote.GetComponent<SpriteRenderer>().sprite


			}
			else if(lane >= 5)
			{
				switch(lane)
				{
				case 5:
					spawnNote = (GameObject) Instantiate(note,new Vector3(9.64f,4.86f + GameManager.liftValue,-2f),Quaternion.identity);
					break;
				case 6:
					spawnNote = (GameObject) Instantiate(note,new Vector3(10.72f,4.21f + GameManager.liftValue,-2f),Quaternion.identity);
					break;
				case 7:
					spawnNote = (GameObject) Instantiate(note,new Vector3(11.81f,3.56f + GameManager.liftValue,-2f),Quaternion.identity);
					break;
				case 8:
					spawnNote = (GameObject) Instantiate(note,new Vector3(10.7f,4.12f + GameManager.liftValue,-1f),Quaternion.identity);
					break;
				default:
					break;
				}
				//spawnNote.name = lane + "," + noteSpawningIndex.ToString();
				spawnNote.name = lane + "," + i;
				spawnNote.GetComponent<NoteMovementRight>().lane = laneNotes[noteSpawningIndex].lane;
				spawnNote.GetComponentInChildren<NineSlice>().height = GameManager.speedModifier * noteData.length * 17;
				float l = GameManager.speedModifier * noteData.length * 17;
				float n = 0.5755f;
				spawnNote.transform.Find("tail").transform.localPosition = new Vector3(l * 0.866723f * 0.5f * n + 0.5f, l * n * 0.498790f * 0.5f, 1f);
				if(lane == 8 && noteData.length == 0)
				{
					spawnNote.GetComponentInChildren<NineSlice>().height = 3;
				}

			}

			if(spawnNote != null)
			{
				spawnNote.SetActive(false);
				laneNoteGameObject.Add(spawnNote);
			}
			i++;
		}
	}

	/* this function plays hit sound */
	void playHitSound()
	{
		if(!GameManager.mutedEffector)
		{
			if(lane > 0 && lane < 5)
			{
				GetComponent<AudioSource>().Play();
			}
			else
			{
				GetComponent<AudioSource>().Play();
			}
		}
	}

	/* this function will return true if the float value is positive */
	private bool isFloatPositive(float number)
	{
		return number > 0;
	}

	private void playHitNoteAnimation(int lane)
	{
		if(lane == 1)
		{
			boomFXright.playBoomAnimation();
		}
		else if(lane == 8)
		{
			boomFXleft.playBoomAnimation();
		}
		else if(lane > 0 && lane < 5)
		{
			this.GetComponent<Animator>().Play("boom",-1,0f);
		}
		else
		{
			this.GetComponent<Animator>().Play("boom_red",-1,0f);
		}
	}

	void Start () 
	{
		pressedTime = 0f;
		pressedLength = 0f;
		noteIndex = 0;
		noteSpawningIndex = 0;
	}

	void FixedUpdate()
	{
		/* spawning note by time */
		if(noteSpawningIndex < laneNotes.Count && gameController.getCurrentFixedGameTime() + gameController.getNoteSpawnTime() + gameController.getScreenLatency() >= laneNotes[noteSpawningIndex].time)
		{
			laneNoteGameObject[noteSpawningIndex].SetActive(true);
			noteSpawningIndex++;
		}

		if(gameController.isPlaying)
		{
			if(noteIndex < laneNotes.Count)
			{
				/* if the player doesn't press in time */
				if (gameController.getCurrentFixedGameTime() > laneNotes[noteIndex].time + 0.2f)
				{
					if (noteIndex < laneNotes.Count)
					{
						laneNoteGameObject[noteIndex].SetActive(false);
						if(laneNotes[noteIndex].length > 0)
						{
							gameController.missCount += 2;
						}
						else
						{
							gameController.missCount++;
						}
						noteIndex++; /* judge next note */
						judgementAnimator.Play("miss",-1,0f);
						comboController.setFont("miss");
						comboController.resetCombo();
						healthController.decreaseHealth(9);
						isJudged = false; /* reset the value for the next note */
						return;

					}
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{

		if(gameController.isPlaying)
		{
			if(noteIndex < laneNotes.Count)
			{
				/* check if the note that is going to be judged is hold note or not */
				if (laneNotes[noteIndex].length > 0)
				{

					/* start judging note */
					if(Input.GetButtonDown(buttonName)) 
					{
						isJudged = judgeNote();
						if (isJudged && noteIndex < laneNotes.Count) noteIndex++;
						pressedLength = 0f;
					}
				}
				else
				{
					/* judge normal note */
					if(Input.GetButtonDown(buttonName)) 
					{
						if (judgeNote() && noteIndex < laneNotes.Count) noteIndex++;
					}
				}
			}

			if(noteIndex > 0 && laneNotes[noteIndex - 1].length > 0)
			{
				/* judge hold note */
				if (!isMissed && Input.GetButton(buttonName))
				{
					pressedLength += Time.deltaTime;
					//GameObject.Find("pressedLength").GetComponent<Text>().text = "Pressed length: " + pressedLength;
					if (noteIndex > 0)
					{
						if (laneNotes[noteIndex - 1].length > 0 && absNoteOffset < 0.2f)
						{
							if (pressedLength < laneNotes[noteIndex - 1].length)
							{
								//if(gameController.getCurrentGameTime() > gameController.lastBeat + gameController.timePerBeat) 
								if(i == 0)
								{
									comboController.addCombo(false);


								}
								i = (i + 1) % 5;

								laneNoteGameObject[noteIndex - 1].GetComponentInChildren<MeshRenderer>().material = DrawInStencilMaterial;
								if(lane > 1 && lane < 8)
								{
									laneNoteGameObject[noteIndex - 1].GetComponent<SpriteRenderer>().color = transparentColor;
								}

								playHitNoteAnimation(lane);
							}
						}
					}
				}

				if(isJudged && Input.GetButtonUp(buttonName))
				{
					if(noteIndex > 0)
					{
						if(laneNotes[noteIndex-1].length > 0 && pressedLength < (laneNotes[noteIndex-1].length - 0.25f))
						{
							judgementAnimator.Play("miss",-1,0f);
							comboController.setFont("miss");
							laneNoteGameObject[noteIndex-1].SetActive(false);
							isMissed = true;
							comboController.resetCombo();
							gameController.missCount++;

							gameController.updateScore();
						}
						else
						{
							
							comboController.addRealCombo();
							gameController.perfectCount++;
							gameController.updateScore();
						}
					}
				}

				if(Input.GetButtonUp(buttonName))
				{
					isJudged = false; /* reset the value for the next note */
					//isMissed = false;
				}
			}

			//if(gameController.getCurrentGameTime() > gameController.lastBeat + gameController.timePerBeat) gameController.lastBeat += gameController.timePerBeat;

		}
	}
}
