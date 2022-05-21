using UnityEngine;

public class Blade : MonoBehaviour {
	#region Events

	private void OnCollisionEnter(Collision collision) {
		var other = collision.collider;
		
		var targetsHealthSystem = other.gameObject.GetComponent<HealthSystem>();
		if (targetsHealthSystem != null) {
			targetsHealthSystem.TakeDamageOverTime(5, 1, false);
		}
	}

	#endregion
}
