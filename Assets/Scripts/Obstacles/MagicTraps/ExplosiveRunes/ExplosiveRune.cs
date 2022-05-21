using UnityEngine;

public class ExplosiveRune : TrapController {
	#region Properties

	[SerializeField]
	private GameObject explosionEffect;

	#endregion
	
	protected override void Update() {
		if (Input.GetButtonDown("Fire1")) {
			TriggerTrap(null);
		}
	}

	#region Methods

	public override void TriggerTrap(Collider triggeredBy) {
		base.TriggerTrap(triggeredBy);

		var explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
		var childParticleSystem = explosion.transform.GetChild(0).GetComponent<ParticleSystem>();
		var explosionDuration = childParticleSystem.main.duration;

		Destroy(explosion, explosionDuration);
		Destroy(gameObject);
	}

	#endregion
}
