using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBillboard : MonoBehaviour {

	public bool randomImg, lockY = true, canRotate = true;

	public Texture2D[] images = new Texture2D[]{};
	public Texture2D backImage;
	public GameObject plane;
	public float cooldown;
	public int maxCycle, minCycle;

	private Transform _lookAt;
	private bool _facingBack;
	private int _idx, _toCycle;
	private float _curTime;
	private MeshRenderer _rend;

	// Use this for initialization
	void Start () {
		_rend = plane.GetComponent<MeshRenderer> ();

		SetImage(_idx);
	
		_lookAt = Camera.main.transform;
	}

	public void SetCycle()
	{
		_toCycle = Random.Range (minCycle, maxCycle);
	}

	// Update is called once per frame
	void Update () {
		if (canRotate) {

			Quaternion wantedRot = Quaternion.LookRotation (transform.position - _lookAt.position);

			if (lockY)
				plane.transform.rotation = Quaternion.Euler (plane.transform.eulerAngles.x, wantedRot.eulerAngles.y, plane.transform.eulerAngles.z);
			else
				plane.transform.rotation = wantedRot;
		}

		float dot = Vector3.Dot ((transform.position - _lookAt.position).normalized, transform.forward);
		if (dot > 0) {

			if (_toCycle > 0) {
				_curTime -= Time.deltaTime;
				if (_curTime < 0) {
					SetImage (_idx);
					_toCycle--;
					_curTime = cooldown;
				}
			} else {
				if(_facingBack)
					SetImage (_idx);
			}

		} else {
			if(!_facingBack)
			SetImage (-10);
		}

	}

	void SetImage(int idx)
	{
		if (idx == -10) {
			_rend.material.mainTexture = backImage;
			_facingBack = true;
		} else {
			_facingBack = false;
			_rend.material.mainTexture = images [idx];
			if (randomImg)
				_idx = Random.Range (0, images.Length-1);
			else
			_idx++;

			if (_idx == images.Length) {
				_idx = 0;
			
			}
		}
	}
}
