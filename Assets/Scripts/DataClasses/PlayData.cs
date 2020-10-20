using System;
using System.Collections;


[System.Serializable]
public class PlayData {

	public string playerName;
	public int playerID;
	public int songID;
	public int difficulty;
	public int perfectCount;
	public int greatCount;
	public int fineCount;
	public int missCount;
	public int allNoteCount;
	public int maxCombo;
	public int maxRealCombo;
	public int score;
	public int mainScore;
	public int comboBonusScore;
	public DateTime recordedTime;
	public int shopID;
	public bool isFailed;
}
