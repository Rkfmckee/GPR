using UnityEngine;

public class RoundStageController : MonoBehaviour
{
	#region Fields

	private Stage currentStage;

	#endregion

	#region Events

	private void Awake()
	{
		References.Game.globalGameObject = gameObject;
		References.Game.roundStage       = this;

		currentStage = new PreparingStage();
		currentStage.StageStart();
	}

	#endregion

	#region Methods

	public Stage GetCurrentStage()
	{
		return currentStage;
	}

	public void SetCurrentStage(Stage stage)
	{
		currentStage.StageEnd();
		currentStage = stage;
		currentStage.StageStart();
		currentStage.StageUISetup();
	}

	#endregion
}
