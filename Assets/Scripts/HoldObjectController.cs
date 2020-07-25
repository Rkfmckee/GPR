using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldObjectController : MonoBehaviour
{
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

    #endregion

    #region Methods

    public void SetHeldObject(GameObject objectHeld) {
        heldObject = objectHeld;
        heldObject.transform.parent = transform;

        heldObjectController = heldObject.GetComponent<PickUpController>();

        References.gameController.GetComponent<GameController>().EnableWorldMousePointer(heldObject.tag == "Trap");
    }

    private void UseHeldObjectIfPressed() {
        if (heldObject == null) return;

        if (Input.GetButtonDown("Fire1")) {
            if (References.gameController.GetComponent<GameController>().worldMousePointer.GetComponent<ObjectPlacementPointer>().validPlacement) {
                SetDownObject();
            }
        } else if (Input.GetButtonDown("Fire2")) {
            if (heldObjectController.canBeThrown) {
                ThrowObject();
            }
        }
    }

    private void SetDownObject() {
        GameObject worldPointer = References.gameController.GetComponent<GameController>().worldMousePointer;
        float xPosition = worldPointer.transform.position.x;
        float zPosition = worldPointer.transform.position.z;
        float yPosition;

        if (heldObject.tag == "Trap") {
            yPosition = 1;
        } else {
            yPosition = 3;
        }

        heldObject.GetComponent<PickUpController>().SetCurrentState(PickUpController.State.Idle);
        heldObject.transform.position = new Vector3(xPosition, yPosition, zPosition);

        ForgetHeldObject();
        References.gameController.GetComponent<GameController>().DisableWorldMousePointer();
    }

    private void ThrowObject() {
        Vector3 throwDirection = (transform.forward + throwHeight) * throwStrength;

        heldObject.GetComponent<PickUpController>().SetCurrentState(PickUpController.State.Thrown);
        heldObject.GetComponent<Rigidbody>().AddForce(throwDirection);

        ForgetHeldObject();
        References.gameController.GetComponent<GameController>().DisableWorldMousePointer();
    }

    private void ForgetHeldObject() {
        heldObject.transform.parent = null;
        heldObject = null;
    }

    #endregion
}
