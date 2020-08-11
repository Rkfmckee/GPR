using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Properties
    [HideInInspector]
    public GameObject worldMousePointer;

    private GameObject worldMousePointerPrefab;
    private bool inventoryOpen;

    #endregion

    #region Events

    private void Awake() {
        References.gameController = gameObject;
        worldMousePointerPrefab = Resources.Load("Prefabs/WorldMousePointer") as GameObject;
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

    public void EnableWorldMousePointerIfPossible(TrapController.Type? trapType) {
        StartCoroutine(CheckIfPointerCanBeEnabled(trapType));
    }

    public void DisableWorldMousePointer() {
        if (worldMousePointer != null) Destroy(worldMousePointer);

        References.UI.canvas.GetComponent<CanvasController>().DisableHoldingItemText();
    }

    private void EnableWorldMousePointer(TrapController.Type? trapType) {
        worldMousePointer = Instantiate(worldMousePointerPrefab);
        worldMousePointer.transform.parent = gameObject.transform;
        worldMousePointer.GetComponent<ObjectPlacementPointer>().trapType = trapType;

        References.UI.canvas.GetComponent<CanvasController>().EnableHoldingItemText(trapType != null);
    }

    #endregion

    #region Coroutines

    private IEnumerator CheckIfPointerCanBeEnabled(TrapController.Type? trapType) {
        GameController gameController = References.gameController.GetComponent<GameController>();

        while(gameController.IsInventoryOpen()) {
            yield return null;
        }

        EnableWorldMousePointer(trapType);
    }

    #endregion
}
