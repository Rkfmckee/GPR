﻿using UnityEngine;

public class LookForHighlightableObjects : MonoBehaviour {
    #region Properties

    private new Camera camera;
    private GameObject lastHighlighted;
    private float dontSelectTimer;
    private float dontSelectTime;

    private GameTrapsController gameController;

    #endregion

    #region Events

    private void Awake() {
        camera = Camera.main;
        gameController = GetComponent<GameTrapsController>();

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

        if (Physics.Raycast(cameraToMouse, out hit, Mathf.Infinity)) {
            GameObject currentHit = hit.transform.gameObject;
            Highlightable highlightScript = currentHit.GetComponent<Highlightable>();
			
            if (highlightScript != null) {
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
    }

    private void ClearLastHighlighted() {
        if (lastHighlighted != null) {
            lastHighlighted.GetComponent<Highlightable>().currentlyHightlightingMe = false;
            lastHighlighted = null;
        }

        if (gameController.IsHighlightTextActive()) {
            gameController.EnableHighlightItemText(false, false);
        }
    }

    #endregion
}
