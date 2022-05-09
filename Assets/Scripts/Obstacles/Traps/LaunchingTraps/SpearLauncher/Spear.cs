using UnityEngine;

public class Spear : LaunchingAmmo {
	#region Events

	protected override void Awake() {
		base.Awake();

		damage = 4;
		ammoPieces = new GameObject[] {
		};
	}

	#endregion
}
