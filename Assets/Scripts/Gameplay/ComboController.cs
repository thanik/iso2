using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ComboController : MonoBehaviour {

	private int combo = 0;
	private int realCombo = 0;
	private int maxCombo = 0;
	private int realMaxCombo = 0;

	public Font perfectFont;
	public Font greatFont;
	public Font fineFont;
	public Font missFont;

	public Text comboText;

	// Use this for initialization
	void Start () {
		combo = 0;
	}
	
	// Update is called once per frame
	void Update () {
		comboText.text = combo.ToString();
	}

	private void playComboIncreaseAnimation()
	{
		
	}

	private void playBreakSound()
	{

	}

	public void setFont(string fontName)
	{
		if(fontName == "perfect")
		{
			comboText.font = perfectFont;
		}
		else if(fontName == "great")
		{
			comboText.font = greatFont;
		}
		else if(fontName == "fine")
		{
			comboText.font = fineFont;
		}
		else if(fontName == "miss")
		{
			comboText.font = missFont;
		}
	}

	#region Combo fetching and updating functions
	public void addCombo(bool addReal)
	{
		combo++;
		if(addReal) realCombo++;
		playComboIncreaseAnimation();
		if(combo > maxCombo) maxCombo = combo;
		if(realCombo > realMaxCombo) realMaxCombo = realCombo;
	}

	public int getCombo()
	{
		return combo;
	}

	public int getRealCombo()
	{
		return realCombo;
	}

	public int getMaxCombo()
	{
		return maxCombo;
	}

	public int getRealMaxCombo()
	{
		return realMaxCombo;
	}

	public void addRealCombo()
	{
		realCombo++;
		if(realCombo > realMaxCombo) realMaxCombo = realCombo;
	}

	public void resetCombo()
	{
		if(combo >= 10)
		{
			playBreakSound();
		}
		combo = 0;
		realCombo = 0;
	}
	#endregion
}
