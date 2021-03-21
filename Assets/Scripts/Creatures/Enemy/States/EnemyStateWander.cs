using Boo.Lang;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateWander : EnemyState
{
    #region Properties

    protected bool arrivedAtDestination;

    #endregion

    #region Methods

    public EnemyStateWander(GameObject enemyObj) : base(enemyObj) {
        arrivedAtDestination = true;
        navMeshAgent.isStopped = false;
    }

    public override void StateUpdate() {
        base.StateUpdate();
    }

    public override void StateFixedUpdate() {
        if (arrivedAtDestination) {
            Move();
            arrivedAtDestination = false;
        } else {
            if (navMeshAgent.remainingDistance < 0.5) {
                behaviour.SetCurrentState(new EnemyStateLookAround(enemyObject, 3));
            }
        }
    }

    protected override void SetupProperties() {
        base.SetupProperties();
    }

    protected override Vector3? FindMovementTarget() {
        List<GameObject> pointsWithProbability = new List<GameObject>();

        foreach (GameObject point in References.Enemy.pathfindingPoints) {
            // Add each point to the list a number of times equal to it's distance rounded down
            int timesToAdd = (int) Vector3.Distance(point.transform.position, enemyObject.transform.position);

            while(timesToAdd > 0) {
                pointsWithProbability.Add(point);
                timesToAdd--;
            }
        }

        int randomPointIndex = Random.Range(0, pointsWithProbability.Count - 1);
        GameObject chosenPoint = pointsWithProbability[randomPointIndex];

        return chosenPoint.transform.position;
    }

    protected override void Move() {
        Vector3? movementTarget = FindMovementTarget();

        if (movementTarget.HasValue) {
            movementDirection = movementTarget.Value;
        }

        navMeshAgent.SetDestination(movementDirection);
    }

    #endregion
}