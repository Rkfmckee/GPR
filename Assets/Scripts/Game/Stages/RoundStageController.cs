using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class RoundStageController : MonoBehaviour
{
    #region Properties

    private Stage currentStage;

    #endregion

    #region Events

    private void Awake() {
        References.GameController.gameControllerObject = gameObject;
        References.GameController.roundStage = this;
        currentStage = new PreparingStage();
        currentStage.StageStart();
    }

    #endregion

    #region Methods

    public Stage GetCurrentStage() {
        return currentStage;
    }

    public void SetCurrentStage(Stage stage) {
        currentStage.StageEnd();
        currentStage = stage;
        currentStage.StageStart();
        currentStage.StageUISetup();
    }
 
    #endregion
}
