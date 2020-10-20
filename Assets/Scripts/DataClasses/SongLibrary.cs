using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class SongLibrary {
	public static List<SongData> database = new List<SongData>();

	public static void loadSongDatabase()
	{
		StringReader reader;
		TextAsset songDatabaseCSV = (TextAsset) Resources.Load("songdb", typeof(TextAsset));
		reader = new StringReader(songDatabaseCSV.text);
		if(reader == null)
		{
			Debug.LogError("Song Database doesn't exist!");
		}
		else
		{
			reader.ReadLine();
			string line;
			while((line = reader.ReadLine()) != null)
			{
				string[] entries = line.Split(',');
				if(entries.Length > 0)
				{
					SongData addedSong = new SongData();
					addedSong.songID = int.Parse(entries[0]);
					addedSong.songName = entries[1];
					addedSong.artistName = entries[2];
					addedSong.easyLv = int.Parse(entries[3]);
					addedSong.easyChartDesigner = entries[4];
					addedSong.easyChartFilename = entries[5];
					addedSong.normalLv = int.Parse(entries[6]);
					addedSong.normalChartDesigner = entries[7];
					addedSong.normalChartFilename = entries[8];
					addedSong.hardLv = int.Parse(entries[9]);
					addedSong.hardChartDesigner = entries[10];
					addedSong.hardChartFilename = entries[11];
					addedSong.masterLv = int.Parse(entries[12]);
					addedSong.masterChartDesigner = entries[13];
					addedSong.masterChartFilename = entries[14];
					addedSong.movieFilename = entries[15];
					addedSong.songFilename = entries[16];
					addedSong.songJacketFilename = entries[17];

					database.Add(addedSong);
				}
			}
		}
	}


}
