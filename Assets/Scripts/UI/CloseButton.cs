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

			case MenuType.TrapModification:
				closeButton.onClick.AddListener(CloseTrapModification);
				break;
		}
	}

	#endregion

	#region Methods

	private void CloseInventory() {
		References.Game.globalObstacles.ShouldShowCraftingMenu(false);
	}

	private void CloseTrapModification() {
		References.Game.globalObstacles.ShouldShowTrapDetails(false, null);
	}

	#endregion

	#region Enum

	public enum MenuType {
		CaveInventory,
		TrapModification
	}

	#endregion
}
