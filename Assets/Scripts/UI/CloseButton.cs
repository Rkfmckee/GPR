using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour {
	#region Properties

	public MenuType menu;
	private Button closeButton;

	#endregion

	#region Events

	private void Awake() {
		closeButton = GetComponent<Button>();

		switch (menu) {
			case MenuType.CaveInventory:
				closeButton.onClick.AddListener(CloseInventory);
				break;

			case MenuType.TrapDetails:
				closeButton.onClick.AddListener(CloseTrapDetails);
				break;
		}
	}

	#endregion

	#region Methods

	private void CloseInventory() {
		References.GameController.gameTraps.ShouldShowCaveInventory(false);
	}

	private void CloseTrapDetails() {
		References.GameController.gameTraps.ShouldShowTrapDetails(false, null);
	}

	#endregion

	#region Enum

	public enum MenuType {
		CaveInventory,
		TrapDetails
	}

	#endregion
}
