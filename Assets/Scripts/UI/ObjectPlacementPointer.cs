using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacementPointer : MonoBehaviour
{
    #region Properties

    [HideInInspector]
    public bool validPlacement;
    public float maxPlacementDistance;
    public LayerMask floorMask;
    public Color invalidPositionColour;
    
    private new Camera camera;
    private Ray cameraToMouseRay;
    private Color defaultColour;

    private new MeshRenderer renderer;

    #endregion

    #region Events

    private void Awake() {
        camera = Camera.main;
        renderer = gameObject.GetComponent<MeshRenderer>();
        defaultColour = renderer.material.color;
        validPlacement = true;
    }

    private void Update() {
        RaycastHit hit;
        cameraToMouseRay = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(cameraToMouseRay, out hit)) {
            Vector3 pointHit = hit.point;

            if (hit.collider.gameObject.tag == "Floor") {
                validPlacement = true;

                pointHit = GetValidFloorPosition(pointHit);
            } else {
                validPlacement = false;
            }

            transform.position = pointHit;
        }

        ChangeColourIfNotValidPlacement();
    }

    #endregion

    #region Methods

    private Vector3 GetValidFloorPosition(Vector3 pointHit) {
        Vector3 playerToPointHit = pointHit - References.currentPlayer.transform.position;

        if (Vector3.Distance(References.currentPlayer.transform.position, pointHit) > maxPlacementDistance) {
            pointHit = References.currentPlayer.transform.position + (playerToPointHit.normalized * maxPlacementDistance);
        }

        return pointHit;
    }

    private void ChangeColourIfNotValidPlacement() {
        if (validPlacement) {
            renderer.material.color = defaultColour;
        } else {
            renderer.material.color = invalidPositionColour;
        }
    }

    #endregion
}
