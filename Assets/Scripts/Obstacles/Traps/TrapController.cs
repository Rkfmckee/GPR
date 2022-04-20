using UnityEngine;

public abstract class TrapController : TrapTriggerBase {

    #region Methods

    public abstract void TriggerTrap(Collider triggeredBy);

    #endregion
}
