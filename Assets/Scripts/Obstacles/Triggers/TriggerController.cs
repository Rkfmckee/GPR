using UnityEngine;

public class TriggerController : TrapTriggerBase {
	#region Properties

	public CanTrigger canTrigger;

	private TrapController linkedTrap;


	#endregion

	#region Events

	protected override void Awake() {
		base.Awake();
		
		// If this script is on a trap, don't add it to the list of triggers
		// and set this trap as it's trigger target
		var trapController = GetComponent<TrapController>();
		if (trapController == null) References.Obstacles.triggers.Add(gameObject);

		linkedTrap = GetComponent<TrapController>();
	}

	private void OnCollisionEnter(Collision collision) {
		Collider triggeredBy = collision.collider;

		if (triggeredBy.gameObject.tag == canTrigger.ToString()) {
			if (linkedTrap != null) {
				linkedTrap.TriggerTrap(triggeredBy);
			}
		}
	}

	private void OnTriggerEnter(Collider triggeredBy) {
		if (triggeredBy.gameObject.tag.Contains(canTrigger.ToString())) {
			if (linkedTrap != null) {
				linkedTrap.TriggerTrap(triggeredBy);
			}
		}
	}

	#endregion

	#region Methods

		#region Get/Set

		public TrapController GetLinkedTrap() {
			return linkedTrap;
		}

		public void SetLinkedTrap(TrapController trap) {
			linkedTrap = trap;
		}

		#endregion

	#endregion

	#region Enums

	public enum CanTrigger {
		Friendly,
		Hostile
	}

	#endregion
}