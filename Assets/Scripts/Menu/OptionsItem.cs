using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsItem : MenuItemController {
    private OptionsMenuController menu;

    private void Awake() {
        menu = GetComponentInChildren<OptionsMenuController>();
    }

    public override void OnSelect() {
        menu.Select();
    }

    protected override void OnEnterTrigger() {
        menu.ActivateMenu();
    }

    protected override void OnExitTrigger() {
        menu.DeactivateMenu();
    }
}
