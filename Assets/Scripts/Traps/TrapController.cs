using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public abstract class TrapController : MonoBehaviour {
    #region Properties

    protected float trapHealth;
    protected float maxHealth;
    protected Type trapType;

    #endregion

    #region Methods

    public abstract void TriggerTrap(Collider triggeredBy);

    public float GetTrapHealth() { return trapHealth; }

    public Type GetTrapType() { return trapType; }

    public void FixTrap(float amount) {
        float newHealth = trapHealth += amount;
        
        if (newHealth > maxHealth) {
            trapHealth = maxHealth;
        } else {
            trapHealth = newHealth;
        }
    }

    public void DamageTrap(float amount) {
        float newHealth = trapHealth -= amount;

        if (newHealth < 0) {
            trapHealth = 0;
        } else {
            trapHealth = newHealth;
        }
    }

    #endregion

    #region Enums

    public enum Type {
        Floor,
        Wall
    }

    #endregion
}
