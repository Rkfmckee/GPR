using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTrapsController : MonoBehaviour
{
    #region Properties
    [HideInInspector]
    public GameObject worldMousePointer;

    private GameObject worldMousePointerPrefab;
    private GameObject trapLinkingLinePrefab;
    private GameObject trapLinkingLine;
    private bool inventoryOpen;
    private bool highlightTextActive;
    private bool linkingTextActive;

    #endregion

    #region Events

    private void Awake() {
        References.GameController.gameTraps = this;
        worldMousePointerPrefab = Resources.Load("Prefabs/WorldMousePointer") as GameObject;
        trapLinkingLinePrefab = Resources.Load("Prefabs/UI/TrapLinkingLine") as GameObject;
    }

    #endregion

    #region Methods

    public void ShouldShowCaveInventory(bool showInventory) {
        References.UI.canvas.GetComponent<CanvasController>().SetCaveInventoryVisible(showInventory);
        References.currentPlayer.GetComponent<PlayerBehaviour>().SetCurrentlyBeingControlled(!showInventory);
        inventoryOpen = showInventory;
    }

    public bool IsInventoryOpen() {
        return inventoryOpen;
    }

    public bool IsTrapLinkingLineActive() {
        return trapLinkingLine != null;
    }

    public bool IsHighlightTextActive() {
        return highlightTextActive;
    }

    public bool IsLinkingTextActive() {
        return linkingTextActive;
    }

    public void RemoveTrapLinkingLine() {
        if (trapLinkingLine != null) {
            Destroy(trapLinkingLine);
        }
    }

    public void EnableWorldMousePointerIfPossible(TrapController.Type? trapType) {
        StartCoroutine(CheckIfPointerCanBeEnabled(trapType));
    }

    public void DisableWorldMousePointer() {
        if (worldMousePointer != null) Destroy(worldMousePointer);

        References.UI.canvas.GetComponent<CanvasController>().DisableHoldingItemText();
    }

    public void CreateTrapLinkingLine(Transform startTransform) {
        if (trapLinkingLine == null) {
            trapLinkingLine = Instantiate(trapLinkingLinePrefab);
            trapLinkingLine.GetComponent<TrapLinkingLineController>().SetStartValue(startTransform);
        }
    }

    public void EnableHighlightItemText(bool enable, bool enableLinkText) {
        References.UI.canvas.GetComponent<CanvasController>().EnableHighlightItemText(enable, enableLinkText);
        highlightTextActive = enable;
    }

    public void EnableLinkingItemText(bool enable) {
        References.UI.canvas.GetComponent<CanvasController>().EnableLinkingItemText(enable);
        linkingTextActive = enable;
    }

    private void EnableWorldMousePointer(TrapController.Type? trapType) {
        worldMousePointer = Instantiate(worldMousePointerPrefab);
        worldMousePointer.transform.parent = gameObject.transform;
        worldMousePointer.GetComponent<ObjectPlacementPointer>().trapType = trapType;

        bool holdingTrap = trapType != null;
        References.UI.canvas.GetComponent<CanvasController>().EnableHoldingItemText(holdingTrap);
    }

    #endregion

    #region Coroutines

    private IEnumerator CheckIfPointerCanBeEnabled(TrapController.Type? trapType) {
        GameTrapsController gameTrapController = References.GameController.gameTraps;

        while(gameTrapController.IsInventoryOpen()) {
            yield return null;
        }

        EnableWorldMousePointer(trapType);
    }

    #endregion
}
