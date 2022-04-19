using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
	#region Properties

	private Image currentHealth;

	#endregion

	#region Events

	private void Awake() {
		currentHealth = transform.Find("BarAmount").GetComponent<Image>();
	}

	#endregion

	#region Methods

	public void ShowHealthFraction(float fraction) {
		currentHealth.transform.localScale = new Vector3(fraction, 1, 1);
	}

	#endregion
}
