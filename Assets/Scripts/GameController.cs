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

    public void EnableWorldMousePointer(bool trapHeld) {
            if (worldMousePointer == null) {
                worldMousePointer = Instantiate(worldMousePointerPrefab);
                worldMousePointer.transform.parent = gameObject.transform;
            }

        References.UI.canvas.GetComponent<CanvasController>().EnableHoldingItemText(trapHeld);
    }

    public void DisableWorldMousePointer() {
        if (worldMousePointer != null) Destroy(worldMousePointer);

        References.UI.canvas.GetComponent<CanvasController>().DisableHoldingItemText();
    }

    #endregion
}
