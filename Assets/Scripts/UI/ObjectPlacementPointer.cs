using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacementPointer : MonoBehaviour {
    #region Properties

    [HideInInspector]
    public bool validPlacement;
    [HideInInspector]
    public RaycastHit hitInformation;
    public float maxPlacementDistance;
    public Color invalidPositionColour;
    public TrapController.Type? trapType;

    private new Camera camera;
    private Ray cameraToMouseRay;
    private Color defaultColour;
    private int terrainMask;
    private int floorMask;
    private int wallMask;

    private new MeshRenderer renderer;

    #endregion

    #region Events

    private void Awake() {
        camera = Camera.main;
        renderer = gameObject.GetComponent<MeshRenderer>();
        defaultColour = renderer.material.color;

        floorMask = 1 << LayerMask.NameToLayer("Floor");
        wallMask = 1 << LayerMask.NameToLayer("Wall");
        terrainMask = floorMask | wallMask;
    }

    private void Update() {
        CheckForRaycastHit();

        ChangeColourIfNotValidPlacement();
    }

    #endregion

    #region Methods

    private void CheckForRaycastHit() {
        //RaycastHit[] allHits;
        RaycastHit validHit;
        cameraToMouseRay = camera.ScreenPointToRay(Input.mousePosition);
        validPlacement = true;

        //allHits = Physics.RaycastAll(cameraToMouseRay, Mathf.Infinity, terrainMask);

        //RaycastHit closestHit = allHits[0];
        //foreach(RaycastHit hit in allHits) {
        //    if (hit.distance < closestHit.distance) {
        //        if (hit.transform.gameObject.GetComponent<Renderer>().enabled == true) {
        //            closestHit = hit;
        //        }
        //    }
        //}
        //validHit = closestHit;

        if (Physics.Raycast(cameraToMouseRay, out validHit, Mathf.Infinity, terrainMask)) {
            hitInformation = validHit;
            Vector3 pointHit = validHit.point;

            if (trapType == null || validHit.collider.gameObject.tag == trapType.ToString()) {
                validPlacement = GetValidPosition(pointHit);
            } else {
                validPlacement = false;
            }

            transform.position = pointHit;
        }
    }

    private bool GetValidPosition(Vector3 pointHit) {
        bool valid = true;

        if (Vector3.Distance(References.currentPlayer.transform.position, pointHit) > maxPlacementDistance) {
            valid = false;
        }

        return valid;
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
