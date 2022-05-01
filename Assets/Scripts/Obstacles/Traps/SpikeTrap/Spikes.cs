using UnityEngine;

public class Spikes : MonoBehaviour {
	#region Events

	private void OnTriggerEnter(Collider other) {
		var targetsHealthSystem = other.gameObject.GetComponent<HealthSystem>();
		if (targetsHealthSystem != null) {
			targetsHealthSystem.TakeDamageOverTime(3, 1, false);
		}
	}

	#endregion
}
