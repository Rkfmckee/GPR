using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public abstract class TrapController : MonoBehaviour {
    #region Properties

    protected SurfaceType trapType;

    #endregion

    #region Methods

    public abstract void TriggerTrap(Collider triggeredBy);

    public SurfaceType GetSurfaceType() { return trapType; }

    #endregion

    #region Enums

    public enum SurfaceType {
        FLOOR,
        WALL,
		CEILING,
		ANY
    }

    #endregion
}
