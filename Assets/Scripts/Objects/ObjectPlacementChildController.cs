using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacementChildController : MonoBehaviour
{
	private ObjectPlacementController parentController;
	private new BoxCollider collider;

	private void Awake() {
		parentController = transform.parent.GetComponent<ObjectPlacementController>();
		collider = GetComponent<BoxCollider>();
	}

    private void OnCollisionEnter(Collision other) {
		//parentController.OnCollisionEnter(collider, other);
	}
}
