using TMPro;
using UnityEngine;

public class ObstacleDetails : MonoBehaviour
{
	#region Fields

	protected ObstacleController obstacle;

	protected TextMeshProUGUI obstacleName;
	protected TextMeshProUGUI obstacleDescription;

	protected Transform background;

	#endregion

	#region Properties

	public ObstacleController Obstacle 
	{
		set
		{
			obstacle = value;
			obstacleDescription.text = obstacle.Description;

			if (obstacle.gameObject.GetComponent<TrapController>())
			{
				obstacleName.text = $"Trap: {obstacle.Name}";
				return;
			}

			if (obstacle.gameObject.GetComponent<TriggerController>())
			{
				obstacleName.text = $"Trigger: {obstacle.Name}";
				return;
			}

			obstacleName.text = obstacle.Name;
		}
	}

	#endregion

	#region Events

	protected virtual void Awake()
	{
		background          = transform.Find("ObstacleDetailsBackground");
		obstacleName        = background.Find("ObstacleName").GetComponent<TextMeshProUGUI>();
		obstacleDescription = background.Find("ObstacleDescription").GetComponent<TextMeshProUGUI>();
	}

	#endregion
}
