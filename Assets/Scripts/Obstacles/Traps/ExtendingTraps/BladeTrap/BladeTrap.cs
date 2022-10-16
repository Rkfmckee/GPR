using UnityEngine;

public class BladeTrap : ExtendingTrap
{
	#region Events

	protected override void Awake()
	{
		base.Awake();

		extendedScale  = new Vector3(1, 1, 1);
		retractedScale = new Vector3(1, 0, 1);

		extendingChildPrefab = Resources.Load("Prefabs/Obstacles/Traps/ExtendingTraps/BladeTrap/Blade") as GameObject;
	}

	#endregion
}
