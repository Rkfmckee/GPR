using UnityEngine;

public class TrapTriggerController : MonoBehaviour {
	#region Properties

	public TrapController trapToTrigger;
	public CanTrigger canTrigger;

	#endregion

	#region Events

	private void Awake() {

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
		if (triggeredBy.gameObject.tag == canTrigger.ToString()) {
			if (trapToTrigger != null) {
				trapToTrigger.TriggerTrap(triggeredBy);
			}
		}
	}

	#endregion

	#region Enums

	public enum CanTrigger {
		Player,
		Enemy
	}

	#endregion
}