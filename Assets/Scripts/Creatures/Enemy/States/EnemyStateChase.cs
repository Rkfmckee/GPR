using System.Collections.Generic;
using UnityEngine;

public class EnemyStateChase : EnemyState 
{
    #region Methods

    public EnemyStateChase(GameObject enemyObj) : base(enemyObj) {
        navMeshAgent.isStopped = true;
        navMeshAgent.ResetPath();
    }

    public override void StateFixedUpdate() {
        Move();
    }

    public override void StateUpdate() {
        //if (fieldOfView.visibleTargets.Count <= 0) {
        //    behaviour.SetCurrentState(new EnemyStateLookAround(enemyObject, 3));
        //}
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
