using System;
using UnityEngine;

internal class FriendlyStatePickupObject : FriendlyState {
	#region Properties
	
	private bool isHoldingObject;
	private float interactionDistance;

	private GameObject objectToPickup;
	private GameObject heldObject;
	private PickUpObject heldObjectPickup;
	private GameObject objectPlacement;
	private ObjectPlacementController objectPlacementController;

	#endregion

	#region Constructor

	public FriendlyStatePickupObject(GameObject gameObj, GameObject objToPickup) : base(gameObj){
		objectToPickup = objToPickup;
		isHoldingObject = false;
		interactionDistance = 3;

		EnableObjectPlacement(objectToPickup);
	}

	#endregion

	#region Events

	public override void Update() {
		base.Update();

		if (!isHoldingObject) {
			navMeshAgent.SetDestination(objectToPickup.transform.position);

			if (IsWithinInteractionDistance(transform.position, objectToPickup.transform.position)) {
				navMeshAgent.isStopped = true;
				SetHeldObject();
			}
		} else {
			if (objectPlacement == null || objectPlacementController == null)
				return;

			if (objectPlacementController.IsPositionFinalized()) {
				var positionToPlace = objectPlacement.transform.position;
				var rotationToPlace = objectPlacement.transform.rotation;
				navMeshAgent.isStopped = false;
				navMeshAgent.SetDestination(positionToPlace);

				if (IsWithinInteractionDistance(transform.position, positionToPlace)) {
					PlaceHeldObject(positionToPlace, rotationToPlace);
				}
			}
		}
	}



	#endregion

	#region Methods

	private void EnableObjectPlacement(GameObject obj) {
		References.GameController.gameTraps.EnableObjectPlacementIfPossible(obj);

		objectPlacement = References.GameController.gameTraps.objectPlacement;
		objectPlacementController = References.GameController.gameTraps.objectPlacementController;

		objectPlacementController.SetHeldObject(obj);
	}

	private bool IsWithinInteractionDistance(Vector3 position, Vector3 targetPosition) {
		targetPosition.y = 0;
		return Vector3.Distance(position, targetPosition) < interactionDistance;
	}

	private void SetHeldObject() {
		heldObject = objectToPickup;
        heldObject.transform.parent = transform;
        heldObjectPickup = heldObject.GetComponent<PickUpObject>();

		isHoldingObject = true;
		heldObjectPickup.SetCurrentState(PickUpObject.State.Held);
		objectToPickup = null;
	}

	private void PlaceHeldObject(Vector3 position, Quaternion rotation) {
		(Vector3 validPosition, Quaternion validRotation) = GetValidPositionAndRotation(position, rotation);
		
		heldObjectPickup.SetCurrentState(PickUpObject.State.Idle);
		heldObject.transform.position = validPosition;
		heldObject.transform.rotation = validRotation;

		ForgetHeldObject();

		behaviour.SetCurrentState(new FriendlyStateIdle(gameObject));
	}

	private (Vector3, Quaternion) GetValidPositionAndRotation(Vector3 position, Quaternion rotation) {
        TrapController trapController = heldObject.GetComponent<TrapController>();

        if (trapController == null) {
			var minY = heldObject.GetComponentInChildren<Collider>().bounds.size.y / 2;

			if (position.y <= minY) {
				position.y = minY;
			}

			return (position, rotation);
		}

		if (trapController.GetSurfaceType() == TrapController.SurfaceType.WALL) {
			Vector3 hitNormal = objectPlacementController.hitInformation.normal;
			rotation = Quaternion.LookRotation(hitNormal);

			if (Math.Abs(hitNormal.x) == 1) {
				position.x = objectPlacementController.hitInformation.point.x;
			} else if (Math.Abs(hitNormal.z) == 1) {
				position.z = objectPlacementController.hitInformation.point.z;
			}
		} else {
			position.y = 0;
		}

        return (position, rotation);
	}

	private void ForgetHeldObject() {
        if (heldObject != null) {
            heldObject.transform.parent = null;
            heldObject = null;

            References.GameController.gameTraps.DisableObjectPlacement();
        }
    }

	#endregion
}