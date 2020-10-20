using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TutorialController : MonoBehaviour {

	public GameController gameController;
	public Image tutorialImg;
	public float timeToTransitionImg;
	public List<Sprite> tutorialImgs;
	public List<float> imgsTime;
	public List<float> durations;

	private bool showing = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float currentTime = gameController.getCurrentFixedGameTime();

		for(int i = 0; i < tutorialImgs.Count; i++)
		{
			if(currentTime > imgsTime[i] && currentTime < imgsTime[i] + durations[i])
			{
				showDialog();
			}
			else if(currentTime > imgsTime[i] + durations[i] && currentTime < imgsTime[i] + durations[i] + timeToTransitionImg)
			{
				hideDialog();
			}
			else if(i+1 < tutorialImgs.Count)
			{
				if(currentTime > imgsTime[i+1] - timeToTransitionImg && currentTime < imgsTime[i+1])
				{
					tutorialImg.sprite = tutorialImgs[i+1];
				}
			}
		}


	}

	private void showDialog()
	{
		if(!showing)
		{
			tutorialImg.GetComponent<Animator>().Play("tutorialImgShowIn");
			showing = true;
		}
	}

	private void hideDialog()
	{
		if(showing)
		{
			tutorialImg.GetComponent<Animator>().Play("tutorialImgShowOut");
			showing = false;
		}
	}
}
