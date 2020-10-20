using UnityEngine;
using System.Collections;

public class NoteMovementRight : MonoBehaviour {

	public int lane;
	private GameController gameController;
	private SpriteRenderer spriteRenderer;

	void Start()
	{
		gameController = GameObject.Find("GameController").GetComponent<GameController>();
	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		if(gameController.isPlaying)
		{
			transform.Translate(new Vector3(-1,-0.5755f,0) * GameManager.speedModifier * Time.fixedDeltaTime * 3f);
		}
	}
}
