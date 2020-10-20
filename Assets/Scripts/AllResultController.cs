using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class AllResultController : MonoBehaviour {

	public Canvas resultCanvas;
	public List<GameObject> results;

	public List<Sprite> gradeImg;

	private bool isAnimating = true;

	// Use this for initialization
	void Start () {
		GameManager.gameState = 6;

		/* assign values */
		for(int i = 0; i < GameManager.playDataList.Count; i++)
		{
			results[i].transform.Find("SongTitle").GetComponent<Text>().text = SongLibrary.database[GameManager.playDataList[i].songID].songName;
			results[i].transform.Find("artist").GetComponent<Text>().text = SongLibrary.database[GameManager.playDataList[i].songID].artistName;
			results[i].transform.Find("songArt").GetComponent<Image>().sprite = (Sprite) Resources.Load("SongArt/" + SongLibrary.database[GameManager.playDataList[i].songID].songJacketFilename, typeof(Sprite));

			string starsString = "";
			switch(GameManager.playDataList[i].difficulty)
			{
			case 0:
				results[i].transform.Find("difficulty").GetComponent<Text>().text = "Easy";
				for(int j = 0; j < SongLibrary.database[GameManager.playDataList[i].songID].easyLv; j++)
				{
					starsString += "★";
				}
				results[i].transform.Find("stars").GetComponent<Text>().text = starsString;
				break;
			case 1:
				results[i].transform.Find("difficulty").GetComponent<Text>().text = "Normal";

				for(int j = 0; j < SongLibrary.database[GameManager.playDataList[i].songID].normalLv; j++)
				{
					starsString += "★";
				}
				results[i].transform.Find("stars").GetComponent<Text>().text = starsString;
				results[i].transform.Find("stars").GetComponent<Text>().color = new Color(0.05098039216f,0.4117647059f,0.7490196078f);
				break;
			case 2:
				results[i].transform.Find("difficulty").GetComponent<Text>().text = "Hard";

				for(int j = 0; j < SongLibrary.database[GameManager.playDataList[i].songID].hardLv; j++)
				{
					starsString += "★";
				}
				results[i].transform.Find("stars").GetComponent<Text>().text = starsString;
				results[i].transform.Find("stars").GetComponent<Text>().color = new Color(0.8823529412f,0.1333333333f,0.1333333333f);
				break;

			case 3:

				results[i].transform.Find("difficulty").GetComponent<Text>().text = "Master";

				for(int j = 0; j < SongLibrary.database[GameManager.playDataList[i].songID].masterLv; j++)
				{
					starsString += "★";
				}
				results[i].transform.Find("stars").GetComponent<Text>().text = starsString;
				results[i].transform.Find("stars").GetComponent<Text>().color = new Color(0.5098039216f,0f,0.6980392157f);
				break;
			}

			results[i].transform.Find("maxRealCombo").GetComponent<Text>().text = GameManager.playDataList[i].maxRealCombo.ToString();
			results[i].transform.Find("perfectCount").GetComponent<Text>().text = GameManager.playDataList[i].perfectCount.ToString();
			results[i].transform.Find("greatCount").GetComponent<Text>().text = GameManager.playDataList[i].greatCount.ToString();
			results[i].transform.Find("fineCount").GetComponent<Text>().text = GameManager.playDataList[i].fineCount.ToString();
			results[i].transform.Find("missCount").GetComponent<Text>().text = GameManager.playDataList[i].missCount.ToString();
			results[i].transform.Find("score").GetComponent<Text>().text = GameManager.playDataList[i].score.ToString();

			results[i].transform.Find("allNoteCount").GetComponent<Text>().text = GameManager.playDataList[i].allNoteCount.ToString();

			if(GameManager.playDataList[i].maxRealCombo >= GameManager.playDataList[i].allNoteCount)
			{
				results[i].transform.Find("fullCombo").GetComponent<Image>().enabled = true;
			}
			else
			{
				results[i].transform.Find("fullCombo").GetComponent<Image>().enabled = false;
			}

			if(!GameManager.playDataList[i].isFailed)
			{
				if(GameManager.playDataList[i].score >= 10000000)
				{
					results[i].transform.Find("grade").GetComponent<Image>().sprite = gradeImg[0];
				}
				else if(GameManager.playDataList[i].score >= 950000)
				{
					results[i].transform.Find("grade").GetComponent<Image>().sprite = gradeImg[1];
				}
				else if(GameManager.playDataList[i].score >= 900000)
				{
					results[i].transform.Find("grade").GetComponent<Image>().sprite = gradeImg[2];
				}
				else if(GameManager.playDataList[i].score >= 850000)
				{
					results[i].transform.Find("grade").GetComponent<Image>().sprite = gradeImg[3];
				}
				else if(GameManager.playDataList[i].score >= 800000)
				{
					results[i].transform.Find("grade").GetComponent<Image>().sprite = gradeImg[4];
				}
				else if(GameManager.playDataList[i].score >= 700000)
				{
					results[i].transform.Find("grade").GetComponent<Image>().sprite = gradeImg[5];
				}
				else if(GameManager.playDataList[i].score >= 600000)
				{
					results[i].transform.Find("grade").GetComponent<Image>().sprite = gradeImg[6];
				}
				else if(GameManager.playDataList[i].score >= 500000)
				{
					results[i].transform.Find("grade").GetComponent<Image>().sprite = gradeImg[7];
				}
				else if(GameManager.playDataList[i].score >= 0)
				{
					results[i].transform.Find("grade").GetComponent<Image>().sprite = gradeImg[8];
				}
			}
			else
			{
				results[i].transform.Find("grade").GetComponent<Image>().sprite = gradeImg[8];
			}
		}

		if(GameManager.stage == 2)
		{
			resultCanvas.GetComponent<Animator>().Play("allGradeResultMoveIn2");
		}
		else
		{
			resultCanvas.GetComponent<Animator>().Play("allGradeResultMoveIn");
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
			GameManager.changeScene("GameOver");
		}
	}
}
