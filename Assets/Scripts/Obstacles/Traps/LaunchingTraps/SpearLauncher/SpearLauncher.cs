using UnityEngine;

public class SpearLauncher : LaunchingTrap
{

	#region Events

	protected override void Awake()
	{
		base.Awake();

		ammoPrefab = Resources.Load("Prefabs/Obstacles/Traps/LaunchingTraps/SpearLauncher/Spear") as GameObject;
	}

	#endregion
}
