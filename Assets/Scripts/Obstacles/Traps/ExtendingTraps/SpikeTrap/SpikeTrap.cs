using UnityEngine;

public class SpikeTrap : ExtendingTrap
{
	#region Events

	protected override void Awake()
	{
		base.Awake();

		extendedScale  = new Vector3(1, 1, 1);
		retractedScale = new Vector3(1, 0, 1);

		extendingChildPrefab = Resources.Load("Prefabs/Obstacles/Traps/ExtendingTraps/SpikeTrap/Spikes") as GameObject;
	}

	#endregion
}
