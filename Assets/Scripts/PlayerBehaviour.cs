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
    }

    private void FixedUpdate() {
        CalculateMovement();
    }

    #endregion

    #region Methods

    public void SetCurrentlyBeingControlled(bool isControlled) {
        currentlyBeingControlled = isControlled;

        if (currentlyBeingControlled) { 
            References.currentPlayer = gameObject;

            var cameraController = Camera.main.GetComponent<CameraController>();

            if (cameraController != null) {
                cameraController.SetCurrentlyControlledPlayer(gameObject);
            }
        }

        ChangeMassIfNotBeingControlled();
    }

    private void SetupAwakeInstanceVariables() {
        References.players.Add(gameObject);
        if (currentlyBeingControlled) { References.currentPlayer = gameObject; }
        rigidbody = GetComponent<Rigidbody>();
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

    #endregion
}
