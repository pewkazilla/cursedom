using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour {
    public enum ButtonType {
        A, B, Start, Left, Right, Up, Down
    };

    public ButtonType type;

    private void Start() {
        var button = GetComponent<Button>();

        button.onClick.AddListener(() => {
            switch (type) {
                case ButtonType.A:
                    InputManager.AButton = true;
                    break;
                case ButtonType.B:
                    InputManager.BButton = true;
                    break;
                case ButtonType.Start:
                    InputManager.StartButton = true;
                    break;
                case ButtonType.Up:
                    InputManager.YAxis = 1;
                    break;
                case ButtonType.Down:
                    InputManager.YAxis = -1;
                    break;
                case ButtonType.Left:
                    InputManager.XAxis = -1;
                    break;
                case ButtonType.Right:
                    InputManager.XAxis = 1;
                    break;
            }
        });
    }
}