using UnityEngine;

public class TriggerController : TrapTriggerBase {
	#region Properties

	public TrapController trapToTrigger;
	public CanTrigger canTrigger;

	#endregion

	#region Events

	private void Awake() {
		// If this script is on a trap, set this trap as it's trigger target
		if (trapToTrigger == null) trapToTrigger = GetComponent<TrapController>();
	}

	private void OnCollisionEnter(Collision collision) {
		Collider triggeredBy = collision.collider;

		if (triggeredBy.gameObject.tag == canTrigger.ToString()) {
			if (trapToTrigger != null) {
				trapToTrigger.TriggerTrap(triggeredBy);
			}
		}
	}

	private void OnTriggerEnter(Collider triggeredBy) {
		if (triggeredBy.gameObject.tag.Contains(canTrigger.ToString())) {
			if (trapToTrigger != null) {
				trapToTrigger.TriggerTrap(triggeredBy);
			}
		}
	}

	#endregion

	#region Enums

	public enum CanTrigger {
		Friendly,
		Hostile
	}

	#endregion
}