using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Properties
    [HideInInspector]
    public GameObject worldMousePointer;

    private GameObject worldMousePointerPrefab;

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
    }

    public void EnableWorldMousePointer(TrapController.Type? trapType) {
            if (worldMousePointer == null) {
                worldMousePointer = Instantiate(worldMousePointerPrefab);
                worldMousePointer.transform.parent = gameObject.transform;
                worldMousePointer.GetComponent<ObjectPlacementPointer>().trapType = trapType;
            }

        References.UI.canvas.GetComponent<CanvasController>().EnableHoldingItemText(trapType != null);
    }

    public void DisableWorldMousePointer() {
        if (worldMousePointer != null) Destroy(worldMousePointer);

        References.UI.canvas.GetComponent<CanvasController>().DisableHoldingItemText();
    }

    #endregion
}
