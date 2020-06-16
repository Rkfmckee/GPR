using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightedByMouse : MonoBehaviour
{
    public GameObject[] objectsToHighlight;
    public Color hightlightColour;

    private new Camera camera;
    private Shader outlineShader;

    private GameObject currentObjectHit;
    private GameObject previousObjectHit;
    private GameObject validObjectHit;

    void Awake() {
        camera = Camera.main;
    }

    void Update() {
        RaycastHit hit;
        Ray cameraToMouseRay = camera.ScreenPointToRay(Input.mousePosition);
        validObjectHit = null;

        if (Physics.Raycast(cameraToMouseRay, out hit)) {

            foreach(var objectToHighlight in objectsToHighlight) {
                if (hit.collider.gameObject.tag == objectToHighlight.tag) {
                    validObjectHit = hit.collider.gameObject;
                    break;
                }
            }

            if (validObjectHit != currentObjectHit) {
                previousObjectHit = currentObjectHit;
                currentObjectHit = validObjectHit;

                if (previousObjectHit != null) {
                    previousObjectHit.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
                }

                if (currentObjectHit != null) {
                    currentObjectHit.GetComponent<Renderer>().material.shader = Shader.Find("Outline");
                }
            }

            ifObjectIsPlayerAllowSwitching(validObjectHit);

            Debug.DrawRay(cameraToMouseRay.origin, cameraToMouseRay.direction, Color.yellow);
        } else {
            Debug.DrawRay(cameraToMouseRay.origin, cameraToMouseRay.direction, Color.white);
        }
    }

    private void ifObjectIsPlayerAllowSwitching(GameObject objectHighlighted) {
        var allPlayerObjects = GameObject.FindGameObjectsWithTag("Player");

        if (objectHighlighted != null) {
            if (objectHighlighted.tag == "Player") {
                if (Input.GetMouseButtonDown(0)) {
                    foreach (GameObject player in allPlayerObjects) {
                        player.GetComponent<PlayerBehaviour>().SetCurrentlyBeingControlled(false);
                    }

                    objectHighlighted.GetComponent<PlayerBehaviour>().SetCurrentlyBeingControlled(true);
                }
            }
        }
    }
}
