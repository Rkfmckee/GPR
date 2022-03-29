﻿using UnityEngine;

public class LookForHighlightableObjects : MonoBehaviour {
    #region Properties

    private new Camera camera;
    private int highlightLayerMask;
    private GameObject lastHighlighted;
    private float dontSelectTimer;
    private float dontSelectTime;

    private GameTrapsController gameController;

    #endregion

    #region Events

    private void Awake() {
        camera = Camera.main;
        gameController = GetComponent<GameTrapsController>();

        int highlightableObjectLayerMask = 1 << LayerMask.NameToLayer("Highlightable");
        int obstacleLayerMask = 1 << LayerMask.NameToLayer("Obstacle");

        highlightLayerMask = highlightableObjectLayerMask | obstacleLayerMask;

        dontSelectTime = 1;
    }

    private void Update() {
        if (dontSelectTimer < dontSelectTime) {
            dontSelectTimer += Time.deltaTime;
        } else {
            if (!gameController.IsInventoryOpen()) {
                SendOutRaycast();
            } else {
                ClearLastHighlighted();
            }
        }
    }

    #endregion

    #region Methods

    public void ResetDontSelectTimer() {
        dontSelectTimer = 0;
    }

    private void SendOutRaycast() {
        RaycastHit hit;
        Ray cameraToMouse = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(cameraToMouse, out hit, Mathf.Infinity, highlightLayerMask)) {
            GameObject currentHit = hit.transform.gameObject;
            HighlightableObject highlightScript = currentHit.GetComponent<HighlightableObject>();

            if (highlightScript != null && References.Player.currentPlayer != null) {
                if (Vector3.Distance(currentHit.transform.position, References.Player.currentPlayer.transform.position) < highlightScript.maxDistanceFromPlayer) {
                    if (currentHit != lastHighlighted) {
                        ClearLastHighlighted();

                        highlightScript.currentlyHightlightingMe = true;
                        lastHighlighted = currentHit;

                        if (!highlightScript.DontSelect()) {
                            if (!gameController.IsHighlightTextActive() && !gameController.IsLinkingTextActive()) {
                                bool modifyText = currentHit.tag == "Trap" || currentHit.tag == "Trigger";

                                gameController.EnableHighlightItemText(true, modifyText);
                            }
                        }
                    }
                } else {
                    ClearLastHighlighted();
                }
            }
        } else {
            ClearLastHighlighted();
        }
    }

    private void ClearLastHighlighted() {
        if (lastHighlighted != null) {
            lastHighlighted.GetComponent<HighlightableObject>().currentlyHightlightingMe = false;
            lastHighlighted = null;
        }

        if (gameController.IsHighlightTextActive()) {
            gameController.EnableHighlightItemText(false, false);
        }
    }

    #endregion
}
