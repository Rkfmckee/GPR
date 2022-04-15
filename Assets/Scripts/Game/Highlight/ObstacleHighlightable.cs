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

		statesAndUiText = new Dictionary<ControllingState, List<GameObject>> {
			{
				ControllingState.ControllingFriendly, new List<GameObject>{
					Resources.Load<GameObject>("Prefabs/UI/Highlight/PickupItem")
				}
			}
		};

        pickupController = GetComponent<PickUpObject>();
    }

	#endregion

	#region Methods

	protected override void Clicked() {
	}

	#endregion
}