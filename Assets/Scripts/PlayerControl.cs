using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class's existence is a bit weird
public class PlayerControl : MonoBehaviour {
    public static PlayerControl instance;
    public bool PlayerMovable { get { return Player.instance.PlayerInControl; } }

    private void Awake()
    {
        instance = this;
    }

    public void PlayerCantMove()
    {
        Player.instance.PlayerInControl = false;
    }
    public void PlayerCanMove()
    {
        Player.instance.PlayerInControl = true;
    }
}
