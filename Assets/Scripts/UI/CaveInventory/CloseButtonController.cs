using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseButtonController : MonoBehaviour
{
    #region Properties

    private Button closeButton;

    #endregion

    #region Events

    private void Awake() {
        closeButton = GetComponent<Button>();
        closeButton.onClick.AddListener(CloseInventory);
    }

    #endregion

    #region Methods

    private void CloseInventory() {
        References.GameController.gameTraps.ShouldShowCaveInventory(false);
    }

    #endregion
}
