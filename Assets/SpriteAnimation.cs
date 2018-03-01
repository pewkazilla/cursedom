using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimation : MonoBehaviour {

	public bool randomImg;

	public Sprite[] images = new Sprite[]{};
	public GameObject plane;
	public float cooldown;

	private int _idx;
	private float _curTime;
	private SpriteRenderer _rend;

	// Use this for initialization
	void Start () {
		_rend = plane.GetComponent<SpriteRenderer> ();

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
		_rend.sprite = images [idx];
		if (randomImg)
			_idx = Random.Range (0, images.Length-1);
		else
			_idx++;

		if (_idx == images.Length) {
			_idx = 0;

		}
	}
}

