using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateWander : EnemyState {
    
    #region Properties

	private int DISTANCE_TO_STOP = 1;

	private NavMeshAgent navMeshAgent;
	private Transform movementTarget;
	private GameObject[] wanderPoints;

	#endregion
	
	#region Constructor

    public EnemyStateWander(GameObject gameObj) : base(gameObj) {
		wanderPoints = GameObject.FindGameObjectsWithTag("WanderPoint");
		movementTarget = null;
		navMeshAgent.speed = movementSpeed;
    }

    #endregion

    #region Actions

    public override void Update() {
		var distanceToTarget = movementTarget != null ? Vector3.Distance(transform.position, movementTarget.position) : 0;

		// If we don't have a target, or have arrived at our target
		if (movementTarget == null || distanceToTarget < DISTANCE_TO_STOP) {
			ChooseNewMovementTarget();
		}

		ChaseTargetIfInFieldOfView();
    }
    
    public override void FixedUpdate() {
    }

    public override void OnCollisionEnter(Collision collision) {
    }

    #endregion

	#region Methods

	public void ChooseNewMovementTarget() {
		var pointsWithProbability = new List<Transform>();

		// If a point is farther away, add it to the list more times
		// so it's more likely to be chosen
		foreach (var point in wanderPoints) {
			var pointTransform = point.transform;
			var distanceToPoint = (int) Vector3.Distance(transform.position, pointTransform.position);

			for (int i = 0; i < distanceToPoint; i++) {
				pointsWithProbability.Add(pointTransform);
			}
		}

		movementTarget = pointsWithProbability[Random.Range(0, pointsWithProbability.Count)];
		navMeshAgent.SetDestination(movementTarget.position);
	}

	protected override void SetupProperties() {
        base.SetupProperties();

		navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
    }

	#endregion
}
