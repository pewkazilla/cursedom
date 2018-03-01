using UnityEngine;
using System.Linq;
using System;

public class OptionsMenuController : MonoBehaviour {
    public static OptionsMenuController Instance { get; private set; }

    public GameObject selector;
    
    private OptionsMenuItem[] buttons;

    public void Select() {
        buttons[selectedButton].OnSelect();
    }

    private int selectedButton = 0;

    private Vector3 selectorLocalPos;

    void Start() {
        Instance = this;

        // find the button objects
        buttons = GetComponentsInChildren<OptionsMenuItem>();

        // start with the menu disabled}
        gameObject.SetActive(false);

        selectorLocalPos = selector.transform.localPosition;
    }

    public void ActivateMenu() {
        selectedButton = 0;
        gameObject.SetActive(true);

        UpdateSelector();
    }

    public void DeactivateMenu() {
        gameObject.SetActive(false);
    }

    private void Update() {
        // scroll up and down
        int nextButton = selectedButton + InputManager.YAxisTap;
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

    private void UpdateSelector() {
        selector.transform.SetParent(buttons[selectedButton].transform);
        selector.transform.localPosition = selectorLocalPos;
    }
}
