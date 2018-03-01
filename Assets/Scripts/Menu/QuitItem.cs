using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitItem : MenuItemController {
    public override void OnSelect() {
        Application.Quit();
    }
}
