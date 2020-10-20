using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class NotesData {

	/*public int level;
	public string noteDesigner;
	public int difficulty;*/

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
					if(entries.Length > 3) /* latest format csv notes file with notesprite defined */
					{
						/* not implemented */
					}
					else if(entries.Length == 3) /* newer format csv notes file with hold notes */
					{
						AddNote(float.Parse(entries[0]), int.Parse(entries[1]), float.Parse(entries[2]), 0, listOfNotes);
					}
					else if(entries.Length == 2) /* old format csv notes file */
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
