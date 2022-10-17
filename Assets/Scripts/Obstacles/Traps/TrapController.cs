using UnityEngine;

public abstract class TrapController : ObstacleController
{
	#region Fields

	private TriggerController linkedTrigger;

	#endregion

	#region Properties

	public TriggerController LinkedTrigger { get => linkedTrigger; }

	#endregion

	#region Events

	protected override void Awake()
	{
		base.Awake();

		References.Obstacles.traps.Add(gameObject);
	}

	#endregion

	#region Methods

	public virtual void SetLinkedTrigger(TriggerController trigger)
	{
		linkedTrigger = trigger;
	}

	public virtual void TriggerTrap(Collider triggeredBy)
	{
		if (ObstacleDisabled)
		{
			return;
		}
	}

	#endregion
}
