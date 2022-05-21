using UnityEngine;

public class GreatAxe : MonoBehaviour {
	#region Properties

	private float rotationThreshold;

	private TrapController trapController;

	#endregion
	
	#region Events

	private void Awake() {
		trapController = GetComponentInParent<TrapController>();

		rotationThreshold = 0.3f;
	}

	private void OnCollisionStay(Collision other) {
		if (trapController.IsObstacleDisabled()) {
			return;
		}

		// Only take damage if we collide in the same direction the blade is facing
		var absoluteDot = Mathf.Abs(Vector3.Dot(other.contacts[0].normal, transform.parent.right));
		
		if (absoluteDot > rotationThreshold) {
			var targetsHealthSystem = other.gameObject.GetComponent<HealthSystem>();
			if (targetsHealthSystem != null) {
				targetsHealthSystem.TakeDamageOverTime(2, 1, false);
			}
		}
	}

	#endregion
}
