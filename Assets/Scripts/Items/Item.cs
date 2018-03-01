using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    private static int itemCount = 0;
    private static Dictionary<string, Item> items;
    public static Item Get(string name) { return items[name]; }

    public int ID { get; private set; }
    public string itemName;
    public string description;
    public int quantity;
    public int value;
    // probably need icon??

    private void Start() {
        if (items == null) {
            items = new Dictionary<string, Item>();
        }

        ID = itemCount++;
        items.Add(itemName, this);
    }
}
