using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundStageController : MonoBehaviour
{
    #region Properties

    private RoundStage currentStage;

    private RoundStageCanvas canvasRoundStage;
    private Button startRoundButton;

    #endregion

    #region Events

    private void Awake() {
        References.GameController.roundStage = this;
        currentStage = RoundStage.Preparing;
    }

    private void Start() {
        canvasRoundStage = References.UI.canvasRoundStage;
        startRoundButton = References.UI.canvas.transform.Find("RoundStage").Find("RoundStageBackground").Find("StartRoundButton").GetComponent<Button>();
    }

    #endregion

    #region Methods

    public RoundStage GetCurrentStage() {
        return currentStage;
    }

    public void SetCurrentStage(RoundStage stage) {
        currentStage = stage;
        canvasRoundStage.SetCurrentStage(stage);

        if (stage == RoundStage.Preparing) {
            startRoundButton.enabled = true;
        } else if (stage == RoundStage.Defending) {
            startRoundButton.enabled = false;
        }
    }
 
    #endregion

    #region Enums

    public enum RoundStage {
        Preparing,
        Defending
    }

    #endregion
}
