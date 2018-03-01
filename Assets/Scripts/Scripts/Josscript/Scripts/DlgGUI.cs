using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class DlgGUI : DlgManager
{
	public GameObject UI;
	public Color colNorm, colHigh;
	public Text myText, yourText;
	public Text[] btnOption = new Text[]{};
	public int idxJaw, idxFHair, idxMouth, idxEyes, idxEyebrows, idxHair;
	public Image jaw, fhair, mouth, eyes, eyebrows, hair;
	public GameObject yourUI, buttons, menu;
	public Font standardFont, scaryFont, confusingFont, cuteFont, boldFont;

	public Sprite[] jawImages = new Sprite[]{};
	public Sprite[] fhairImages = new Sprite[]{};
	public Sprite[] mouthImages = new Sprite[]{};
	public Sprite[] eyesImages = new Sprite[]{};
	public Sprite[] eyebrowsImages = new Sprite[]{};
	public Sprite[] hairImages = new Sprite[]{};

	bool showingBtn = true, talking = false, showingUI = true;

	public GameObject lookAt;
	public AIBillboard talkingTo;
	public FirstPersonController cont;

	public void SetBoy()
	{
		idxJaw = 0;
		idxFHair = 0;
		idxMouth = 3;
		idxEyes = 7;
		idxEyebrows = 2;
		idxHair = 7;
	}

	public void SetGirl()
	{
		idxJaw = 2;
		idxFHair = -1;
		idxMouth = 1;
		idxEyes = 3;
		idxEyebrows = 1;
		idxHair = 0;
	}

	public void ShowUI()
	{
		LoadFace ();
		StartCoroutine ("Blink");
		txtChanged ();

		UI.SetActive (true);
		menu.SetActive (true);
	}

	// Use this for initialization
	void Start () {
		//Load();
		//Save ();

		DlgManager.GameKeys.Add ("Interaction", 0, "value");
		UI.SetActive (false);
		StartDialogue (215);
	}

	void Update()
	{
		HandleInput ();

		if (!HasEnded) {
			if (WhoIsSpeakingID != 0) {
				yourText.text = FormattedText;
			} else {
				myText.text = FormattedText;
			}

			if (!talking) {
				talking = true;
				cont.canMove = false;
			}


		} else if (talking) {
			talking = false;
			cont.canMove = true;
		}


		if (talking) {
			if (lookAt != null) {

				Quaternion rot = Quaternion.LookRotation (((lookAt.transform.position+new Vector3(0,0.5f,0))-transform.position).normalized, Vector3.up);
				cont.m_Camera.transform.rotation = Quaternion.Euler (
					Mathf.LerpAngle(cont.m_Camera.transform.eulerAngles.x, rot.eulerAngles.x, Time.deltaTime*3),
					Mathf.LerpAngle(cont.m_Camera.transform.eulerAngles.y, rot.eulerAngles.y, Time.deltaTime*3),
					Mathf.LerpAngle(cont.m_Camera.transform.eulerAngles.z, rot.eulerAngles.z, Time.deltaTime*3));
			}
		}

	}

	public void LoadFace()
	{
		jaw.sprite = jawImages [idxJaw];
		mouth.sprite = mouthImages [idxMouth];
		if (idxFHair != -1) {
			fhair.gameObject.SetActive (true);
			fhair.sprite = fhairImages [idxFHair];
		} else {
			fhair.gameObject.SetActive (false);
		}
		eyes.sprite = eyesImages [idxEyes];
		eyebrows.sprite = eyebrowsImages [idxEyebrows];
		if (idxHair != -1) {
			hair.gameObject.SetActive (true);
			hair.sprite = hairImages [idxHair];
		} else {
			hair.gameObject.SetActive (false);
		}

	}

	public void ColourButtons ()
	{
		foreach(Text txt in btnOption)
		{
			txt.color = colNorm;
		}
		btnOption[SelectedOption()].color = colHigh;
	}


	public void InitiateDialogue (GameObject targ, int overrideLine)
	{
		int lineToRead;
		if (overrideLine != 0)
			lineToRead = overrideLine;
		else
			lineToRead = targ.GetComponent<Interact> ().speakToIndex;

		if (targ.GetComponent<AIBillboard> () != null) {
			talkingTo =	targ.GetComponent<AIBillboard> ();
		}
		gameObject.BroadcastMessage ("StartDialogue", lineToRead);
		DlgManager.GameKeys.Seti ("Interaction", targ.gameObject.GetInstanceID ());
		lookAt = targ.gameObject;
		Debug.Log (DlgManager.GameKeys.Valuei ("Interaction", "value"));
		txtChanged ();
	}

	public override void HandleInput ()
	{
		if (HasEnded || State == DlgState.Inactive) {
			if (Input.GetKeyDown (KeyCode.E)) {
				GameObject targ = null;
				float range = 3;
				foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Useable")) {
					float dist = Vector3.Distance (transform.position, obj.transform.position);
					if (dist < range) {
						targ = obj;
						range = dist;
					}
				}

				if (targ != null) { 

					InitiateDialogue (targ, 0);
				}
			}
		} else {
			

			if (CurrentIsChoice) {
				if (Input.GetKeyDown (KeyCode.E)) {
					OptionSelect(SelectedOption());
					Speak ();
					txtChanged ();
				}
				if (Input.GetKeyDown (KeyCode.W)) {
					OptionPrevious ();
				}
				if (Input.GetKeyDown (KeyCode.S)) {
					OptionNext ();
				}
				ColourButtons ();
			} else {
				if (Input.GetKeyDown (KeyCode.E)) {
					Speak ();
					txtChanged ();
				}
			}
		}


	}

	public void CancelDialogue()
	{
		EndDialogue ();
		lookAt = null;
		txtChanged ();
	}

	public void txtChanged()
	{
		if (HasEnded || State == DlgState.Inactive) {
			if (showingBtn) {
				buttons.SetActive (false);
				showingBtn = false;
			}
			if (showingUI) {
				yourUI.SetActive (false);
				showingUI = false;
			}

			if (yourText.text != "")
				yourText.text = "";

			if (myText.text != "")
				myText.text = "";
		} else {
			if (CurrentIsChoice) {
				
				for(int i = 0; i<3;i++) {
					{
						if (i < CurrentLine.data.Count) {
							btnOption [i].gameObject.SetActive (true);
							btnOption [i].text = CurrentLine.data [i];
						} else
							btnOption [i].gameObject.SetActive (false);
					}

				}
				if (!showingBtn) {
					buttons.SetActive (true);
					showingBtn = true;
					myText.gameObject.SetActive (false);
				}
			} else {

				if (showingBtn) {
					buttons.SetActive (false);
					showingBtn = false;
					myText.gameObject.SetActive (true);
				}

			
				if (WhoIsSpeakingID != 0) {
					if(talkingTo != null)
					talkingTo.SetCycle ();

					switch (WhoIsSpeakingID) {
					case 1:
						yourText.font = standardFont; 
						break;

					case 2:
						yourText.font = scaryFont; 
						break;

					case 3:
						yourText.font = cuteFont; 
						break;

					case 4:
						yourText.font = confusingFont; 
						break;

					case 5:
						yourText.font = boldFont; 
						break;


					}

					if (!showingUI) {
						yourUI.SetActive (true);
						showingUI = true;
					}
				} else {


					if (showingUI) {
						yourUI.SetActive (false);
						showingUI = false;
					}
				}
			}
		}


	}


	public IEnumerator Blink()
	{
		int idx = idxEyes;

		yield return new WaitForSeconds (0.1f);

		eyes.sprite = eyesImages [idx-1];

		yield return new WaitForSeconds (0.1f);

		eyes.sprite = eyesImages [idx-2];

		yield return new WaitForSeconds (0.1f);

		eyes.sprite = eyesImages [idx-3];

		yield return new WaitForSeconds (0.1f);

		eyes.sprite = eyesImages [idx-2];

		yield return new WaitForSeconds (0.1f);

		eyes.sprite = eyesImages [idx-1];

		yield return new WaitForSeconds (0.1f);

		eyes.sprite = eyesImages [idx];

		float r = Random.Range (0.3f, 6);

		yield return new WaitForSeconds (r);

		StartCoroutine ("Blink");

	}


	void Save()
	{
		Invoke ("Save", 30f);
		DlgManager.GameKeys.Save("Josskeys",true);
	}
	void Load()
	{
		DlgManager.GameKeys.Load("Josskeys");
	}
}