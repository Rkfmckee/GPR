using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldObjectController : MonoBehaviour {
    #region Properties

	private GameObject objectPlacement;
	private ObjectPlacementController objectPlacementController;
    private GameObject heldObject;
    private float throwStrength;
    private Vector3 throwHeight;

    private PickUpController heldObjectController;

    #endregion

    #region Events

    private void Awake() {
        throwStrength = 5000;
        throwHeight = transform.up / 10;

        if (gameObject.tag == "Player") {
            References.Player.playerHoldController = this;
        }
    }

    private void Update() {
        UseHeldObjectIfPressed();
    }

    private void OnDestroy() {
        ForgetHeldObject();
    }

    #endregion

    #region Methods

    public bool isHoldingObject() {
        return heldObject != null;
    }

    public void SetHeldObject(GameObject objectHeld) {
        heldObject = objectHeld;
        heldObject.transform.parent = transform;
        heldObjectController = heldObject.GetComponent<PickUpController>();

		References.GameController.gameTraps.EnableObjectPlacementIfPossible(heldObject, heldObjectController.canBeThrown);

		objectPlacement = References.GameController.gameTraps.objectPlacement;
		objectPlacementController = References.GameController.gameTraps.objectPlacementController;
    }

    private void UseHeldObjectIfPressed() {
        if (heldObject == null) return;

        if (Input.GetButtonDown("Fire1")) {
            if (objectPlacementController != null) {
                if (objectPlacementController.validPlacement) {
                    PlaceObject();
                }
            }
        } else if (Input.GetButtonDown("Fire2")) {
            if (heldObjectController.canBeThrown) {
                ThrowObject();
            }
        }
    }

    private void PlaceObject() {
		if (objectPlacement == null || objectPlacementController == null) {
			print("Object Placement hasn't been created");
			return;
		}

        float xPosition = objectPlacement.transform.position.x;
        float yPosition = objectPlacement.transform.position.y;
        float zPosition = objectPlacement.transform.position.z;
        Quaternion rotation = heldObject.transform.rotation;

        TrapController trapController = heldObject.GetComponent<TrapController>();
        if (trapController != null) {
            if (trapController.GetSurfaceType() == TrapController.SurfaceType.WALL) {
                Vector3 hitNormal = objectPlacementController.hitInformation.normal;
                rotation = Quaternion.LookRotation(objectPlacementController.hitInformation.normal);

                if (Math.Abs(hitNormal.x) == 1) {
                    xPosition = objectPlacementController.hitInformation.point.x;
                } else if (Math.Abs(hitNormal.z) == 1) {
                    zPosition = objectPlacementController.hitInformation.point.z;
                }
            } else {
                yPosition = 0;
            }
        } else {
            yPosition = heldObject.GetComponentInChildren<Collider>().bounds.size.y / 2;
        }

        heldObject.GetComponent<PickUpController>().SetCurrentState(PickUpController.State.Idle);
        heldObject.transform.position = new Vector3(xPosition, yPosition, zPosition);
        heldObject.transform.rotation = rotation;

        ForgetHeldObject();
    }

    private void ThrowObject() {
        Vector3 throwDirection = (transform.forward + throwHeight) * throwStrength;

        heldObject.GetComponent<PickUpController>().SetCurrentState(PickUpController.State.Thrown);
        heldObject.GetComponent<Rigidbody>().AddForce(throwDirection);


        ForgetHeldObject();
    }

    private void ForgetHeldObject() {
        if (heldObject != null) {
            heldObject.transform.parent = null;
            heldObject = null;

            References.GameController.gameControllerObject.GetComponent<LookForHighlightableObjects>().ResetDontSelectTimer();
            References.GameController.gameTraps.DisableObjectPlacement();
        }
    }

    #endregion
}
