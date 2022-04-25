using UnityEngine;

public abstract class TrapTriggerBase : MonoBehaviour {
	#region Properties

	[SerializeField]
	protected new string name;
	[SerializeField]
	protected SurfaceType surfaceType;

	#endregion

	#region Events

	protected virtual void Awake() {
		References.Obstacles.allTrapsAndTriggers.Add(gameObject);
	}

	#endregion

	#region Methods

	public string GetName() { return name; }
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
