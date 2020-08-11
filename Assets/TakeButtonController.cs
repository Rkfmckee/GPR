using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakeButtonController : MonoBehaviour
{
    #region Properties

    private Button takeButton;
    private CaveInventoryController inventoryController;

    #endregion

    #region Events

    private void Awake() {
        inventoryController = GetComponentInParent<CaveInventoryController>();
        takeButton = GetComponent<Button>();
        takeButton.onClick.AddListener(TakeButtonClicked);
    }

    #endregion

    #region Methods

    private void TakeButtonClicked() {
        GameObject itemSelected = inventoryController.GetSelectedItem();

        if (itemSelected != null) {
            GameObject newItem = Instantiate(itemSelected);
            PickUpController newItemPickup = newItem.GetComponent<PickUpController>();

            if (newItemPickup != null) {
                newItemPickup.SetCurrentState(PickUpController.State.Held);
            }

            References.gameController.GetComponent<GameController>().ShouldShowCaveInventory(false);
        }
    }

    #endregion
}
