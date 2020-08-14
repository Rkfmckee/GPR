using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldObjectController : MonoBehaviour {
    #region Properties

    private GameObject heldObject;
    private float throwStrength;
    private Vector3 throwHeight;

    private PickUpController heldObjectController;

    #endregion

    #region Events

    private void Awake() {
        throwStrength = 5000;
        throwHeight = transform.up / 10;
    }

    private void Update() {
        UseHeldObjectIfPressed();
    }

    private void OnDestroy() {
        ForgetHeldObject();
    }

    #endregion

    #region Methods

    public void SetHeldObject(GameObject objectHeld) {
        heldObject = objectHeld;
        heldObject.transform.parent = transform;
        heldObjectController = heldObject.GetComponent<PickUpController>();

        TrapController.Type? trapType = null;
        if (heldObject.tag == "Trap") {
            trapType = heldObject.GetComponent<TrapController>().GetTrapType();
        }

        References.GameController.gameTraps.EnableWorldMousePointerIfPossible(trapType);
    }

    private void UseHeldObjectIfPressed() {
        if (heldObject == null) return;

        if (Input.GetButtonDown("Fire1")) {
            if (References.GameController.gameTraps.worldMousePointer != null) {
                if (References.GameController.gameTraps.worldMousePointer.GetComponent<ObjectPlacementPointer>().validPlacement) {
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
        GameObject worldPointer = References.GameController.gameTraps.worldMousePointer;
        ObjectPlacementPointer worldPointerScript = worldPointer.GetComponent<ObjectPlacementPointer>();
        float xPosition = worldPointer.transform.position.x;
        float yPosition = worldPointer.transform.position.y;
        float zPosition = worldPointer.transform.position.z;
        Quaternion rotation = heldObject.transform.rotation;

        TrapController trapController = heldObject.GetComponent<TrapController>();
        if (trapController != null) {
            if (trapController.GetTrapType() == TrapController.Type.Wall) {
                Vector3 hitNormal = worldPointerScript.hitInformation.normal;
                rotation = Quaternion.LookRotation(worldPointerScript.hitInformation.normal);

                if (Math.Abs(hitNormal.x) == 1) {
                    xPosition = worldPointerScript.hitInformation.point.x;
                } else if (Math.Abs(hitNormal.z) == 1) {
                    zPosition = worldPointerScript.hitInformation.point.z;
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
            References.GameController.gameTraps.DisableWorldMousePointer();
        }
    }

    #endregion
}
