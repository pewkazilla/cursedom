using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueItem : MenuItemController {
    public override void OnSelect() {
        MenuController.Instance.DeactivateMenu();
    }
}
