using System.Collections.Generic;
using UnityEngine;

public class LaunchingTrap : TrapController {

    #region Properties

	[SerializeField]
	protected float ammoSpeed;
	[SerializeField]
	protected float ammoScale;

    protected GameObject ammoPrefab;
	private List<GameObject> launchingSlots;

    #endregion

	#region Events

	protected override void Awake() {
		base.Awake();

		launchingSlots = new List<GameObject>();
        foreach(Transform child in transform) {
            launchingSlots.Add(child.gameObject);
        }
	}

	#endregion

	#region Methods

    public override void TriggerTrap(Collider triggeredBy) {
		base.TriggerTrap(triggeredBy);

        foreach(GameObject slot in launchingSlots) {
            GameObject ammo = Instantiate(ammoPrefab);
            LaunchingAmmo ammoController = ammo.GetComponent<LaunchingAmmo>();
            Rigidbody ammoRigidbody = ammo.GetComponent<Rigidbody>();

            ammo.transform.parent = transform;
			ammo.transform.localRotation = Quaternion.Euler(Vector3.zero);
			ammo.transform.localScale = Vector3.one * ammoScale;
            ammo.transform.position = slot.transform.position;

            if (ammoController != null) {
                ammoController.SetCollidersToIgnore(Physics.OverlapSphere(ammo.transform.position, 1));
            }

            if (ammoRigidbody != null) {
				ammoRigidbody.AddForce(ammo.transform.forward * ammoSpeed, ForceMode.Impulse);
            }
        }
    }

	#endregion
}
