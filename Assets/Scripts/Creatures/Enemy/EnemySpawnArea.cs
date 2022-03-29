using UnityEngine;

public class EnemySpawnArea : MonoBehaviour {
	#region Properties

	private void Awake() {
		References.enemySpawnArea = gameObject;
	}

	#endregion
}
