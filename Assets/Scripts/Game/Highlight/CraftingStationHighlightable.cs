using System.Collections.Generic;
using UnityEngine;
using static CameraController;
using static NotificationController;

public class CraftingStationHighlightable : Highlightable {
	#region Properties

	private CraftingStation craftingStation;

	#endregion
	
	#region Events

	protected override void Awake() {
		base.Awake();

		craftingStation = GetComponent<CraftingStation>();

		statesAndUiText = new Dictionary<CameraControllingState, List<string>> {
			{
				CameraControllingState.ControllingSelf, new List<string>{
					"Left click to Craft"
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
		base.LeftClicked();

		if (!craftingStation.CheckCraftingAreaIsClear()) {
			References.UI.notifications.AddNotification("Cannot craft until crafting area is clear", NotificationType.Error);
			return;
		}

		globalObstacles.ShouldShowCraftingMenu(true, craftingStation);
	}

	protected override void RightClicked() {
		base.RightClicked();
	}

	protected override bool DontHighlight() {
		return base.DontHighlight() || craftingStation.IsCurrentlyCrafting();
	}

	#endregion
}
