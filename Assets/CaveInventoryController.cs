using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaveInventoryController : MonoBehaviour
{
    #region Properties

    private GameObject itemSelected;
    private Button floorTrapButton;
    private Button wallTrapButton;
    private Button triggerButton;
    private GameObject floorTrapItems;
    private GameObject wallTrapItems;
    private GameObject triggerItems;

    #endregion

    #region Events

    private void Awake() {
        floorTrapButton = transform.Find("FloorTraps").GetComponent<Button>();
        wallTrapButton = transform.Find("WallTraps").GetComponent<Button>();
        triggerButton = transform.Find("Triggers").GetComponent<Button>();

        floorTrapButton.onClick.AddListener(() => ChangeCategory(Categories.FloorTrap));
        wallTrapButton.onClick.AddListener(() => ChangeCategory(Categories.WallTrap));
        triggerButton.onClick.AddListener(() => ChangeCategory(Categories.Trigger));

        Transform inventoryBackground = transform.Find("InventoryBackground");
        floorTrapItems = inventoryBackground.Find("FloorTrapItems").gameObject;
        wallTrapItems = inventoryBackground.Find("WallTrapItems").gameObject;
        triggerItems = inventoryBackground.Find("TriggerItems").gameObject;
    }

    #endregion

    #region Methods

    public GameObject GetSelectedItem() {
        return itemSelected;
    }

    public void SetSelectedItem(GameObject item) {
        itemSelected = item;
    }

    private void ChangeCategory(Categories categorySelected) {
        floorTrapItems.SetActive(false);
        wallTrapItems.SetActive(false);
        triggerItems.SetActive(false);

        switch (categorySelected) {
            case Categories.FloorTrap:
                floorTrapItems.SetActive(true);
                break;
            case Categories.WallTrap:
                wallTrapItems.SetActive(true);
                break;
            case Categories.Trigger:
                triggerItems.SetActive(true);
                break;
        }
    }

    #endregion

    #region Enums

    public enum Categories {
        FloorTrap,
        WallTrap,
        Trigger
    }

    #endregion
}
