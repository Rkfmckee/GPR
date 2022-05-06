using TMPro;
using UnityEngine;

public class ObstacleDetails : MonoBehaviour {
	#region Properties

	protected ObstacleController obstacle;
	
	protected TextMeshProUGUI obstacleName;
	protected Transform background;

	#endregion

	#region Events

	protected virtual void Awake() {
		background = transform.Find("ObstacleDetailsBackground");
		obstacleName = background.Find("ObstacleName").GetComponent<TextMeshProUGUI>();
	}

	#endregion

	#region Methods

		#region Get/Set

		public void SetObstacle(ObstacleController obstacle) {
			this.obstacle = obstacle;
			obstacleName.text = obstacle.GetName();
		}

		#endregion

	#endregion
}
