using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour {
	#region Properties

	public float maxHealth;
	public float healthBarHeightOffset;

	private float currentHealth;
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
		Transform healthBarParent = canvas.transform.Find("HealthBars").transform;

		healthBar = Instantiate(healthBarPrefab, healthBarParent);
		healthBar.name = $"{name}HealthBar";
		healthBarController = healthBar.GetComponent<HealthBarController>();

		if (gameObject.tag == "Trap" || gameObject.tag == "Trigger") {
			healthBar.SetActive(false);
		}
	}

	private void LateUpdate() {
		healthBar.transform.position = camera.WorldToScreenPoint(transform.position + new Vector3(0, healthBarHeightOffset, 0));
		healthBarController.ShowHealthFraction(currentHealth / maxHealth);
	}

	private void OnDestroy() {
		Destroy(healthBar);
	}

	#endregion

	#region Methods

	public float GetCurrentHealth() {
		return currentHealth;
	}

	public GameObject GetHealthBar() {
		return healthBar;
	}

	private void SetupInstanceVariables() {
		currentHealth = maxHealth;

		camera = Camera.main;
		healthBarPrefab = Resources.Load("Prefabs/UI/HealthBar") as GameObject;
	}

	public void Heal(float amount) {
		float newHealth = currentHealth += amount;

		if (newHealth > maxHealth) {
			currentHealth = maxHealth;
		} else {
			currentHealth = newHealth;
		}
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
