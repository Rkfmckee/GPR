using UnityEngine;

public class Fire : MonoBehaviour
{
	#region Fields

	private float damageAmount;

	private TrapController trapController;

	#endregion

	#region Events

	private void Awake()
	{
		trapController = GetComponentInParent<TrapController>();

		if (trapController.GetName().Contains("Big"))
		{
			damageAmount = 2;
		}
		else
		{
			damageAmount = 1;
		}
	}

	private void OnTriggerStay(Collider other)
	{
		var targetsHealthSystem = other.gameObject.GetComponent<HealthSystem>();
		if (targetsHealthSystem == null)
			return;

		targetsHealthSystem.TakeDamage(damageAmount * Time.deltaTime);
	}

	private void OnTriggerExit(Collider other)
	{
		var targetsHealthSystem = other.gameObject.GetComponent<HealthSystem>();
		if (targetsHealthSystem == null)
			return;

		targetsHealthSystem.TakeDamageOverTime(damageAmount * 2, damageAmount * 2, true);
	}

	#endregion
}
