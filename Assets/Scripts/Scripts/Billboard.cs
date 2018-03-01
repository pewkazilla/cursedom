using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {
	
	public bool lockY = true;
	public Transform _lookAt;

	// Use this for initialization
	void Start () {
		_lookAt = Player.instance.transform;
	}
	
	// Update is called once per frame
	void Update () {
		Quaternion wantedRot = Quaternion.LookRotation (transform.position - _lookAt.position);

		if (lockY)
			transform.rotation = Quaternion.Euler (transform.eulerAngles.x, wantedRot.eulerAngles.y, transform.eulerAngles.z);
		else
			transform.rotation = wantedRot;
	}
}
