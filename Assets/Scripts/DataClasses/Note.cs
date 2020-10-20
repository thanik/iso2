using System;
[System.Serializable]
public class Note
{
	public float time;
	public int lane;
	public float length = 0;
	public int noteRenderType = 0; /* 0 for normal note, 1 for simutanously note, 2 for hold note */

	public Note(float noteTime, int noteLane, float noteLength, int renderType)
	{
		time = noteTime;
		lane = noteLane;
		length = noteLength;
		noteRenderType = renderType;
	}
}