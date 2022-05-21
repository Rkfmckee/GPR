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
	private HealthBar healthBarController;
	private GameObject bloodPrefab;
	private GameObject firePrefab;
	private new Collider collider;


	#endregion

	#region Events

	private void Awake() {
		collider = GetComponentInChildren<Collider>();

		healthBarPrefab = Resources.Load<GameObject>("Prefabs/UI/HealthBar");
		bloodPrefab = Resources.Load<GameObject>("Prefabs/Misc/Effects/BloodSplat");
		firePrefab = Resources.Load<GameObject>("Prefabs/Misc/Effects/Fire");

		currentHealth = maxHealth;
	}

	private void Start() {
		canvas = References.UI.canvas;
		camera = References.Camera.camera;
		Transform healthBarParent = canvas.transform.Find("HealthBars").transform;

		healthBar = Instantiate(healthBarPrefab, healthBarParent);
		healthBar.name = $"{name}HealthBar";
		healthBarController = healthBar.GetComponent<HealthBar>();

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

	public void TakeDamageOverTime(float totalDamage, float timeInSeconds, bool fireDamage) {
		StartCoroutine(DamageOverTime(totalDamage, timeInSeconds, fireDamage));
	}

	private void CheckIfNoHealth() {
		if (currentHealth <= 0) {
			Destroy(gameObject);
		}
	}

	#endregion

	#region Coroutines

	private IEnumerator DamageOverTime(float totalDamage, float timeInSeconds, bool fireDamage) {
		GameObject damageEffect;
		if (fireDamage) {
			damageEffect = Instantiate(firePrefab, transform);
		} else {
			damageEffect = Instantiate(bloodPrefab, transform);
		}

		damageEffect.transform.localPosition = Vector3.up * collider.bounds.extents.y;

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

		Destroy(damageEffect);
		CheckIfNoHealth();
	}

	#endregion
}
