using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnimateTex : MonoBehaviour {

	public Image matRenderer;
	public float xSpeed, ySpeed;
	private Material _mat;

	// Use this for initialization
	void Start () {
		_mat = matRenderer.material;
	}
	
	// Update is called once per frame
	void Update () {
		_mat.mainTextureOffset += new Vector2 (xSpeed * Time.deltaTime, ySpeed * Time.deltaTime);

		if (_mat.mainTextureOffset.x > 1 || _mat.mainTextureOffset.x < -1)
			_mat.mainTextureOffset = new Vector2( 0, _mat.mainTextureOffset.y );
		if (_mat.mainTextureOffset.y > 1 || _mat.mainTextureOffset.y < -1)
			_mat.mainTextureOffset = new Vector2( _mat.mainTextureOffset.y, 0);
	}
}
