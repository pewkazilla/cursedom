using UnityEngine;
using System.Collections;

public class MusicQueue : MonoBehaviour {

	private bool _playingIntro;

	public AudioSource audioObj;
	public int curSong = -1;
	public bool[] playIntro;
	public AudioClip[] songLoops;
	public AudioClip[] songIntros;

	void Start()
	{
		LoadNextSong (true);
	}

	// Update is called once per frame
	void Update () {
		if (_playingIntro) {
			if (!audioObj.isPlaying)
				LoadNextSong (false);
		}
	}

	public void LoadNextSong(bool ShiftUp)
	{
		audioObj.Stop ();
		AudioClip clip;
		if(ShiftUp)
			curSong++;
		if (playIntro [curSong] && !_playingIntro) {
			clip = songIntros [curSong];
			_playingIntro = true;
			audioObj.loop = false;
		} else {
			clip = songLoops [curSong];
			_playingIntro = false;
			audioObj.loop = true;
		}
		audioObj.clip = clip;
		audioObj.Play ();
	}
}
