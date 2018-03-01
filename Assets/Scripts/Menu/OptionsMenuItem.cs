using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuItem : MonoBehaviour {
    private bool selected = false;

    public void OnEnter() {
        OnEnterTrigger();
        selected = true;
    }
    public void OnExit() {
        OnExitTrigger();
        selected = false;
    }

    protected virtual void OnEnterTrigger() {
        Debug.Log("Entered " + name);
    }

    protected virtual void OnExitTrigger() {
        Debug.Log("Exited " + name);

    }

    public virtual void OnSelect() {
        Debug.Log("Selected " + name);
    }
}
