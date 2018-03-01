using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsItem : MenuItemController {
    public override void OnSelect() {
        ItemManager.Instance.Select();
    }

    protected override void OnEnterTrigger() {
        ItemManager.Instance.ActivateMenu();
    }

    protected override void OnExitTrigger() {
        ItemManager.Instance.DeactivateMenu();
    }
}
