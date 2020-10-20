using UnityEngine;
using System.Collections;
using UnityEngine.Video;

public class GameVideoPlayer : MonoBehaviour {

	private static GameVideoPlayer _instance;
	private static VideoPlayer vidPlayer;
	void Start()
	{
		//if (Application.platform == RuntimePlatform.WindowsPlayer)
		//{
		//	this.transform.localScale = new Vector3(17.8454f,10.0468f,1f);
		//}
		vidPlayer = GetComponent<VideoPlayer>();
	}

	void Awake()
	{
		if(!_instance)
		{
			_instance = this;
		}
		else
		{
			Destroy(this.gameObject);
		}
		DontDestroyOnLoad(this.gameObject);
	}

	public static void insertVideo(string movieFilename,bool looping)
	{
		//((MovieTexture) _instance.GetComponent<MeshRenderer>().material.mainTexture).Stop();
		//movie.loop = looping;
		//_instance.GetComponent<MeshRenderer>().material.mainTexture = movie;

		//_instance.GetComponent<VideoPlayer>().OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, movieFilename);
		vidPlayer.source = VideoSource.Url;
		vidPlayer.url = Application.streamingAssetsPath + "/" + movieFilename;
		vidPlayer.isLooping = looping;
		//_instance.GetComponent<VideoPlayer>().clip;
		//_instance.GetComponent<MediaPlayer>().Control.SetLooping(looping);

	}

	public static void seekVideo(float time)
	{
		vidPlayer.time = time;
	}

	public static void setLooping(bool looping)
	{
		vidPlayer.isLooping = looping;
	}

	public static void Play() {
		vidPlayer.Play();
	}

	public static void Stop() {
		vidPlayer.Stop();
	}

	public static void Pause() {
		vidPlayer.Pause();
	}
		

	public static void Disable() {
		vidPlayer.Stop();
		_instance.gameObject.SetActive(false);
	}

	public static void Enable()
	{
		_instance.gameObject.SetActive(true);
	}

	public static void setMaterial(string material)
	{
		if(material == "movie")
		{
			_instance.GetComponent<MeshRenderer>().material = (Material) Resources.Load("Materials/movie");
		}
		else if(material == "menubg")
		{
			_instance.GetComponent<MeshRenderer>().material = (Material) Resources.Load("Materials/menubg");
		}
	}
}
