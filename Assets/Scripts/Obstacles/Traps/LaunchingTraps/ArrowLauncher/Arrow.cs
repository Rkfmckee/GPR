using UnityEngine;

public class Arrow : LaunchingAmmo
{
	#region Events

	protected override void Awake()
	{
		base.Awake();

		damage = 2;
		ammoPieces = new GameObject[] {
			Resources.Load("Prefabs/Obstacles/Traps/LaunchingTraps/ArrowLauncher/ArrowPiece-Feather") as GameObject,
			Resources.Load("Prefabs/Obstacles/Traps/LaunchingTraps/ArrowLauncher/ArrowPiece-Body") as GameObject,
			Resources.Load("Prefabs/Obstacles/Traps/LaunchingTraps/ArrowLauncher/ArrowPiece-Tip") as GameObject
		};
	}

	#endregion
}
