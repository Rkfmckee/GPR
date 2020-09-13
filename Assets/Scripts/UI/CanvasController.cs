using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    #region Properties

    private GameObject caveInventory;
    private GameObject caveInventoryPrefab;

    private GameObject trapDetails;
    private GameObject trapDetailsPrefab;

    private GameObject holdingItemPlaceText;
    private GameObject holdingItemThrowText;
    private GameObject holdingItemPlacePrefab;
    private GameObject holdingItemThrowPrefab;

    private GameObject pickupItemText;
    private GameObject modifyItemText;
    private GameObject linkItemBothText;
    private GameObject pickupItemTextPrefab;
    private GameObject modifyItemTextPrefab;
    private GameObject linkItemBothTextPrefab;

    #endregion

    #region Events

    private void Awake() {
        References.UI.canvas = gameObject;
        caveInventoryPrefab = Resources.Load<GameObject>("Prefabs/UI/CaveInventory");
        trapDetailsPrefab = Resources.Load<GameObject>("Prefabs/UI/TrapDetails");
        holdingItemPlacePrefab = Resources.Load<GameObject>("Prefabs/UI/HoldingItemPlace");
        holdingItemThrowPrefab = Resources.Load<GameObject>("Prefabs/UI/HoldingItemThrow");

        pickupItemTextPrefab = Resources.Load<GameObject>("Prefabs/UI/PickupItem");
        modifyItemTextPrefab = Resources.Load<GameObject>("Prefabs/UI/ModifyItem");
        linkItemBothTextPrefab = Resources.Load<GameObject>("Prefabs/UI/LinkItemBoth");
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

    public void SetTrapDetailsVisible(bool visible, GameObject trap) {
        if (visible) {
            if (trapDetails == null) {
                trapDetails = Instantiate(trapDetailsPrefab, References.UI.canvas.transform);
                trapDetails.GetComponent<TrapDetailsController>().SetTrapShowing(trap);
            }
        } else {
            if (trapDetails != null) {
                Destroy(trapDetails);
                trapDetails = null;
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

    public void EnableHighlightItemText(bool enable, bool enableModifyText) {
        if (enable) {
            if (pickupItemText == null) {
                pickupItemText = Instantiate(pickupItemTextPrefab);
                pickupItemText.transform.SetParent(transform);
            }

            if (enableModifyText) {
                if (modifyItemText == null) {
                    modifyItemText = Instantiate(modifyItemTextPrefab);
                    modifyItemText.transform.SetParent(transform);
                }
            }
        } else {
            if (pickupItemText != null) Destroy(pickupItemText);
            if (modifyItemText != null) Destroy(modifyItemText);
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
