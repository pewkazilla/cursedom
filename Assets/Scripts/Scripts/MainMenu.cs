using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public AudioClip open, close;

	void Start () {


		iTween.CameraFadeAdd ();	

		CamFadeIn ();
	}

	public void CamFadeOut(float time)
	{
		iTween.CameraFadeTo (1f, time);

	}

	public void CamFadeIn()
	{
		iTween.CameraFadeFrom (1, 0.0001f);
		iTween.CameraFadeTo (0f, 1.5f);

	}

	public void ExitGame()
	{
		AudioSource.PlayClipAtPoint (close, transform.position);
		CamFadeOut (1.5f);
		Invoke ("Exit", 1.5f);
	}

	public void StartGame()
	{
		AudioSource.PlayClipAtPoint (open, transform.position);
		CamFadeOut (4);
		Invoke ("Open", 2f);
	}

	void Exit()
	{
		Application.Quit ();
	}
	void Open()
	{
		SceneManager.LoadScene("pewkascene");
	}

}
