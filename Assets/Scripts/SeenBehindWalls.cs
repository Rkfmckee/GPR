using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeenBehindWalls : MonoBehaviour
{
    private new GameObject camera;

    private GameObject currentObjectHit;
    private GameObject previousObjectHit;
    private GameObject nextObjectHit;

    private Material wallMaterialSolid;
    private Material wallMaterialTranslucent;

    void Awake() {
        camera = GameObject.FindGameObjectWithTag("MainCamera");

        wallMaterialSolid = Resources.Load("Materials/WallMaterial") as Material;
        wallMaterialTranslucent = Resources.Load("Materials/WallMaterialTranslucent") as Material;
    }

    void Update() {
        seeObjectThroughWalls();
    }

    private void seeObjectThroughWalls() {
        RaycastHit hit;
        Vector3 fromCameraToObject = -(camera.transform.position - transform.position);

        if (Physics.Raycast(camera.transform.position, fromCameraToObject, out hit)) {
            
            nextObjectHit = hit.collider.gameObject;

            if (nextObjectHit != currentObjectHit) {
                previousObjectHit = currentObjectHit;
                currentObjectHit = nextObjectHit;

                if (previousObjectHit != null) {
                    if (previousObjectHit.tag == "Wall") {
                        previousObjectHit.GetComponent<Renderer>().material = wallMaterialSolid;
                    }
                }

                if (currentObjectHit != null) {
                    if (currentObjectHit.tag == "Wall") {
                        currentObjectHit.GetComponent<Renderer>().material = wallMaterialTranslucent;
                    }
                }
            }

            Debug.DrawRay(camera.transform.position, fromCameraToObject * hit.distance, Color.yellow);

        } else {
            Debug.DrawRay(camera.transform.position, fromCameraToObject * 1000, Color.white);
        }
    }
}
