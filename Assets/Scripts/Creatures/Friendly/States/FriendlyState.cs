using UnityEngine;
using UnityEngine.AI;

public abstract class FriendlyState {
	#region Properties

	protected float movementSpeed;

	protected GameObject gameObject;
	protected Transform transform;
    protected Rigidbody rigidbody;
	protected NavMeshAgent navMeshAgent;
	protected AnimatorController animatorController;
    protected FriendlyBehaviour behaviour;

	protected CursorController cursor;

	#endregion

	#region Constructor

	public FriendlyState(GameObject gameObj) {
		gameObject = gameObj;

		transform = gameObject.transform;
        rigidbody = gameObject.GetComponent<Rigidbody>();
		navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
		animatorController = gameObject.GetComponent<AnimatorController>();
        behaviour = gameObject.GetComponent<FriendlyBehaviour>();
		cursor = References.Game.cursor;

        movementSpeed = behaviour.movementSpeed;
		navMeshAgent.speed = movementSpeed;
		navMeshAgent.ResetPath();
		navMeshAgent.velocity = Vector3.zero;
	}

	#endregion

	#region Events

	public virtual void Update() {
		animatorController.UpdateAnimatorValues(navMeshAgent.velocity.magnitude, 0);
	}

	#endregion
}
