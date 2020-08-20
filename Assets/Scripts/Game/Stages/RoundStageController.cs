using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundStageController : MonoBehaviour
{
    #region Properties

    private Stage currentStage;

    #endregion

    #region Events

    private void Awake() {
        References.GameController.roundStage = this;
        currentStage = new PreparingStage();
    }

    #endregion

    #region Methods

    public Stage GetCurrentStage() {
        return currentStage;
    }

    public void SetCurrentStage(Stage stage) {
        currentStage = stage;
        currentStage.StageStart();
        currentStage.StageUISetup();
    }
 
    #endregion
}
