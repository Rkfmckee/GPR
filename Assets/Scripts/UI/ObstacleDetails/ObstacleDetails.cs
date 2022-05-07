using TMPro;
using UnityEngine;

public class ObstacleDetails : MonoBehaviour {
	#region Properties

	protected ObstacleController obstacle;
	
	protected TextMeshProUGUI obstacleName;
	protected TextMeshProUGUI obstacleDescription;

	protected Transform background;

	#endregion

	#region Events

	protected virtual void Awake() {
		background = transform.Find("ObstacleDetailsBackground");
		obstacleName = background.Find("ObstacleName").GetComponent<TextMeshProUGUI>();
		obstacleDescription = background.Find("ObstacleDescription").GetComponent<TextMeshProUGUI>();
	}

	#endregion

	#region Methods

		#region Get/Set

		public void SetObstacle(ObstacleController obstacle) {
			this.obstacle = obstacle;
			obstacleDescription.text = obstacle.GetDescription();

			if (obstacle.gameObject.GetComponent<TrapController>()) {
				obstacleName.text = $"Trap: {obstacle.GetName()}";
				return;
			}

			if (obstacle.gameObject.GetComponent<TriggerController>()) {
				obstacleName.text = $"Trigger: {obstacle.GetName()}";
				return;
			}

			obstacleName.text = obstacle.GetName();
		}

		#endregion

	#endregion
}
