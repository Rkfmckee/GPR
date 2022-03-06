using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateWander : EnemyState {
    
    #region Properties

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
		if (movementTarget == null || distanceToTarget < 1) {
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
		movementTarget = wanderPoints[Random.Range(0, wanderPoints.Length)].transform;
		navMeshAgent.SetDestination(movementTarget.position);
	}

	protected override void SetupProperties() {
        base.SetupProperties();

		navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
    }

	#endregion
}
