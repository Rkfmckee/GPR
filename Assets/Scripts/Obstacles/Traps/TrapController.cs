using UnityEngine;

public abstract class TrapController : TrapTriggerBase {

    #region Properties

	private TriggerController linkedTrigger;

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

    public abstract void TriggerTrap(Collider triggeredBy);

    #endregion
}
