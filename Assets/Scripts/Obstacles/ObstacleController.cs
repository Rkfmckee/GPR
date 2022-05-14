using UnityEngine;

public class ObstacleController : MonoBehaviour {
	#region Properties

	[SerializeField]
	protected new string name;
	[SerializeField]
	protected string description;
	[SerializeField]
	protected SurfaceType surfaceType;

	private bool obstacleDisabled;

	protected PickUpObject pickUpController;

	#endregion

	#region Events

	protected virtual void Awake() {
		References.Obstacles.trapsAndTriggers.Add(gameObject);

		pickUpController = GetComponent<PickUpObject>();
		obstacleDisabled = false;
	}

	protected virtual void Update() {
	}

	#endregion

	#region Methods

		#region Get/Set

		public bool IsObstacleDisabled() {
			return obstacleDisabled;
		}

		public void SetObstacleDisabled(bool disabled) {
			obstacleDisabled = disabled;
		}

		public string GetName() { return name; }
		public string GetDescription() { return description; }
		public SurfaceType GetSurfaceType() { return surfaceType; }

		#endregion

	#endregion

	#region Enums

	public enum SurfaceType {
		Floor,
		Wall,
		Ceiling,
		Any
	}

	#endregion
}
