using UnityEngine;

public class SawbladeTrap : TrapController {
	#region Properties

	private bool canMove;	
	private bool constantlyMoving;

	private Animator animator;

	#endregion

	#region Events

	protected override void Awake() {
		base.Awake();
		
		animator = GetComponent<Animator>();

		canMove = name.Contains("Moving");
		SetConstantlyMoving(canMove);
	}

	private void Update() {
		if (!canMove)
			return;

		// if (Input.GetKeyDown(KeyCode.Tab))
		// 	SetConstantlyMoving(!constantlyMoving);

		// if (!constantlyMoving) {
		// 	if (Input.GetKeyDown(KeyCode.Backspace)) {
		// 		animator.Play("SawBladeMovingOnce");
		// 	}
		// }
	}

	#endregion

	#region Methods

		#region Get/Set

		public bool? IsConstantlyMoving() {
			if (!canMove)
				return null;
			
			return constantlyMoving;
		}

		public void SetConstantlyMoving(bool shouldMove) {
			if (!canMove)
				return;

			constantlyMoving = shouldMove;
			animator.SetBool("constantlyMoving", shouldMove);
		}

		#endregion

	public override void TriggerTrap(Collider triggeredBy) {
		if (constantlyMoving || !canMove)
			return;

		animator.Play("SawBladeMovingOnce");
	}

	public override void SetLinkedTrigger(TriggerController trigger) {
		base.SetLinkedTrigger(trigger);
		SetConstantlyMoving(false);
	}

	#endregion
}
