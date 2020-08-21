using UnityEngine;
using UnityEngine.UI;

public class PreparingStage : Stage {
    #region Methods

    public override void StageStart() {
        StorageRoomController storageRoom = References.storageRoom.GetComponent<StorageRoomController>();

        if (!storageRoom.IsDoorOpen()) {
            storageRoom.Open();
        }
    }

    public override void StageUISetup() {
        Transform canvas = References.UI.canvas.transform;
        Transform roundStageBackground = canvas.Find("RoundStage").Find("RoundStageBackground").transform;
        StartRoundButtonController startButton = canvas.transform.Find("RoundStage").Find("RoundStageBackground").Find("StartRoundButton").GetComponent<StartRoundButtonController>();

        roundStageBackground.Find("PlanningStage").gameObject.SetActive(true);
        roundStageBackground.Find("DefendingStage").gameObject.SetActive(false);
        startButton.SetStartButtonPressed(false);
    }

    #endregion
}