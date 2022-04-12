using System.Collections.Generic;
using UnityEngine;

public class ObstacleHighlightable : Highlightable {
    #region Properties

    private PickUpObject pickupController;

    #endregion

    #region Events

    protected override void Awake() {
        base.Awake();

		highlightTextObjects = new List<GameObject> {
			Resources.Load<GameObject>("Prefabs/UI/Highlight/PickupItem"),
		};

        pickupController = GetComponent<PickUpObject>();
    }

    #endregion

    #region Methods

    public override bool DontSelect() {
        bool dontSelect = pickupController.currentState == PickUpObject.State.Held;

        return dontSelect || base.DontSelect();
    }

    protected override void Clicked() {
        if (pickupController == null) {
            print(gameObject + "can't be picked up");
            return;
        }

        //pickupController.SetCurrentState(PickUpObject.State.HELD);
    }

    #endregion
}