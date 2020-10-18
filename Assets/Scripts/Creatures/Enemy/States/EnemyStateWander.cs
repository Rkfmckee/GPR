using Boo.Lang;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateWander : EnemyState
{
    #region Properties

    protected bool arrivedAtDestination;
    protected NavMeshAgent navMeshAgent;

    #endregion

    #region Methods

    public EnemyStateWander(GameObject enemyObj) : base(enemyObj) {
    }

    public override void MoveTowardsTarget() {
        if (arrivedAtDestination) {
            Move();
            arrivedAtDestination = false;
        } else {
            if (navMeshAgent.remainingDistance < 0.5) {
                arrivedAtDestination = true;
            }
        }
    }

    protected override void SetupProperties() {
        base.SetupProperties();

        navMeshAgent = enemyObject.GetComponent<NavMeshAgent>();
        navMeshAgent.speed = movementSpeed;
        arrivedAtDestination = true;
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

        //float maxRadius = fieldOfView.viewRadius * 3;
        //Vector3 directionToMove = Random.insideUnitSphere * maxRadius;
        //directionToMove += transform.position;

        //NavMeshHit hit;
        //Vector3? movementTarget = null;

        //if (NavMesh.SamplePosition(directionToMove, out hit, maxRadius, 1)) {
        //    movementTarget = hit.position;
        //}

        //return movementTarget;
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