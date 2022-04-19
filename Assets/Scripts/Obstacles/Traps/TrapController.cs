using UnityEngine;

public abstract class TrapController : MonoBehaviour {
    #region Properties

	[SerializeField]
    protected SurfaceType surfaceType;

    #endregion

    #region Methods

    public abstract void TriggerTrap(Collider triggeredBy);

    public SurfaceType GetSurfaceType() { return surfaceType; }

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
