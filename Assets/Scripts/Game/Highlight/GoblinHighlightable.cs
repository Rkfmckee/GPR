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

		statesAndUiText = new Dictionary<ControllingState, List<GameObject>> {
			{
				ControllingState.ControllingSelf, new List<GameObject>{
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
        if (goblinBehaviour == null) {
            print($"{gameObject} doesn't have a GoblinBehaviour");
            return;
        }

		goblinBehaviour.SetCurrentlyControlled(true);
    }

	protected override void RightClicked() {
	}

	protected override bool DontHighlight() {
		return base.DontHighlight() || goblinBehaviour.GetCurrentState() is FriendlyStatePickupObject;
	}

	#endregion
}
