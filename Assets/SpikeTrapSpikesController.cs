using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapSpikesController : MonoBehaviour
{
    #region Events

    private void OnTriggerEnter(Collider triggeredBy) {
        var targetsHealthSystem = null as HealthSystem;
        if ((targetsHealthSystem = triggeredBy.gameObject.GetComponent<HealthSystem>()) != null) {
            targetsHealthSystem.TakeDamageOverTime(5, 3);
        }
    }

    #endregion
}
