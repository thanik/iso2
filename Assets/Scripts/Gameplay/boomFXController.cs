using UnityEngine;
using System.Collections;

public class boomFXController : MonoBehaviour {

	public void playBoomAnimation()
	{
		GetComponent<Animator>().Play("boom_fx",-1,0f);
	}
}
