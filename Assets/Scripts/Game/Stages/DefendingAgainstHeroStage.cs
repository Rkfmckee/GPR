using System.Collections.Generic;
using UnityEngine;

public class DefendingAgainstHeroStage : DefendingStage
{
	#region Fields

	protected GameObject enemyPrefab;

	#endregion

	#region Methods

	public override void StageStart()
	{
		base.StageStart();
		stageID += "_HERO";

		enemyPrefab    	   = Resources.Load<GameObject>("Prefabs/Enemy");
		var enemySpawnArea = References.HostileCreature.spawnArea.transform;

		enemies = new List<GameObject>();
		enemies.Add(Object.Instantiate(enemyPrefab, enemySpawnArea.position, Quaternion.LookRotation(-Vector3.forward)));

		References.HostileCreature.enemies = enemies;
	}

	#endregion
}
