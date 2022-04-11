using UnityEngine;
using UnityEngine.AI;

public class FriendlyStateGoTo : FriendlyState {
	#region Properties

	private Vector3 targetPosition;

	#endregion
	
	#region Constructor
	
	public FriendlyStateGoTo(GameObject gameObj, Vector3 target) : base(gameObj) {		
		targetPosition = target;
		navMeshAgent.SetDestination(targetPosition);
	}

	#endregion

	#region Events

	public override void Update() {		
		base.Update();

		if (HasReachedDestination()) {
			behaviour.SetCurrentState(new FriendlyStateListening(gameObject));
		}
	}

	#endregion

	#region Methods

	private bool HasReachedDestination() {
		return navMeshAgent.remainingDistance != Mathf.Infinity
			&& navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete
			&& Mathf.Approximately(navMeshAgent.remainingDistance, 0);
	}

	#endregion
}
