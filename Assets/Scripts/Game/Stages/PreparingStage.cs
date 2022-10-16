using UnityEngine;
using static NotificationController;

public class PreparingStage : Stage
{
	#region Methods

	public override void StageStart()
	{
		var storageRoom = References.storageRoom.GetComponent<StorageRoom>();
		stageID = "PREP";

		if (!storageRoom.IsDoorOpen())
		{
			storageRoom.Open();
		}
	}

	public override void StageUISetup()
	{
		var canvas               = References.UI.canvas.transform;
		var roundStageBackground = canvas.Find("RoundStage").Find("RoundStageBackground").transform;
		var startButton          = canvas.transform.Find("RoundStage").Find("RoundStageBackground").Find("StartRoundButton").GetComponent<StartRoundButton>();

		roundStageBackground.Find("PlanningStage").gameObject.SetActive(true);
		roundStageBackground.Find("DefendingStage").gameObject.SetActive(false);
		startButton.SetStartButtonPressed(false);
	}

	public override void StageEnd()
	{
		References.UI.notifications.AddNotification("Defend your allies and valuables!", NotificationType.Info);
	}

	#endregion
}