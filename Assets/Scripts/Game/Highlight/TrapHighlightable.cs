using System.Collections.Generic;
using UnityEngine;
using static CameraController;

public class TrapHighlightable : ObstacleHighlightable {
    #region Properties

    private SpikeTrap spikeController;
    private HealthSystem healthSystem;
    private GameObject healthBar;

    #endregion

    #region Events

    protected override void Awake() {
        base.Awake();

        spikeController = GetComponent<SpikeTrap>();
        healthSystem = GetComponent<HealthSystem>();

		highlightTextObjects = new List<GameObject> {
			Resources.Load<GameObject>("Prefabs/UI/Highlight/PickupItem"),
			Resources.Load<GameObject>("Prefabs/UI/Highlight/ModifyItem")
		};

		highlightableStates = new List<ControllingState>() {
			ControllingState.ControllingSelf,
			ControllingState.ControllingFriendly
		};
    }

    protected override void Update() {		
		base.Update();

        healthBar = healthSystem.GetHealthBar();

        if (IsHighlightingMe()) {
            if (!healthBar.activeSelf) {
                healthBar.SetActive(true);
            }

            if (Input.GetButtonDown("Fire2")) {
                if (tag == "Trap" || tag == "Trigger") {
                    gameTraps.ShouldShowTrapDetails(true, gameObject);
                }
            }
        } else {
            if (healthBar.activeSelf) {
                healthBar.SetActive(false);
            }
        }
    }

	#endregion

	protected override bool DontHighlight() {
		var dontHighlight = gameTraps.IsTrapDetailsOpen();

		if (spikeController != null) {
        	dontHighlight = dontHighlight || spikeController.currentState != SpikeTrap.SpikeState.SpikesDown;
		}
		
		return dontHighlight || base.DontHighlight();
	}
}