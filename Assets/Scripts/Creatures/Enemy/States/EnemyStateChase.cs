using System.Collections.Generic;
using UnityEngine;

public class EnemyStateChase : EnemyState 
{
    #region Properties

    protected FieldOfView fieldOfView;

    #endregion

    #region Methods

    protected override void SetupProperties(GameObject gameObject) {
        base.SetupProperties(gameObject);

        fieldOfView = enemyObject.GetComponent<FieldOfView>();
    }

    protected override Vector3? FindMovementTarget() {
        List<GameObject> playersInEyesight = fieldOfView.visibleTargets;

        if (playersInEyesight.Count <= 0) { return null; }

        GameObject nearestPlayer = playersInEyesight[0];
        float distanceToNearestPlayer = Vector3.Distance(transform.position, nearestPlayer.transform.position);

        for (int i = 1; i < playersInEyesight.Count; i++) {
            GameObject currentPlayer = playersInEyesight[i];
            float distanceToCurrentPlayer = Vector3.Distance(transform.position, currentPlayer.transform.position);

            if (distanceToCurrentPlayer < distanceToNearestPlayer) {
                nearestPlayer = currentPlayer;
                distanceToNearestPlayer = distanceToCurrentPlayer;
            }

        }

        return nearestPlayer.transform.position;
    }

    #endregion
}
