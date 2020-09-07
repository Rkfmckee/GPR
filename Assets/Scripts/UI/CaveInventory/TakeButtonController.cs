using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakeButtonController : MonoBehaviour
{
    #region Properties

    private Button takeButton;
    private Image takeButtonImage;
    private CaveInventoryController inventoryController;
    private Sprite takeButtonUnpressed;
    private Sprite takeButtonPressed;

    #endregion

    #region Events

    private void Awake() {
        inventoryController = GetComponentInParent<CaveInventoryController>();
        takeButton = GetComponent<Button>();
        takeButton.onClick.AddListener(TakeButtonClicked);

        takeButtonImage = GetComponent<Image>();
        takeButtonUnpressed = Resources.Load<Sprite>("Images/UI/CaveInventory/TakeButton");
        takeButtonPressed = Resources.Load<Sprite>("Images/UI/CaveInventory/TakeButtonPressed");

        SetButtonImagePressed(true);
    }

    #endregion

    #region Methods

    public void SetButtonImagePressed(bool pressed) {
        if (pressed) {
            takeButtonImage.sprite = takeButtonPressed;
            takeButton.enabled = false;
        } else {
            takeButtonImage.sprite = takeButtonUnpressed;
            takeButton.enabled = true;
        }
    }

    private void TakeButtonClicked() {
        GameObject itemSelected = inventoryController.GetSelectedItem();
        CaveInventoryItemController itemInvController = itemSelected.GetComponent<CaveInventoryItemController>();

        if (itemSelected != null) {
            if (itemInvController.RemoveResourcesToSpawnItem()) {
                GameObject newItem = Instantiate(itemSelected);
                newItem.name = itemSelected.name;
                PickUpController newItemPickup = newItem.GetComponent<PickUpController>();

                if (newItemPickup != null) {
                    newItemPickup.SetCurrentState(PickUpController.State.Held);
                }

                AddNotificationOfNewItem(newItem.name);
                References.GameController.gameTraps.ShouldShowCaveInventory(false);
            }
        }
    }

    private void AddNotificationOfNewItem(string itemName) {
        string[] vowels = { "a", "e", "i", "o", "u" };
        string aOrAn = "a";

        foreach(string vowel in vowels) {
            if (itemName.ToLower().StartsWith(vowel)) {
                aOrAn = "an";
                break;
            }
        }

        References.UI.notifications.AddNotification($"Made {aOrAn} {itemName}");
    }

    #endregion
}
