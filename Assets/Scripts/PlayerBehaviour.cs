using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    #region Properties

    public bool currentlyBeingControlled;
    public float movementSpeed;

    private new Rigidbody rigidbody;

    private Vector3 movementAmount;
    private float zSpeed;
    private float xSpeed;

    #endregion

    #region Events

    private void Awake() {
        setupInstanceVariables();
    }

    private void Start() {
    }

    private void Update() {
        if (currentlyBeingControlled) {
            calculateMovement();
        }
    }

    #endregion

    #region Methods

    private void setupInstanceVariables() {
        rigidbody = GetComponent<Rigidbody>();
        movementSpeed = movementSpeed * Time.deltaTime;
    }

    private void calculateMovement() {
        zSpeed = Input.GetAxis("Vertical") * movementSpeed;
        xSpeed = Input.GetAxis("Horizontal") * movementSpeed;
        movementAmount = new Vector3(xSpeed, 0, zSpeed);
        var newPosition = transform.position + movementAmount;

        rigidbody.MovePosition(newPosition);
        transform.LookAt(newPosition);
    }

    #endregion
}
