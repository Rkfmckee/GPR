using System.Collections;
using UnityEngine;

public class GameTrapsController : MonoBehaviour
{
    #region Properties

    [HideInInspector]
    public GameObject objectPlacement;
	public ObjectPlacementController objectPlacementController;

    private GameObject objectPlacementPrefab;
    private GameObject trapLinkingLinePrefab;
    private GameObject trapLinkingLine;
    private bool inventoryOpen;
    private bool trapDetailsOpen;
    private bool highlightTextActive;
    private bool linkingTextActive;

    #endregion

    #region Events

    private void Awake() {
        References.GameController.gameTraps = this;
        objectPlacementPrefab = Resources.Load("Prefabs/ObjectPlacement") as GameObject;
        trapLinkingLinePrefab = Resources.Load("Prefabs/UI/TrapLinkingLine") as GameObject;
    }

    #endregion

    #region Methods

    public void ShouldShowCaveInventory(bool showInventory) {
        References.UI.canvas.GetComponent<CanvasController>().SetCaveInventoryVisible(showInventory);
        References.Player.currentPlayer.GetComponent<PlayerBehaviour>().SetCurrentlyBeingControlled(!showInventory);
        inventoryOpen = showInventory;
    }

    public void ShouldShowTrapDetails(bool showDetails, GameObject trap) {
        References.UI.canvas.GetComponent<CanvasController>().SetTrapDetailsVisible(showDetails, trap);
        References.Player.currentPlayer.GetComponent<PlayerBehaviour>().SetCurrentlyBeingControlled(!showDetails);
        trapDetailsOpen = showDetails;
    }

    public bool IsInventoryOpen() {
        return inventoryOpen;
    }

    public bool IsTrapDetailsOpen() {
        return trapDetailsOpen;
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

    public void EnableObjectPlacementIfPossible(string objectName, bool canBeThrown) {
        StartCoroutine(CheckIfObjectPlacementCanBeEnabled(objectName, canBeThrown));
    }

    public void DisableObjectPlacement() {
        if (objectPlacement != null) Destroy(objectPlacement);

        References.UI.canvas.GetComponent<CanvasController>().DisableHoldingItemText();
    }

    public void CreateTrapLinkingLine(Transform startTransform) {
        if (trapLinkingLine == null) {
            trapLinkingLine = Instantiate(trapLinkingLinePrefab);
            trapLinkingLine.GetComponent<TrapLinkingLineController>().SetStartValue(startTransform);
        }
    }

    public void EnableHighlightItemText(bool enable, bool enableModifyText) {
        References.UI.canvas.GetComponent<CanvasController>().EnableHighlightItemText(enable, enableModifyText);
        highlightTextActive = enable;
    }

    public void EnableLinkingItemText(bool enable) {
        References.UI.canvas.GetComponent<CanvasController>().EnableLinkingItemText(enable);
        linkingTextActive = enable;
    }

    private void EnableObjectPlacement(string objectName, bool canBeThrown) {
        objectPlacement = Instantiate(objectPlacementPrefab);
        objectPlacement.transform.parent = gameObject.transform;
		objectPlacementController = objectPlacement.GetComponent<ObjectPlacementController>();
		objectPlacementController.SetHeldObject(objectName);

        References.UI.canvas.GetComponent<CanvasController>().EnableHoldingItemText(canBeThrown);
    }

    #endregion

    #region Coroutines

    private IEnumerator CheckIfObjectPlacementCanBeEnabled(string objectName, bool canBeThrown) {
        GameTrapsController gameTrapController = References.GameController.gameTraps;

        while(gameTrapController.IsInventoryOpen()) {
            yield return null;
        }

        EnableObjectPlacement(objectName, canBeThrown);
    }

    #endregion
}
