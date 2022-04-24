using UnityEngine;

public class SpikeTrapSpikes : MonoBehaviour {
	#region Events

	private void OnTriggerEnter(Collider other) {
		var targetsHealthSystem = other.gameObject.GetComponent<HealthSystem>();
		if (targetsHealthSystem != null) {
			targetsHealthSystem.TakeDamageOverTime(5, 3);
		}
	}

	#endregion
}
