using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaveInventoryController : MonoBehaviour
{
    #region Properties

    private Color titleInitialColour;
    private Color titleTransparentColor;

    private GameObject itemSelected;
    private GameObject floorTrapButton;
    private GameObject wallTrapButton;
    private GameObject triggerButton;
    private GameObject floorTrapItems;
    private GameObject wallTrapItems;
    private GameObject triggerItems;
    private GameObject takeButton;

    #endregion

    #region Events

    private void Awake() {
        takeButton = transform.Find("TakeButton").gameObject;

        floorTrapButton = transform.Find("FloorTraps").gameObject;
        wallTrapButton = transform.Find("WallTraps").gameObject;
        triggerButton = transform.Find("Triggers").gameObject;

        floorTrapButton.GetComponent<Button>().onClick.AddListener(() => ChangeCategory(Categories.FloorTrap));
        wallTrapButton.GetComponent<Button>().onClick.AddListener(() => ChangeCategory(Categories.WallTrap));
        triggerButton.GetComponent<Button>().onClick.AddListener(() => ChangeCategory(Categories.Trigger));

        Transform inventoryBackground = transform.Find("InventoryBackground");
        floorTrapItems = inventoryBackground.Find("FloorTrapItems").gameObject;
        wallTrapItems = inventoryBackground.Find("WallTrapItems").gameObject;
        triggerItems = inventoryBackground.Find("TriggerItems").gameObject;

        titleInitialColour = floorTrapButton.GetComponent<Image>().color;
        titleTransparentColor = titleInitialColour;
        titleTransparentColor.a = 0.5f;

        wallTrapButton.GetComponent<Image>().color = titleTransparentColor;
        triggerButton.GetComponent<Image>().color = titleTransparentColor;
    }

    #endregion

    #region Methods

    public GameObject GetSelectedItem() {
        return itemSelected;
    }

    public void SetSelectedItem(GameObject item) {
        itemSelected = item;

        if (item != null) {
            takeButton.GetComponent<TakeButtonController>().SetButtonImagePressed(false);
        } else {
            takeButton.GetComponent<TakeButtonController>().SetButtonImagePressed(true);
        }
    }

    private void ChangeCategory(Categories categorySelected) {
        floorTrapItems.SetActive(false);
        floorTrapButton.GetComponent<Image>().color = titleTransparentColor;
        wallTrapItems.SetActive(false);
        wallTrapButton.GetComponent<Image>().color = titleTransparentColor;
        triggerItems.SetActive(false);
        triggerButton.GetComponent<Image>().color = titleTransparentColor;

        switch (categorySelected) {
            case Categories.FloorTrap:
                floorTrapItems.SetActive(true);
                floorTrapButton.GetComponent<Image>().color = titleInitialColour;
                break;
            case Categories.WallTrap:
                wallTrapItems.SetActive(true);
                wallTrapButton.GetComponent<Image>().color = titleInitialColour;
                break;
            case Categories.Trigger:
                triggerItems.SetActive(true);
                triggerButton.GetComponent<Image>().color = titleInitialColour;
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
