using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public abstract class TrapController : MonoBehaviour {
    #region Properties

    protected Type trapType;

    #endregion

    #region Methods

    public abstract void TriggerTrap(Collider triggeredBy);

    public Type GetTrapType() { return trapType; }

    #endregion

    #region Enums

    public enum Type {
        Floor,
        Wall
    }

    #endregion
}
