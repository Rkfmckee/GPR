using UnityEngine;

public class EnemySpawnArea : MonoBehaviour
{
	#region Events

	private void Awake()
	{
		References.HostileCreature.spawnArea = gameObject;
	}

	#endregion
}
