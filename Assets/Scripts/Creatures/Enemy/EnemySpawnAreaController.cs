﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnAreaController : MonoBehaviour
{
    #region Properties

    private void Awake() {
        References.Enemy.enemySpawnArea = gameObject;
    }

    #endregion
}
