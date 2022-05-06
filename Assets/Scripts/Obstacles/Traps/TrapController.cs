using UnityEngine;
using static PickUpObject;

public abstract class TrapController : ObstacleController {

    #region Properties

	private TriggerController linkedTrigger;

	#endregion

	#region Events

	protected override void Awake() {
		base.Awake();
		
		References.Obstacles.traps.Add(gameObject);
	}

	#endregion
	
	#region Methods

		#region Get/Set

		public TriggerController GetLinkedTrigger() {
			return linkedTrigger;
		}

		public virtual void SetLinkedTrigger(TriggerController trigger) {
			linkedTrigger = trigger;
		}

		#endregion

    public virtual void TriggerTrap(Collider triggeredBy) {
	}

    #endregion
}
