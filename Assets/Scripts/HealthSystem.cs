using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    #region Properties

    public float maxHealth;

    public float currentHealth;
    private float damageOverTimeTimer;
    private float damageToTakeOverTime;

    #endregion

    #region Events

    private void Awake() {
        currentHealth = maxHealth;
    }

    #endregion

    #region Methods

    public void TakeDamage(float damage) {
        currentHealth -= damage;

        if (currentHealth <= 0) {
            Destroy(gameObject);
        }
    }

    public void TakeDamageOverTime(float totalDamage, float timeInSeconds) {
        StartCoroutine(DamageOverTime(totalDamage, timeInSeconds));
    }

    #endregion

    #region Coroutines

    private IEnumerator DamageOverTime(float totalDamage, float timeInSeconds) {
        float targetHealth = currentHealth - totalDamage;
        
        while (currentHealth > targetHealth) {
            var decreaseIncrement = (totalDamage / timeInSeconds) * Time.deltaTime;

            if (currentHealth - decreaseIncrement < targetHealth) {
                currentHealth = targetHealth;
            } else {
                currentHealth -= decreaseIncrement;
            }

            print(currentHealth);

            yield return null;
        }
    }

    #endregion
}
