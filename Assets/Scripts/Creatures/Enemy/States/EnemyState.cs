using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState
{
    #region Properties

    protected Vector3 movementTarget;

    #endregion

    #region Methods

    protected abstract Vector3 FindMovementTarget();

    #endregion
}