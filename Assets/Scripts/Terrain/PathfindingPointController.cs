using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingPointController : MonoBehaviour
{
    #region Events

    private void Awake() {
        References.Enemy.pathfindingPoints.Add(gameObject);
    }

    #endregion
}
