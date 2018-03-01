using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticAnimation : MonoBehaviour {

	public bool randomImg;

	public Texture2D[] images = new Texture2D[]{};
	public GameObject plane;
	public float cooldown;

	private int _idx;
	private float _curTime;
	private MeshRenderer _rend;

	// Use this for initialization
	void Start () {
		_rend = plane.GetComponent<MeshRenderer> ();

		SetImage(_idx);
	}


	// Update is called once per frame
	void Update () {

		_curTime -= Time.deltaTime;
		if (_curTime < 0) {
			SetImage (_idx);
			_curTime = cooldown;
		}

	}

	void SetImage(int idx)
	{
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

