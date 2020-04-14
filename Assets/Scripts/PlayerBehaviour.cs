using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    #region Properties

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
        calculateMovement();
    }

    #endregion

    #region Methods

    private void setupInstanceVariables() {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void calculateMovement() {
        zSpeed = Input.GetAxis("Vertical") * movementSpeed;
        xSpeed = Input.GetAxis("Horizontal") * movementSpeed;
        movementAmount = new Vector3(xSpeed, 0, zSpeed);
        var newPosition = transform.position + movementAmount;

        rigidbody.velocity = movementAmount;
        transform.LookAt(newPosition);
    }

    #endregion
}
