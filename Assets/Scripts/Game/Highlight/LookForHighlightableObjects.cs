using UnityEngine;

public class LookForHighlightableObjects : MonoBehaviour {
    #region Properties

    private GameObject lastHighlighted;

	private new Camera camera;
    private CanvasController canvasController;
	private GameTrapsController gameTrapsController;

    #endregion

    #region Events

    private void Awake() {
        camera = Camera.main;
    }

	private void Start() {
		canvasController = References.UI.Controllers.canvasController;
		gameTrapsController = References.GameController.gameTraps;
	}

    private void Update() {
		if (gameTrapsController.IsInventoryOpen()) {
			ClearLastHighlighted();
			return;
		}

		LookForObjects();
    }

    #endregion

    #region Methods

    private void LookForObjects() {
        RaycastHit hit;
        Ray cameraToMouse = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(cameraToMouse, out hit, Mathf.Infinity)) {
            GameObject currentHit = hit.transform.gameObject;
            Highlightable highlightScript = currentHit.GetComponent<Highlightable>();
			
			if (highlightScript == null) {
				ClearLastHighlighted();
				return;
			}

			if (currentHit != lastHighlighted) {
				ClearLastHighlighted();

				highlightScript.currentlyHightlightingMe = true;
				lastHighlighted = currentHit;

				canvasController.EnableHighlightText(highlightScript.GetHighlightTextObjects());
			}
        }
    }

    private void ClearLastHighlighted() {
        if (lastHighlighted != null) {
            lastHighlighted.GetComponent<Highlightable>().currentlyHightlightingMe = false;
            lastHighlighted = null;
        }

        if (canvasController.IsHighlightTextActive()) {
            canvasController.DisableHighlightText();
        }
    }

    #endregion
}
