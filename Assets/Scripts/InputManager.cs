using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour {
    public GameObject dPadPrefab;
    public GameObject abPrefab;
    public GameObject canvas;
    public Sprite dPadLeftTex;
    public Sprite dPadRightTex;
    public Sprite dPadUpTex;
    public Sprite dPadDownTex;
    public Sprite dPadStaticTex;
    public Sprite aButtonTex;
    public Sprite bButtonTex;
    public Sprite abStaticTex;

    private GameObject dPadImage;
    private GameObject abImage;

	private static float deadZone = 0.3f;
	private static bool readFromVirtualAxis = true, pressForward, pressBack, pressLeft, pressRight;
    private static int xAxis;

    public static int XAxisTap {
        get {
            if (readFromVirtualAxis == false) {
                return XAxis;
            } else {
                int dir = 0;
				if (Input.GetAxisRaw("Horizontal") >= 1) {
					if (!pressRight) {
						++dir;
						pressRight = true;
					}
				} else if(Input.GetAxisRaw("Horizontal") < deadZone){
					if (pressRight) {
						pressRight = false;
					}
				}
				if (Input.GetAxisRaw("Horizontal") <= -1) {
					if (!pressLeft) {
						--dir;
						pressLeft = true;
					}
				} else if(Input.GetAxisRaw("Horizontal") > -deadZone){
					if (pressLeft) {
						pressLeft = false;
					}
				}
                return dir;
            }
        }
    }

    public static int YAxisTap {
        get {
            if (readFromVirtualAxis == false) {
                return YAxis;
            } else {
				int dir = 0;
				if (Input.GetAxisRaw("Vertical") >= 1) {
					if (!pressForward) {
						--dir;
						pressForward = true;
					}
				} else if(Input.GetAxisRaw("Vertical") < deadZone){
					if (pressForward) {
						pressForward = false;
					}
				}
				if (Input.GetAxisRaw("Vertical") <= -1) {
					if (!pressBack) {
						++dir;
						pressBack = true;
					}
				} else if(Input.GetAxisRaw("Vertical") > -deadZone){
					if (pressBack) {
						pressBack = false;
					}
				} 
                return dir;
            }
        }
    }

    public static int XAxis {
        get {
            var val = xAxis;
            xAxis = 0;
            return val;
        }
        set {
            readFromVirtualAxis = false;
            xAxis = value;
        }
    }
    private static int yAxis;
    public static int YAxis {
        get {
            var val = yAxis;
            yAxis = 0;
            return val;
        }
        set {
            readFromVirtualAxis = false;
            yAxis = value;
        }
    }
    private static bool bButton;
    public static bool BButton {
        get {
            var val = bButton;
            bButton = false;
            return val;
        }
        set {
            readFromVirtualAxis = false;
            bButton = value;
        }
    }
    private static bool aButton;
    public static bool AButton {
        get {
            var val = aButton;
            aButton = false;
            return val;
        }
        set {
            readFromVirtualAxis = false;
            aButton = value;
        }
    }
    private static bool startButton;
    public static bool StartButton {
        get {
            var val = startButton;
            startButton = false;
            return val;
        }
        set {
            readFromVirtualAxis = false;
            startButton = value;
        }
    }

    private void Start() {
        dPadImage = Instantiate(dPadPrefab, canvas.transform);
        abImage = Instantiate(abPrefab, canvas.transform);

        dPadImage.GetComponent<Image>().sprite = dPadStaticTex;
        abImage.GetComponent<Image>().sprite = abStaticTex;
    }

    private void Update() {
        // switch to keyboad input if a key is pressed
        if (Input.anyKey) {
            readFromVirtualAxis = true;
        }

        if (readFromVirtualAxis) {
            xAxis = (int)Input.GetAxisRaw("Horizontal");
            yAxis = (int)Input.GetAxisRaw("Vertical");

			aButton = Input.GetButtonDown("A_BTN");
			bButton = Input.GetButtonDown("B_BTN");
			startButton = Input.GetButtonDown("Start_BTN");
        }

        if (xAxis == -1) {
            dPadImage.GetComponent<Image>().sprite = dPadLeftTex;
        } else if (xAxis == 1) {
            dPadImage.GetComponent<Image>().sprite = dPadRightTex;
        } else if (yAxis == 1) {
            dPadImage.GetComponent<Image>().sprite = dPadUpTex;
        } else if (yAxis == -1) {
            dPadImage.GetComponent<Image>().sprite = dPadDownTex;
        } else {
            dPadImage.GetComponent<Image>().sprite = dPadStaticTex;
        }

        if (aButton) {
            abImage.GetComponent<Image>().sprite = aButtonTex;
        } else if (bButton) {
            abImage.GetComponent<Image>().sprite = bButtonTex;
        } else {
            abImage.GetComponent<Image>().sprite = abStaticTex;
        }
    }
}