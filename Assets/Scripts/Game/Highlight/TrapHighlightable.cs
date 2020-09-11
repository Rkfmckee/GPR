using UnityEngine;

public class TrapHighlightable : ObstacleHighlightable {
    #region Properties

    private SpikeTrapController spikeController;
    private HealthSystem healthSystem;
    private GameObject healthBar;

    #endregion

    #region Events

    protected override void Awake() {
        base.Awake();

        spikeController = GetComponent<SpikeTrapController>();
        healthSystem = GetComponent<HealthSystem>();
    }

    protected override void Update() {
        base.Update();
        healthBar = healthSystem.GetHealthBar();

        if (currentlyHightlightingMe) {
            if (!healthBar.activeSelf) {
                healthBar.SetActive(true);
            }

            if (Input.GetButtonDown("Fire2")) {
                if (tag == "Trap" || tag == "Trigger") {
                    gameTraps.CreateTrapLinkingLine(transform);
                } else {
                    print($"Objects with tag {tag} can't be linked");
                }
            }
        } else {
            if (healthBar.activeSelf) {
                healthBar.SetActive(false);
            }
        }
    }

    #endregion

    #region Methods

    protected override bool DontSelect() {
        bool dontSelect = false;

        if (spikeController != null) {
            // If the type of trap we're picking up is a spike trap
            if (spikeController.currentState != SpikeTrapController.SpikeState.SpikesDown) {
                dontSelect = true;
            }
        }

        return dontSelect || base.DontSelect();
    }

    #endregion
}