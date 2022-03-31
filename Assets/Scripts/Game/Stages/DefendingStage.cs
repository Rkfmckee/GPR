using System.Collections.Generic;
using UnityEngine;

public abstract class DefendingStage : Stage {
    
    #region Properties

    protected List<GameObject> enemies;

    #endregion

    #region Methods

    public override void StageStart() {
        stageID = "DEF";
        StorageRoom storageRoom = References.storageRoom.GetComponent<StorageRoom>();

        if (storageRoom.IsDoorOpen()) {
            storageRoom.Close();
        }
    }

    public override void StageUISetup() {
        Transform canvas = References.UI.canvas.transform;
        Transform roundStageBackground = canvas.Find("RoundStage").Find("RoundStageBackground").transform;
        StartRoundButton startButton = canvas.transform.Find("RoundStage").Find("RoundStageBackground").Find("StartRoundButton").GetComponent<StartRoundButton>();

        roundStageBackground.Find("PlanningStage").gameObject.SetActive(false);
        roundStageBackground.Find("DefendingStage").gameObject.SetActive(true);
        startButton.SetStartButtonPressed(true);
    }

    public override void StageEnd() {
        References.UI.notifications.AddNotification("Cave defended successfully");
    }

    #endregion
}
