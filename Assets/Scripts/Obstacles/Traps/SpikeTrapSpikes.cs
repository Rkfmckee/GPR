using UnityEngine;

public class SpikeTrapSpikes : MonoBehaviour {
	#region Events

	private void OnTriggerEnter(Collider triggeredBy) {
		var targetsHealthSystem = null as HealthSystem;
		if ((targetsHealthSystem = triggeredBy.gameObject.GetComponent<HealthSystem>()) != null) {
			targetsHealthSystem.TakeDamageOverTime(5, 3);
		}
	}

	#endregion
}
