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

	#endregion

	#region Constructor

	public FriendlyState(GameObject gameObj) {
		gameObject = gameObj;
		SetupProperties();
	}

	#endregion

	#region Events

	public virtual void Update() {
		animatorController.UpdateAnimatorValues(navMeshAgent.velocity.magnitude, 0);
	}

    public abstract void FixedUpdate();

	#endregion

	#region Methods

	protected virtual void SetupProperties() {
        transform = gameObject.transform;
        rigidbody = gameObject.GetComponent<Rigidbody>();
		navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
		animatorController = gameObject.GetComponent<AnimatorController>();
        behaviour = gameObject.GetComponent<FriendlyBehaviour>();

        movementSpeed = behaviour.movementSpeed;
		navMeshAgent.speed = movementSpeed;
    }

	#endregion
}
