using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorZone : MonoBehaviour {

	public Transform endPos;
	Camera fadeObj;

	// Use this for initialization
	void Start () {
		fadeObj = Camera.main;
	}
	
	void OnTriggerEnter (Collider col) {
		if (col.gameObject.CompareTag ("Player")) {
			fadeObj.gameObject.GetComponent<CamFader>().CamFadeOut();
			Invoke ("ShiftPlayer", 0.6f);
		}
	}

	void ShiftPlayer()
	{
		fadeObj.gameObject.GetComponent<CamFader>().CamFadeIn();
		fadeObj.transform.root.position = endPos.position;
		fadeObj.transform.root.rotation = endPos.rotation;
	}
}
