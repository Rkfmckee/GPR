using UnityEngine;
using UnityEngine.UI;

public abstract class DefendingStage : Stage {
    #region Methods

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
