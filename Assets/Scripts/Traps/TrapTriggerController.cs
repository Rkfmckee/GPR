using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTriggerController : MonoBehaviour
{
    #region Properties

    public TrapController trapToTrigger;

    #endregion

    #region Events

    private void OnCollisionEnter(Collision collision) {
        Collider triggeredBy = collision.collider;

        if (triggeredBy.gameObject.tag == "Player") {
            if (trapToTrigger != null) {
                trapToTrigger.TriggerTrap(triggeredBy);
            }
        }
    }

    private void OnTriggerEnter(Collider triggeredBy) {
        if (triggeredBy.gameObject.tag == "Player") {
            if (trapToTrigger != null) {
                trapToTrigger.TriggerTrap(triggeredBy);
            }
        }
    }

    #endregion
}