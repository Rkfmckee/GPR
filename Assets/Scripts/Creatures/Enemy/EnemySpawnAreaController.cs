using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnAreaController : MonoBehaviour
{
    #region Properties

    private void Awake() {
        References.enemySpawnArea = gameObject;
    }

    #endregion
}
