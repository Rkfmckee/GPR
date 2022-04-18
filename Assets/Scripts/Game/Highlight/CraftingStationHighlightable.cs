using System.Collections.Generic;
using UnityEngine;
using static CameraController;

public class CraftingStationHighlightable : Highlightable {
	#region Events

	protected override void Awake() {
		base.Awake();

		statesAndUiText = new Dictionary<ControllingState, List<GameObject>> {
			{
				ControllingState.ControllingSelf, new List<GameObject>{
				}
			}
		};
	}

	protected override void Update() {
		base.Update();
	}

	#endregion
	
	#region Methods
	
	protected override void LeftClicked() {
		gameTraps.ShouldShowCaveInventory(true);
	}

	protected override void RightClicked() {
	}

	#endregion
}
