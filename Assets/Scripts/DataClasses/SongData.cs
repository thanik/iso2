using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class SongData {

	public int songID;
	public string songName;
	public string artistName;
	public string songFilename;
	public int easyLv;
	public string easyChartDesigner;
	public string easyChartFilename;
	public int normalLv;
	public string normalChartDesigner;
	public string normalChartFilename;
	public int hardLv;
	public string hardChartDesigner;
	public string hardChartFilename;
	public int masterLv;
	public string masterChartDesigner;
	public string masterChartFilename;
	public string movieFilename;
	public string songJacketFilename;

	public static void loadSongData(string fileName, List<Note> listOfNotes)
	{
		StringReader reader;
		TextAsset noteData = (TextAsset) Resources.Load("NoteData/" + fileName, typeof(TextAsset));
		reader = new StringReader(noteData.text);
		if(reader == null)
		{
			Debug.LogError("Note Data doesn't exist!");
		}
		else
		{
			string line;
			while((line = reader.ReadLine()) != null)
			{
				string[] entries = line.Split(',');
				if(entries.Length > 0)
				{
					if(entries.Length > 2)
					{
						AddNote(float.Parse(entries[0]), int.Parse(entries[1]), float.Parse(entries[2]), 0, listOfNotes);
					}
					else
					{
						AddNote(float.Parse(entries[0]), int.Parse(entries[1]), 0f, 0, listOfNotes);
					}
				}
			}
		}
	}

	private static void AddNote(float noteTime, int noteLane, float noteLength, int noteRenderType, List<Note> listOfNotes)
	{
		Note newNote = new Note(noteTime, noteLane, noteLength, noteRenderType);
		listOfNotes.Add(newNote);

	}
}
