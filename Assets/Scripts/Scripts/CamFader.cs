using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFader : MonoBehaviour {


		public GameObject fadeObj, audioObj;
	public AudioClip doorSound;

	private float audioVol, audioPitch;



	// Use this for initialization
	void Start () {
		if (fadeObj == null)
			fadeObj = gameObject;
		
		if (fadeObj.GetComponent<AudioSource> () != null) {
			audioVol = fadeObj.GetComponent<AudioSource> ().volume;
			audioPitch = fadeObj.GetComponent<AudioSource> ().pitch;

		}

		CamFadeIn ();
	}

	public void CamFadeOut()
	{
		iTween.CameraFadeTo (1f, 1.5f);
		//iTween.AudioTo (audioObj, 0, audioPitch, 1.5f);
	}

	public void CamFadeIn()
	{
		iTween.CameraFadeFrom (1, 0.0001f);
		iTween.CameraFadeTo (0f, 1.5f);
		//iTween.AudioTo (audioObj, audioVol, audioPitch, 1.5f);
		AudioSource.PlayClipAtPoint (doorSound, transform.position);

	}



}
