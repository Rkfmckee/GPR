using UnityEngine;

public class ObstacleController : MonoBehaviour {
	#region Properties

	[SerializeField]
	protected new string name;
	[SerializeField]
	protected string description;
	[SerializeField]
	protected SurfaceType surfaceType;

	protected PickUpObject pickUpController;

	#endregion

	#region Events

	protected virtual void Awake() {
		References.Obstacles.trapsAndTriggers.Add(gameObject);

		pickUpController = GetComponent<PickUpObject>();
	}

	#endregion

	#region Methods

	public string GetName() { return name; }
	public string GetDescription() { return description; }
	public SurfaceType GetSurfaceType() { return surfaceType; }

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
