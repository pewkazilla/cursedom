using UnityEngine.UI;
using UnityEngine;

public class ItemManager : MenuItemController {
    public string[] items;

    public static ItemManager Instance { get; private set; }

    private Text itemName;
    private Text itemDesc;
    private Text itemValue;
    private Text itemCount;

    // Can't call setup in awake; wait for update
    private bool setup = false;
    private int selectedItem = 0;

    private void Awake() {
        Instance = this;

        // this sucks, sorry
        itemName = GameObject.Find("Item Name").GetComponent<Text>();
        itemDesc = GameObject.Find("Item Description").GetComponent<Text>();
        itemValue = GameObject.Find("Item Value").GetComponent<Text>();
        itemCount = GameObject.Find("Item Count").GetComponent<Text>();

        gameObject.SetActive(false);
    }

    public void ActivateMenu() {
        selectedItem = 0;
        gameObject.SetActive(true);
    }

    public void DeactivateMenu() {
        gameObject.SetActive(false);
    }
    
    // Does nothing for now
    public void Select() {

    }

    private void Update() {
        if (setup == false) {
            setup = true;
            UpdateItem();
        }

        // scroll up and down
        int nextItem = selectedItem + InputManager.YAxisTap;
        if (selectedItem != nextItem) {
            Debug.Log(items.Length);
            // bounds checking
            nextItem = nextItem % items.Length;
            if (nextItem < 0) {
                nextItem = items.Length - 1;
            }

            selectedItem = nextItem;
            UpdateItem();
        }
    }

    private void UpdateItem() {
        Item item;

        try {
            item = Item.Get(items[selectedItem]);
        } catch {
            Debug.Log("Unknown item " + items[selectedItem]);
            selectedItem = 0;
            return;
        }

        itemName.text = item.name;
        itemDesc.text = item.description;
        itemValue.text = "$" + item.value;
        itemCount.text = item.quantity.ToString();
    }
}
