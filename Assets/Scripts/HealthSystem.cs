using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    #region Properties

    public float maxHealth;
    public float currentHealth;

    private new Camera camera;
    private GameObject canvas;
    private GameObject healthBarPrefab;
    private GameObject healthBar;
    private HealthBarController healthBarController;


    #endregion

    #region Events

    private void Awake() {
        SetupInstanceVariables();
    }

    private void Start() {
        canvas = References.UI.canvas;

        healthBar = Instantiate(healthBarPrefab, canvas.transform);
        healthBarController = healthBar.GetComponent<HealthBarController>();
    }

    private void LateUpdate() {
        healthBar.transform.position = camera.WorldToScreenPoint(transform.position + new Vector3(0, 1, 0));
        healthBarController.ShowHealthFraction(currentHealth / maxHealth);
    }

    private void OnDestroy() {
        Destroy(healthBar);
    }

    #endregion

    #region Methods

    private void SetupInstanceVariables() {
        currentHealth = maxHealth;

        camera = Camera.main;
        healthBarPrefab = Resources.Load("Prefabs/HealthBar") as GameObject;
    }

    public void TakeDamage(float damage) {
        currentHealth -= damage;

        CheckIfNoHealth();
    }

    public void TakeDamageOverTime(float totalDamage, float timeInSeconds) {
        StartCoroutine(DamageOverTime(totalDamage, timeInSeconds));
    }

    private void CheckIfNoHealth() {
        if (currentHealth <= 0) {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Coroutines

    private IEnumerator DamageOverTime(float totalDamage, float timeInSeconds) {
        float targetHealth = currentHealth - totalDamage;
        if (targetHealth < 0) { targetHealth = 0; }
        
        while (currentHealth > targetHealth) {
            var decreaseIncrement = (totalDamage / timeInSeconds) * Time.deltaTime;

            if (currentHealth - decreaseIncrement < targetHealth) {
                currentHealth = targetHealth;
            } else {
                currentHealth -= decreaseIncrement;
            }

            yield return null;
        }

        CheckIfNoHealth();
    }

    #endregion
}
