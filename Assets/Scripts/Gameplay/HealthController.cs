using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthController : MonoBehaviour {

	private int health;
	public GameController gameController;
	// Use this for initialization
	void Start () {
		health = 100;
	}
	
	// Update is called once per frame
	void Update () {
		if(health == 100)
		{
			transform.Find("H1").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H2").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H3").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H4").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H5").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H6").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H7").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H8").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H9").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H10").GetComponent<SpriteRenderer>().enabled = true;
		}
		else if(health >= 90 && health < 100)
		{
			transform.Find("H1").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H2").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H3").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H4").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H5").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H6").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H7").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H8").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H9").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H10").GetComponent<SpriteRenderer>().enabled = false;
		}
		else if(health >= 80 && health < 90)
		{
			transform.Find("H1").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H2").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H3").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H4").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H5").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H6").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H7").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H8").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H9").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H10").GetComponent<SpriteRenderer>().enabled = false;
		}
		else if(health >= 70 && health < 80)
		{
			transform.Find("H1").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H2").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H3").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H4").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H5").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H6").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H7").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H8").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H9").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H10").GetComponent<SpriteRenderer>().enabled = false;
		}
		else if(health >= 60 && health < 70)
		{
			transform.Find("H1").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H2").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H3").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H4").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H5").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H6").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H7").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H8").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H9").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H10").GetComponent<SpriteRenderer>().enabled = false;
		}
		else if(health >= 50 && health < 60)
		{
			transform.Find("H1").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H2").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H3").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H4").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H5").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H6").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H7").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H8").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H9").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H10").GetComponent<SpriteRenderer>().enabled = false;
		}
		else if(health >= 40 && health < 50)
		{
			transform.Find("H1").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H2").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H3").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H4").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H5").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H6").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H7").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H8").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H9").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H10").GetComponent<SpriteRenderer>().enabled = false;
		}
		else if(health >= 30 && health < 40)
		{
			transform.Find("H1").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H2").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H3").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H4").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H5").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H6").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H7").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H8").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H9").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H10").GetComponent<SpriteRenderer>().enabled = false;
		}
		else if(health >= 20 && health < 30)
		{
			transform.Find("H1").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H2").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H3").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H4").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H5").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H6").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H7").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H8").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H9").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H10").GetComponent<SpriteRenderer>().enabled = false;
		}
		else if(health >= 10 && health < 20)
		{
			transform.Find("H1").GetComponent<SpriteRenderer>().enabled = true;
			transform.Find("H2").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H3").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H4").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H5").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H6").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H7").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H8").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H9").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H10").GetComponent<SpriteRenderer>().enabled = false;
		}
		else if(health < 10)
		{
			transform.Find("H1").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H2").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H3").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H4").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H5").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H6").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H7").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H8").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H9").GetComponent<SpriteRenderer>().enabled = false;
			transform.Find("H10").GetComponent<SpriteRenderer>().enabled = false;
		}
	}

	#region Health fetching and updating functions
	public void decreaseHealth(int healthToDecrease)
	{
		if(health - healthToDecrease < 0)
		{
			//Game Over
			health = 0;
			gameController.GameOver();
		}
		else
		{
			health -= healthToDecrease;
		}
	}

	public void increaseHealth(int healthToIncrease)
	{
		if(health + healthToIncrease >= 100)
		{
			health = 100;
		}
		else
		{
			health += healthToIncrease;
		}
	}

	public void resetHealth()
	{
		health = 100;
	}

	public int getHealth()
	{
		return health;
	}
	#endregion
}
