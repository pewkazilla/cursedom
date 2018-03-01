using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageSequence : MonoBehaviour {

	public Texture2D[] images = new Texture2D[]{};
	public bool randImg, looping = true, triPlanar;
	public float cooldown;

	private int _idx;
	private float _curTime;
	private MeshRenderer _rend;

	// Use this for initialization
	void Start () {
		_rend = GetComponent<MeshRenderer> ();
		if (randImg)
			_idx = Random.Range (0, images.Length);
		
		SetImage(_idx);
	}
	
	// Update is called once per frame
	void Update () {
		_curTime -= Time.deltaTime;
		if(_curTime < 0)
		{
			if (randImg)
				_idx = Random.Range (0, images.Length);
			SetImage (_idx);

			_curTime = cooldown;
		}


	}

	void SetImage(int idx)
	{
		if (!triPlanar)
			_rend.material.mainTexture = images [idx];
		else {
			_rend.material.SetTexture("_Top", images [idx]);
			_rend.material.SetTexture("_Side", images [idx]);
			_rend.material.SetTexture("_Bottom", images [idx]);
		}
		_idx++;

		if (_idx == images.Length) {
			if (looping)
				_idx = 0;
			else
				Destroy (transform.parent.root.gameObject);
		}
	}

}
