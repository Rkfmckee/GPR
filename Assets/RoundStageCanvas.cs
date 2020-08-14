using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundStageCanvas : MonoBehaviour
{
    #region Properties

    private GameObject roundStageBackground;
    private GameObject preparingStageText;
    private GameObject defendingStageText;

    #endregion

    #region Events

    private void Awake() {
        References.UI.canvasRoundStage = this;
        roundStageBackground = transform.Find("RoundStage").Find("RoundStageBackground").gameObject;
        preparingStageText = roundStageBackground.transform.Find("PlanningStage").gameObject;
        defendingStageText = roundStageBackground.transform.Find("DefendingStage").gameObject;
    }

    #endregion

    #region Methods

    public void SetCurrentStage(RoundStageController.RoundStage stage) {
        if (stage == RoundStageController.RoundStage.Preparing) {
            preparingStageText.SetActive(true);
            defendingStageText.SetActive(false);
        } else if (stage == RoundStageController.RoundStage.Defending) {
            preparingStageText.SetActive(false);
            defendingStageText.SetActive(true);
        }
    }

    #endregion
}
