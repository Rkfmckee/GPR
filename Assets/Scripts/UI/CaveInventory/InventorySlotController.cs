using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotController : MonoBehaviour
{
    #region Properties

    public GameObject itemInSlot;

    private Button itemButton;
    private CaveInventoryController inventoryController;

    #endregion

    #region Events

    private void Awake() {
        inventoryController = GetComponentInParent<CaveInventoryController>();
        itemButton = transform.Find("ItemButton").GetComponent<Button>();

        if (itemButton != null) {
            itemButton.onClick.AddListener(ItemButtonClicked);
        }

        GameObject itemPrice = transform.Find("ItemButton").Find("ItemPrice").gameObject;
        itemPrice.transform.Find("ItemPriceText").GetComponent<Text>().text = itemInSlot.GetComponent<CaveInventoryItemController>().GetResourceCost().ToString();
    }

    #endregion

    #region Methods

    private void ItemButtonClicked() {
        inventoryController.SetSelectedItem(itemInSlot);
    }

    #endregion
}
