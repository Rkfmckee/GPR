using UnityEngine;

public class SawbladeTrap : TrapController {
	#region Properties

	private bool canMove;	
	private bool constantlyTriggered;

	private Animator animator;

	#endregion

	#region Events

	protected override void Awake() {
		base.Awake();
		
		animator = GetComponent<Animator>();

		canMove = name.Contains("Moving");
		SetConstantlyMoving(canMove);
	}

	protected override void Update() {
		base.Update();

		if (!canMove)
			return;
	}

	#endregion

	#region Methods

		#region Get/Set

		public bool? IsConstantlyMoving() {
			if (!canMove)
				return null;
			
			return constantlyTriggered;
		}

		public void SetConstantlyMoving(bool shouldMove) {
			if (!canMove)
				return;

			constantlyTriggered = shouldMove;
			animator.SetBool("constantlyMoving", shouldMove);
		}

		#endregion

	public override void TriggerTrap(Collider triggeredBy) {
		base.TriggerTrap(triggeredBy);
		
		if (constantlyTriggered || !canMove)
			return;

		animator.Play("SawBladeMovingOnce");
	}

	public override void SetLinkedTrigger(TriggerController trigger) {
		base.SetLinkedTrigger(trigger);
		SetConstantlyMoving(false);
	}

	#endregion
}
