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

    private GameObject currentObjectHit;
    private GameObject previousObjectHit;
    private GameObject nextObjectHit;


    #endregion

    #region Events

    private void Awake() {
        setupInstanceVariables();

        currentObjectHit = null;
        previousObjectHit = null;
        nextObjectHit = null;
    }

    private void Start() {
    }

    private void Update() {
        calculateMovement();
        seePlayerThroughWalls();
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

    private void seePlayerThroughWalls() {
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        RaycastHit hit;
        Vector3 fromCameraToPlayer = -(camera.transform.position - transform.position);
        Material solidMaterial = Resources.Load("Materials/WallMaterial") as Material;
        Material translucentMaterial = Resources.Load("Materials/WallMaterialTranslucent") as Material;

        if (Physics.Raycast(camera.transform.position, fromCameraToPlayer, out hit)) {
            Debug.DrawRay(camera.transform.position, fromCameraToPlayer * hit.distance, Color.yellow);

            nextObjectHit = hit.collider.gameObject;

            if (nextObjectHit != currentObjectHit) {
                previousObjectHit = currentObjectHit;
                currentObjectHit = nextObjectHit;

                if (previousObjectHit != null) {
                    if (previousObjectHit.tag == "Wall") {
                        previousObjectHit.GetComponent<Renderer>().material = solidMaterial;
                    }
                }

                if (currentObjectHit != null) {
                    if (currentObjectHit.tag == "Wall") {
                        currentObjectHit.GetComponent<Renderer>().material = translucentMaterial;
                    }
                }
            }

        } else {
            Debug.DrawRay(camera.transform.position, fromCameraToPlayer * 1000, Color.white);
        }
    }

    #endregion
}
