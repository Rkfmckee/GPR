using System.Collections.Generic;
using UnityEngine;

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
    }

    protected override void Update() {
        base.Update();
        if (DontSelect()) return;
        healthBar = healthSystem.GetHealthBar();

        if (IsHighlightingMe()) {
            if (!healthBar.activeSelf) {
                healthBar.SetActive(true);
            }

            // if (Input.GetButtonDown("Fire2")) {
            //     if (tag == "Trap" || tag == "Trigger") {
            //         gameTraps.ShouldShowTrapDetails(true, gameObject);
            //     }
            // }
        } else {
            if (healthBar.activeSelf) {
                healthBar.SetActive(false);
            }
        }
    }

    #endregion

    #region Methods

    public override bool DontSelect() {
        bool dontSelect = false;

        if (gameTraps.IsTrapDetailsOpen()) {
            dontSelect = true;
        }

        if (spikeController != null) {
            // If the type of trap we're picking up is a spike trap
            if (spikeController.currentState != SpikeTrap.SpikeState.SpikesDown) {
                dontSelect = true;
            }
        }

        return dontSelect || base.DontSelect();
    }

    #endregion
}