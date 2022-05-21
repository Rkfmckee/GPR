using System.Collections.Generic;
using UnityEngine;
using static CameraController;

public class TrapHighlightable : ObstacleHighlightable {
    #region Properties

    private HealthSystem healthSystem;
    private GameObject healthBar;

    #endregion

    #region Events

    protected override void Awake() {
        base.Awake();

        healthSystem = GetComponent<HealthSystem>();

		statesAndUiText = new Dictionary<ControllingState, List<string>> {
			{
				ControllingState.ControllingSelf, new List<string>{
					"Right click to Modify",
				}
			},
			{
				ControllingState.ControllingFriendly, new List<string>{
					"Left click to Pick up"
				}
			}
		};
    }

    protected override void Update() {		
		base.Update();

        if (healthSystem != null) {
			healthBar = healthSystem.GetHealthBar();

			if (IsHighlightingMe()) {
				if (!healthBar.activeSelf) {
					healthBar.SetActive(true);
				}
			} else {
				if (healthBar.activeSelf) {
					healthBar.SetActive(false);
				}
			}
		}
    }

	#endregion

	#region Methods

	protected override void RightClicked() {
		if (cameraController.GetControllingState() == ControllingState.ControllingSelf) {
			base.RightClicked();
			
			globalObstacles.ShouldShowTrapDetails(true, gameObject);
		}
	}

	protected override bool DontHighlight() {
		var dontHighlight = globalObstacles.IsTrapDetailsOpen();
		
		return dontHighlight || base.DontHighlight();
	}

	#endregion
}