using System.Collections.Generic;
using UnityEngine;
using static CameraController;

public class CraftingStationHighlightable : Highlightable {
	#region Properties

	private CraftingStation craftingStation;

	#endregion
	
	#region Events

	protected override void Awake() {
		base.Awake();

		craftingStation = GetComponent<CraftingStation>();

		statesAndUiText = new Dictionary<ControllingState, List<GameObject>> {
			{
				ControllingState.ControllingSelf, new List<GameObject>{
				}
			}
		};

		highlightCursor = CursorData.CursorType.Craft;
	}

	protected override void Update() {
		base.Update();
	}

	#endregion
	
	#region Methods
	
	protected override void LeftClicked() {
		if (!craftingStation.CheckCraftingAreaIsClear()) {
			References.UI.notifications.AddNotification("Cannot craft until crafting area is clear");
			return;
		}

		gameTraps.ShouldShowCraftingMenu(true, craftingStation);
	}

	protected override void RightClicked() {
	}

	#endregion
}
