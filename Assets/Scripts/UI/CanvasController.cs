﻿using System.Collections;
using System.Collections.Generic;
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

    #endregion

    #region Events

    private void Awake() {
        References.UI.canvas = gameObject;
        caveInventoryPrefab = Resources.Load("Prefabs/UI/CaveInventory") as GameObject;
        holdingItemPlacePrefab = Resources.Load("Prefabs/UI/HoldingItemPlace") as GameObject;
        holdingItemThrowPrefab = Resources.Load("Prefabs/UI/HoldingItemThrow") as GameObject;
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

    #endregion
}
