using UnityEngine;

public class FireTrap : TrapController {

	#region Properties

	public float fireDurationTime;

	private float fireDurationTimer;

	private GameObject fire;

	#endregion

	#region Events

	protected override void Awake() {
		base.Awake();

		fire = transform.Find("Fire").gameObject;

		fireDurationTimer = fireDurationTime;
	}

	#endregion

	private void Update() {
		if (fireDurationTimer < fireDurationTime) {
			fireDurationTimer += Time.deltaTime;

			if (!fire.activeSelf)
				fire.SetActive(true);
		} else {
			if (fire.activeSelf)
				fire.SetActive(false);
		}
	}

	#region Methods

	public override void TriggerTrap(Collider triggeredBy) {
		base.TriggerTrap(triggeredBy);

		fireDurationTimer = 0;
	}

	#endregion
}
