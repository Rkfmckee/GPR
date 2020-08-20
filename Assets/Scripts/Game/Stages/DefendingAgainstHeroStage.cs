using System.Collections.Generic;
using UnityEngine;

public class DefendingAgainstHeroStage : DefendingStage {
    #region Properties

    protected GameObject enemyPrefab;

    #endregion

    #region Methods

    public override void StageStart() {
        base.StageStart();

        enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy");
        Transform enemySpawnArea = References.enemySpawnArea.transform;

        enemies = new List<GameObject>();
        enemies.Add(Object.Instantiate(enemyPrefab, enemySpawnArea.position, Quaternion.LookRotation(-Vector3.forward)));
    }

    #endregion
}
