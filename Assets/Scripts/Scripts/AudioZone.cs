using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioZone : MonoBehaviour {

	public GameObject weatherObj;
	public AudioClip zoneClip;
	
	// Update is called once per frame
	void OnTriggerEnter (Collider col) {
		if (col.gameObject.CompareTag ("Player")) {
			
			if (GameObject.Find ("WEATHER") != null)
				Destroy (GameObject.Find ("WEATHER"));	

			if (weatherObj != null) {
				GameObject w = GameObject.Instantiate (weatherObj, col.transform.position, Quaternion.identity);
				w.name = "WEATHER";
			}

			AudioSource aud = col.gameObject.GetComponent<AudioSource> ();
			if (aud.clip.name != zoneClip.name) {
				aud.Stop ();
				aud.clip = zoneClip;
				aud.Play ();
			}
		}
	}
}
