using UnityEngine;

public class ObstacleController : MonoBehaviour
{
	#region Fields

	[SerializeField]
	protected new string name;
	[SerializeField]
	protected string description;
	[SerializeField]
	protected ObstacleSurfaceType surfaceType;

	private bool obstacleDisabled;

	protected PickUpObject pickUpController;

	#endregion

	#region Properties

	public string Name { get => name; }

	public string Description { get => description; }

	public ObstacleSurfaceType SurfaceType { get => surfaceType; }

	public bool ObstacleDisabled { get => obstacleDisabled; set => obstacleDisabled = value; }

	#endregion

	#region Events

	protected virtual void Awake()
	{
		References.Obstacles.trapsAndTriggers.Add(gameObject);

		pickUpController = GetComponent<PickUpObject>();
		obstacleDisabled = false;
	}

	protected virtual void Update()
	{
	}

	#endregion

	#region Enums

	public enum ObstacleSurfaceType
	{
		Floor,
		Wall,
		Ceiling,
		Any
	}

	#endregion
}
