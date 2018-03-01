using UnityEngine;
using System.Linq;

public class MenuController : MonoBehaviour {
    public static MenuController Instance { get; private set; }
    public bool Active { get; private set; }

    public GameObject selector;

    private GameObject buttonParent;
    private MenuItemController[] buttons;
    private int selectedButton = 0;

    private Vector3 selectorLocalPos;

    void Start() {
        Instance = this;

        // find the button objects
        buttons = GetComponentsInChildren<MenuItemController>();
		if (buttons.Length > 0) {
			buttonParent = buttons [0].transform.parent.gameObject;
			Debug.LogWarning("Can't find any buttons on " + gameObject.name + "! Menu Controller might not work properly D:");
			// start with the menu disabled}
			buttonParent.SetActive (false);
		}
		Active = false;

        selectorLocalPos = selector.transform.localPosition;
    }

    public void ActivateMenu() {
        Active = true;
        selectedButton = 0;
        PlayerControl.instance.PlayerCantMove();
        buttonParent.SetActive(Active);

        UpdateSelector();

        buttons[selectedButton].OnEnter();
    }

    public void DeactivateMenu() {
        buttons[selectedButton].OnExit();

        Active = false;
        PlayerControl.instance.PlayerCanMove();
        buttonParent.SetActive(Active);
    }

    private void Update() {
        // toggle the menu
        if (InputManager.StartButton) {
            // turn the menu on
            if (!Active) {
                // check there isn't a dialogue open
                if (PlayerControl.instance.PlayerMovable) {
                    ActivateMenu();
                }
            } else {
                DeactivateMenu();
            }
        }

        if (Active) {
            // scroll left and right
            int nextButton = selectedButton + InputManager.XAxisTap;
            if (selectedButton != nextButton) {
                // bounds checking
                nextButton = nextButton % buttons.Length;
                if (nextButton < 0) {
                    nextButton = buttons.Length - 1;
                }

                // trigger events
                buttons[selectedButton].OnExit();
                buttons[nextButton].OnEnter();

                selectedButton = nextButton;
                UpdateSelector();
            }

            if (InputManager.AButton) {
                buttons[selectedButton].OnSelect();
            }
        }
    }

    private void UpdateSelector() {
        selector.transform.SetParent(buttons[selectedButton].transform);
        selector.transform.localPosition = selectorLocalPos;
    }
}
