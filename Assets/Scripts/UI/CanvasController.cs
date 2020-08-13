using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    #region Properties

    private GameObject caveInventory;
    private GameObject caveInventoryPrefab;

    private GameObject holdingItemPlaceText;
    private GameObject holdingItemThrowText;
    private GameObject holdingItemPlacePrefab;
    private GameObject holdingItemThrowPrefab;

    private GameObject pickupItemText;
    private GameObject linkItemText;
    private GameObject linkItemBothText;
    private GameObject pickupItemTextPrefab;
    private GameObject linkItemTextPrefab;
    private GameObject linkItemBothTextPrefab;

    #endregion

    #region Events

    private void Awake() {
        References.UI.canvas = gameObject;
        caveInventoryPrefab = Resources.Load("Prefabs/UI/CaveInventory") as GameObject;
        holdingItemPlacePrefab = Resources.Load("Prefabs/UI/HoldingItemPlace") as GameObject;
        holdingItemThrowPrefab = Resources.Load("Prefabs/UI/HoldingItemThrow") as GameObject;

        pickupItemTextPrefab = Resources.Load("Prefabs/UI/PickupItem") as GameObject;
        linkItemTextPrefab = Resources.Load("Prefabs/UI/LinkItem") as GameObject;
        linkItemBothTextPrefab = Resources.Load("Prefabs/UI/LinkItemBoth") as GameObject;
    }

    #endregion

    #region Methods

    public void SetCaveInventoryVisible(bool visible) {
        if (visible) {
            if (caveInventory == null) {
                caveInventory = Instantiate(caveInventoryPrefab, References.UI.canvas.transform);
            }
        } else {
            if (caveInventory != null) {
                Destroy(caveInventory);
                caveInventory = null;
            }
        }
    }

    public void EnableHoldingItemText(bool trapHeld) {
        if (holdingItemPlaceText == null) {
            holdingItemPlaceText = Instantiate(holdingItemPlacePrefab);
            holdingItemPlaceText.transform.SetParent(transform);
        }

        if (!trapHeld) {
            if (holdingItemThrowText == null) {
                holdingItemThrowText = Instantiate(holdingItemThrowPrefab);
                holdingItemThrowText.transform.SetParent(transform);
            }
        }
    }

    public void DisableHoldingItemText() {
        if (holdingItemPlaceText != null) Destroy(holdingItemPlaceText);
        if (holdingItemThrowText != null) Destroy(holdingItemThrowText);
    }

    public void EnableHighlightItemText(bool enable) {
        if (enable) {
            if (pickupItemText == null) {
                pickupItemText = Instantiate(pickupItemTextPrefab);
                pickupItemText.transform.SetParent(transform);
            }

            if (linkItemText == null) {
                linkItemText = Instantiate(linkItemTextPrefab);
                linkItemText.transform.SetParent(transform);
            }
        } else {
            if (pickupItemText != null) Destroy(pickupItemText);
            if (linkItemText != null) Destroy(linkItemText);
        }
    }

    public void EnableLinkingItemText(bool enable) {
        if (enable) {
            if (linkItemBothText == null) {
                linkItemBothText = Instantiate(linkItemBothTextPrefab);
                linkItemBothText.transform.SetParent(transform);
            }
        } else {
            if (linkItemBothText != null) Destroy(linkItemBothText);
        }
    }

    #endregion
}
