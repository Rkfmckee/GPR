using UnityEngine;

public class TrapHighlightable : ObstacleHighlightable {
    #region Properties

    private SpikeTrapController spikeController;

    #endregion

    #region Events

    protected override void Awake() {
        base.Awake();

        spikeController = GetComponent<SpikeTrapController>();
    }

    protected override void Update() {
        base.Update();

        if (currentlyHightlightingMe) {
            if (Input.GetButtonDown("Fire2")) {
                if (tag == "Trap" || tag == "Trigger") {
                    gameTraps.CreateTrapLinkingLine(transform);
                } else {
                    print($"Objects with tag {tag} can't be linked");
                }
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