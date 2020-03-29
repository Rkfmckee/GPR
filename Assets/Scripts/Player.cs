using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 movementAmount;
    private float movementSpeed;
    private float zSpeed;
    private float xSpeed;

    void Start() {
        movementSpeed = 3 * Time.deltaTime;
    }

    void Update() {
        calculateMovement();
    }

    private void calculateMovement() {
        zSpeed = Input.GetAxis("Vertical") * movementSpeed;
        xSpeed = Input.GetAxis("Horizontal") * movementSpeed;
        movementAmount = new Vector3(xSpeed, 0, zSpeed);

        transform.position += movementAmount;
    }
}
