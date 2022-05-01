using System.Collections.Generic;
using UnityEngine;
using static CameraController;

public class ObstacleHighlightable : Highlightable {
    #region Properties

    private PickUpObject pickupController;

    #endregion

    #region Events

    protected override void Awake() {
        base.Awake();

		statesAndUiText = new Dictionary<ControllingState, List<string>> {
			{
				ControllingState.ControllingFriendly, new List<string>{
					"Left click to Pick up"
				}
			}
		};

        pickupController = GetComponent<PickUpObject>();
    }

	#endregion

	#region Methods

	protected override void LeftClicked() {
		base.LeftClicked();
	}

	protected override void RightClicked() {
		base.RightClicked();
	}

	#endregion
}