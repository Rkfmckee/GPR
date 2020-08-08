﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookForHighlightableObjects : MonoBehaviour {
    #region Properties

    private new Camera camera;
    private int highlightLayerMask;
    private GameObject lastHighlighted;

    #endregion

    #region Events

    private void Awake() {
        camera = Camera.main;

        int highlightableObjectLayerMask = 1 << LayerMask.NameToLayer("Highlightable");
        int obstacleLayerMask = 1 << LayerMask.NameToLayer("Obstacle");

        highlightLayerMask = highlightableObjectLayerMask | obstacleLayerMask;
    }

    private void Update() {
        RaycastHit hit;
        Ray cameraToMouse = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(cameraToMouse, out hit, Mathf.Infinity, highlightLayerMask)) {
            GameObject currentHit = hit.transform.gameObject;
            HighlightedByMouse highlightScript = currentHit.GetComponent<HighlightedByMouse>();

            if (highlightScript != null) {
                if (Vector3.Distance(currentHit.transform.position, References.currentPlayer.transform.position) < highlightScript.maxDistanceFromPlayer) {
                    if (currentHit != lastHighlighted) {
                        ClearLastHighlighted();

                        highlightScript.currentlyHightlightingMe = true;
                        lastHighlighted = currentHit;
                    }
                } else {
                    ClearLastHighlighted();
                }
            }
        } else {
            ClearLastHighlighted();
        }
    }

    #endregion

    #region Methods

    private void ClearLastHighlighted() {
        if (lastHighlighted != null) {
            lastHighlighted.GetComponent<HighlightedByMouse>().currentlyHightlightingMe = false;
            lastHighlighted = null;
        }
    }

    #endregion
}
