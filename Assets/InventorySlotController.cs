using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotController : MonoBehaviour
{
    #region Properties

    public GameObject itemInSlot;

    private Button itemButton;

    #endregion

    #region Events

    private void Awake() {
        itemButton = transform.Find("ItemButton").GetComponent<Button>();

        if (itemButton != null) {
            itemButton.onClick.AddListener(ItemButtonClicked);
        }
    }

    #endregion

    #region Methods

    private void ItemButtonClicked() {
        print($"Spawned {itemInSlot.name}.");
    }

    #endregion
}
