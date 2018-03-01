using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour {

	public Transform endPos;

	// Use this for initialization
	public void OpenDoor () {
		Transform player = GameObject.FindGameObjectWithTag ("Player").transform;
		player.position = endPos.position;
		player.rotation = endPos.rotation;
	}

}
