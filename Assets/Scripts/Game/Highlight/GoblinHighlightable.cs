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

		statesAndUiText = new Dictionary<ControllingState, List<GameObject>> {
			{
				ControllingState.ControllingSelf, new List<GameObject>{
				}
			}
		};

        goblinBehaviour = GetComponent<GoblinBehaviour>();
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

	#endregion
}
