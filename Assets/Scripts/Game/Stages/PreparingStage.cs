using UnityEngine;
using UnityEngine.UI;

public class PreparingStage : Stage {
    #region Methods

    public override void StageStart() {
        throw new System.NotImplementedException();
    }

    public override void StageUISetup() {
        Transform canvas = References.UI.canvas.transform;
        Transform roundStageBackground = canvas.Find("RoundStage").Find("RoundStageBackground").transform;
        Button startButton = canvas.transform.Find("RoundStage").Find("RoundStageBackground").Find("StartRoundButton").GetComponent<Button>();

        roundStageBackground.Find("PlanningStage").gameObject.SetActive(true);
        roundStageBackground.Find("DefendingStage").gameObject.SetActive(false);
        startButton.enabled = true;
        startButton.GetComponent<StartRoundButtonController>().ResetStartRoundButton();
    }

    #endregion
}