using UnityEngine;
using static PickUpObject;

public class TriggerController : ObstacleController
{
	#region Fields

	public CanTrigger canTrigger;

	private TrapController linkedTrap;

	#endregion

	#region Properties

	public TrapController LinkedTrap { get => linkedTrap; set => linkedTrap = value; }

	#endregion

	#region Events

	protected override void Awake()
	{
		base.Awake();

		// If this script is on a trap, don't add it to the list of triggers
		// and set this trap as it's trigger target
		var trapController = GetComponent<TrapController>();
		if (trapController == null) References.Obstacles.triggers.Add(gameObject);

		linkedTrap = trapController;
	}

	private void OnCollisionEnter(Collision collision)
	{
		Collider triggeredBy = collision.collider;

		ShouldTriggerTrap(triggeredBy);
	}

	private void OnTriggerEnter(Collider triggeredBy)
	{
		ShouldTriggerTrap(triggeredBy);
	}

	#endregion

	#region Methods

	private void ShouldTriggerTrap(Collider triggeredBy)
	{
		if (linkedTrap == null ||
			IsObstacleDisabled())
		{
			return;
		}

		if (linkedTrap.GetComponent<PickUpObject>().GetCurrentState() == PickUpState.Held)
		{
			return;
		}

		if (triggeredBy.gameObject.tag.Contains(canTrigger.ToString()) ||
			canTrigger == CanTrigger.Any)
		{
			linkedTrap.TriggerTrap(triggeredBy);
		}
	}

	#endregion

	#region Enums

	public enum CanTrigger
	{
		Friendly,
		Hostile,
		Any
	}

	#endregion
}