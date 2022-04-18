using UnityEngine;

public class EnemySpawnArea : MonoBehaviour {
	#region Properties

	private void Awake() {
		References.HostileCreature.spawnArea = gameObject;
	}

	#endregion
}
