using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    #region Properties

    public bool currentlyBeingControlled;
    public float movementSpeed;

    private new Rigidbody rigidbody;
    private GameObject gameController;

    private Vector3 movementAmount;
    private Vector3 directionVector;
    private GameObject heldObject;
    private Vector3 throwVerticalOffset;

    #endregion

    #region Events

    private void Awake() {
        SetupAwakeInstanceVariables();
    }

    private void Start() {
        SetupStartInstanceVariables();
        ChangeMassIfNotBeingControlled();
    }

    private void Update() {
        if (!currentlyBeingControlled) {
            directionVector = Vector3.zero;
            return;
        }

        GetMovementDirection();

        UseHeldObjectIfPressed();
    }

    private void FixedUpdate() {
        CalculateMovement();
    }

    #endregion

    #region Methods

    public void SetCurrentlyBeingControlled(bool isControlled) {
        currentlyBeingControlled = isControlled;
        if (currentlyBeingControlled) { References.currentPlayer = gameObject; }

        ChangeMassIfNotBeingControlled();

        var cameraController = Camera.main.GetComponent<CameraController>();

        if (cameraController != null) {
            cameraController.SetCurrentlyControlledPlayer(gameObject);
        }
    }

    public void SetHeldObject(GameObject objectHeld) {
        heldObject = objectHeld;
        heldObject.transform.parent = transform;
    }

    private void SetupAwakeInstanceVariables() {
        References.players.Add(gameObject);
        if (currentlyBeingControlled) { References.currentPlayer = gameObject; }
        rigidbody = GetComponent<Rigidbody>();

        throwVerticalOffset = transform.up / 10;
    }

    private void SetupStartInstanceVariables() {
        gameController = References.gameController;
    }

    private void GetMovementDirection() {
        var zDirection = Input.GetAxis("Vertical");
        var xDirection = Input.GetAxis("Horizontal");
        directionVector = new Vector3(xDirection, 0, zDirection);

        directionVector = NormaliseVectorToKeepDeceleration(directionVector);
    }

    private void CalculateMovement() {
        movementAmount = directionVector * (movementSpeed * Time.fixedDeltaTime);
        var newPosition = transform.position + movementAmount;

        rigidbody.MovePosition(newPosition);
        transform.LookAt(newPosition);
    }

    private Vector3 NormaliseVectorToKeepDeceleration(Vector3 vector) {
        // Normalizing a decimal vector rounds it to 1, which causes weird deceleration
        // So don't do that if it's between 1 and -1

        if ((vector.magnitude > 1) || (vector.magnitude < -1)) {
            vector = vector.normalized;
        }

        return vector;
    }

    private void ChangeMassIfNotBeingControlled() {
        if (currentlyBeingControlled) {
            rigidbody.mass = 1;
        } else {
            rigidbody.mass = 10;
        }
    }

    private void UseHeldObjectIfPressed() {
        if (heldObject == null) return;

        if (Input.GetButtonDown("Fire1")) {
            heldObject.GetComponent<ObstacleController>().SetCurrentState(ObstacleController.State.THROWN);
            heldObject.GetComponent<Rigidbody>().AddForce((transform.forward + throwVerticalOffset) * 5000);

            heldObject.transform.parent = null;
            heldObject = null;
        }
    }

    #endregion
}
