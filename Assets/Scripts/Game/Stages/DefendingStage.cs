using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class DefendingStage : Stage {
    #region Properties

    protected List<GameObject> enemies;

    #endregion

    #region Methods

    public override void StageStart() {
        StorageRoomController storageRoom = References.storageRoom.GetComponent<StorageRoomController>();

        if (storageRoom.IsDoorOpen()) {
            storageRoom.Close();
        }
    }

    public override void StageUISetup() {
        Transform canvas = References.UI.canvas.transform;
        Transform roundStageBackground = canvas.Find("RoundStage").Find("RoundStageBackground").transform;
        Button startButton = canvas.transform.Find("RoundStage").Find("RoundStageBackground").Find("StartRoundButton").GetComponent<Button>();

        roundStageBackground.Find("PlanningStage").gameObject.SetActive(false);
        roundStageBackground.Find("DefendingStage").gameObject.SetActive(true);
        startButton.enabled = false;
    }

    #endregion
}
