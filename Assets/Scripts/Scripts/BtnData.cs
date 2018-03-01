using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnData : MonoBehaviour {

	public int idxJaw = -1, idxFHair = -1, idxMouth = -1, idxEyes = -1, idxEyebrows = -1, idxHair = -1;
	public string name, desc;
	public bool owned;

	public void ApplyChanges ()
	{
		DlgGUI ui = GameObject.FindGameObjectWithTag ("Player").GetComponent<DlgGUI> ();
		if (idxJaw != -1)
			ui.idxJaw = idxJaw;
		if (idxFHair != -1)
			ui.idxFHair = idxFHair;
		if (idxMouth != -1)
			ui.idxMouth = idxMouth;
		if (idxEyes != -1)
			ui.idxEyes = idxEyes;
		if (idxEyebrows != -1)
			ui.idxEyebrows = idxEyebrows;
		if (idxHair != -1)
			ui.idxHair = idxHair;
		ui.LoadFace ();

	}
}
