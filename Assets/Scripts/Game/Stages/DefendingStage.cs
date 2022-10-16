using System.Collections.Generic;
using UnityEngine;
using static NotificationController;

public abstract class DefendingStage : Stage
{
	#region Fields

	protected List<GameObject> enemies;

	#endregion

	#region Methods

	public override void StageStart()
	{
		var storageRoom = References.storageRoom.GetComponent<StorageRoom>();
		stageID     	= "DEF";

		if (storageRoom.IsDoorOpen())
		{
			storageRoom.Close();
		}
	}

	public override void StageUISetup()
	{
		var canvas               = References.UI.canvas.transform;
		var roundStageBackground = canvas.Find("RoundStage").Find("RoundStageBackground").transform;
		var startButton          = canvas.transform.Find("RoundStage").Find("RoundStageBackground").Find("StartRoundButton").GetComponent<StartRoundButton>();

		roundStageBackground.Find("PlanningStage").gameObject.SetActive(false);
		roundStageBackground.Find("DefendingStage").gameObject.SetActive(true);
		startButton.SetStartButtonPressed(true);
	}

	public override void StageEnd()
	{
		References.UI.notifications.AddNotification("Cave defended successfully", NotificationType.Success);
	}

	#endregion
}
