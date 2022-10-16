using System.Collections.Generic;
using UnityEngine;
using static CameraController;

public class GoblinHighlightable : Highlightable {
    #region Properties

    private GoblinBehaviour goblinBehaviour;

    #endregion

    #region Events

    protected override void Awake() {
        base.Awake();

		goblinBehaviour = GetComponent<GoblinBehaviour>();

		statesAndUiText = new Dictionary<CameraControllingState, List<string>> {
			{
				CameraControllingState.ControllingSelf, new List<string>{
					"Left click to Control"
				}
			}
		};

		highlightCursor = CursorData.CursorType.BasicGreen;
    }

	protected override void Update() {
		base.Update();
	}

    #endregion

    #region Methods

    protected override void LeftClicked() {
		base.LeftClicked();

        if (goblinBehaviour == null) {
            print($"{gameObject} doesn't have a GoblinBehaviour");
            return;
        }

		goblinBehaviour.CurrentlyControlled = true;
    }

	protected override void RightClicked() {
		base.RightClicked();
	}

	protected override bool DontHighlight() {
		return base.DontHighlight() || goblinBehaviour.CurrentState is FriendlyStatePickupObject;
	}

	#endregion
}
