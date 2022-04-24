using UnityEngine;

public abstract class TrapTriggerBase : MonoBehaviour {
	#region Properties

	[SerializeField]
	protected new string name;
	[SerializeField]
	protected SurfaceType surfaceType;

	#endregion

	#region Methods

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
