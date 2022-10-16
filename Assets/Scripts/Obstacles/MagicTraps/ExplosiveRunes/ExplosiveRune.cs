using UnityEngine;

public class ExplosiveRune : TrapController
{
	#region Fields

	[SerializeField]
	private GameObject explosionEffect;

	private int creatureMask;

	#endregion

	#region Events

	protected override void Awake()
	{
		base.Awake();

		var layerMasks = GeneralHelper.GetLayerMasks();
		creatureMask   = layerMasks["FriendlyCreature"] | layerMasks["HostileCreature"];
	}

	#endregion

	#region Methods

	public override void TriggerTrap(Collider triggeredBy)
	{
		base.TriggerTrap(triggeredBy);

		var allCreatures = Physics.OverlapSphere(transform.position, 1, creatureMask);

		foreach (var creature in allCreatures)
		{
			var healthSystem = creature.GetComponent<HealthSystem>();
			if (healthSystem == null)
			{
				continue;
			}

			healthSystem.TakeDamageOverTime(3, 2, true);
		}

		var explosion           = Instantiate(explosionEffect, transform.position, Quaternion.identity);
		var childParticleSystem = explosion.transform.GetChild(0).GetComponent<ParticleSystem>();
		var explosionDuration   = childParticleSystem.main.duration;

		Destroy(explosion, explosionDuration);
		Destroy(gameObject);
	}

	#endregion
}
