using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ResultController : MonoBehaviour {

	public Text header;

	public Text songTitle;
	public Image songArt;
	public Text artist;
	public Text difficulty;
	public Text stars;
	public Text chartLevel;

	public Text maxRealCombo;
	public Text perfectCount;
	public Text greatCount;
	public Text fineCount;
	public Text missCount;
	public Text score;
	public Text mainScore;
	public Text comboBonusScore;

	public Image grade;
	public Image fullCombo;
	public Text allNoteCount;

	public List<Sprite> gradeImg;

	PlayData currentPlayData;

	private bool isAnimating = true;

	// Use this for initialization
	void Start () {
		GameManager.gameState = 5;
		GameVideoPlayer.setMaterial("menubg");
		GameVideoPlayer.insertVideo ("Movies/menubg.mov", true);
		GameVideoPlayer.Play();
		currentPlayData = GameManager.playDataList[GameManager.stage - 1];

		if(currentPlayData.isFailed)
		{
			header.text = "VERTEX " + GameManager.stage + " FAILED";
			grade.sprite = gradeImg[8];
		}
		else
		{
			header.text = "VERTEX " + GameManager.stage + " CLEARED";
		}
		songTitle.text = SongLibrary.database[currentPlayData.songID].songName;
		artist.text = SongLibrary.database[currentPlayData.songID].artistName;
		songArt.sprite = (Sprite) Resources.Load("SongArt/" + SongLibrary.database[currentPlayData.songID].songJacketFilename, typeof(Sprite));
		string starsString = "";
		switch(currentPlayData.difficulty)
		{
		case 0:
			difficulty.text = "Easy";
			chartLevel.text = "Lv." + SongLibrary.database[currentPlayData.songID].easyLv.ToString();

			for(int i = 0; i < SongLibrary.database[currentPlayData.songID].easyLv; i++)
			{
				starsString += "★";
			}
			stars.text = starsString;
			break;
		case 1:
			difficulty.text = "Normal";
			chartLevel.text = "Lv." + SongLibrary.database[currentPlayData.songID].normalLv.ToString();
			for(int i = 0; i < SongLibrary.database[currentPlayData.songID].normalLv; i++)
			{
				starsString += "★";
			}
			stars.text = starsString;
			stars.color = new Color(0.05098039216f,0.4117647059f,0.7490196078f);
			break;
		case 2:
			difficulty.text = "Hard";
			chartLevel.text = "Lv." + SongLibrary.database[currentPlayData.songID].hardLv.ToString();
			for(int i = 0; i < SongLibrary.database[currentPlayData.songID].hardLv; i++)
			{
				starsString += "★";
			}
			stars.text = starsString;
			stars.color = new Color(0.8823529412f,0.1333333333f,0.1333333333f);
			break;
		case 3:
			difficulty.text = "Master";
			chartLevel.text = "Lv." + SongLibrary.database[currentPlayData.songID].masterLv.ToString();
			for(int i = 0; i < SongLibrary.database[currentPlayData.songID].masterLv; i++)
			{
				starsString += "★";
			}
			stars.text = starsString;
			stars.color = new Color(0.5098039216f,0f,0.6980392157f);
			break;
		}

		maxRealCombo.text = currentPlayData.maxRealCombo.ToString();
		perfectCount.text = currentPlayData.perfectCount.ToString();
		greatCount.text = currentPlayData.greatCount.ToString();
		fineCount.text = currentPlayData.fineCount.ToString();
		missCount.text = currentPlayData.missCount.ToString();
		mainScore.text = currentPlayData.mainScore.ToString();
		comboBonusScore.text = currentPlayData.comboBonusScore.ToString();
		score.text = currentPlayData.score.ToString();

		allNoteCount.text = currentPlayData.allNoteCount.ToString();

		if(currentPlayData.maxRealCombo >= currentPlayData.allNoteCount)
		{
			fullCombo.enabled = true;
		}
		else
		{
			fullCombo.enabled = false;
		}

		if(!currentPlayData.isFailed)
		{
			if(currentPlayData.score >= 10000000)
			{
				grade.sprite = gradeImg[0];
			}
			else if(currentPlayData.score >= 950000)
			{
				grade.sprite = gradeImg[1];
			}
			else if(currentPlayData.score >= 900000)
			{
				grade.sprite = gradeImg[2];
			}
			else if(currentPlayData.score >= 850000)
			{
				grade.sprite = gradeImg[3];
			}
			else if(currentPlayData.score >= 800000)
			{
				grade.sprite = gradeImg[4];
			}
			else if(currentPlayData.score >= 700000)
			{
				grade.sprite = gradeImg[5];
			}
			else if(currentPlayData.score >= 600000)
			{
				grade.sprite = gradeImg[6];
			}
			else if(currentPlayData.score >= 500000)
			{
				grade.sprite = gradeImg[7];
			}
			else if(currentPlayData.score >= 0)
			{
				grade.sprite = gradeImg[8];
			}
		}
		StartCoroutine(setIsAnimating());
	}

	IEnumerator setIsAnimating()
	{
		yield return new WaitForSeconds(2.1f);
		isAnimating = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Submit") && !isAnimating)
		{
			if(GameManager.stage == 3 || (GameManager.stage == 2 && (GameManager.playDataList[0].isFailed || GameManager.playDataList[1].isFailed)))
			{
				GameManager.changeScene("AllResult");

			}
			else
			{
				GameManager.stage++;
				GameManager.changeScene("MusicSelect");
			}
		}
	}
}
