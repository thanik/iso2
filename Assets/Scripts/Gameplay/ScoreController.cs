using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreController : MonoBehaviour {

	public int score;
	public int deltaScore;
	public Text scoreText;

	/* animation */
	float pointAnimDurationSec = 2f;
	float pointAnimTimer = 0f;
	// var for storing the actual current score.
	// end point of the Lerp
	// var for storing the score just before points were last added
	// start point of the Lerp
	float savedDisplayedScore = 0;
	// a variable for the "animated" score you should show in the UI.
	// We can't put the result of Lerp into
	// the vars above because that would mess up the result of the next Lerp
	//int score = 0;
	// ^^ Added the word "displayed" because neither 
	// of these mark how many points th player really has.
	// Afterall you animate the score just to make it pretty

	// Use this for initialization
	void Start () {
		score = 0;
	}
	
	// Update is called once per frame
	void Update () {
		pointAnimTimer += Time.deltaTime * 1000;
		float prcComplete = pointAnimTimer / pointAnimDurationSec;
		score = Mathf.RoundToInt(Mathf.Lerp(savedDisplayedScore, score, prcComplete));
		scoreText.text = score.ToString();
	}

	/* this function sets score based on formula */
	public void setScore(int perfectCount, int goodCount, int nearCount, int noteCount)
	{
		float x = 950000/(float) noteCount;
		deltaScore = (Mathf.RoundToInt((x * (float) perfectCount) + (0.8f * x * (float) goodCount) + (0.3f * x * (float) nearCount))) - score;
		//score = Mathf.RoundToInt((x * (float) perfectCount) + (0.8f * x * (float) goodCount) + (0.3f * x * (float) nearCount));

		score += deltaScore;
		savedDisplayedScore = score;
		pointAnimTimer = 0f;
	}
}
