using UnityEngine;

public class LookForHighlightableObjects : MonoBehaviour {
    #region Properties

    private GameObject lastHighlighted;
	private int layerMask;

	private new Camera camera;
    private CanvasController canvasController;
	private GameTrapsController gameTrapsController;

    #endregion

    #region Events

    private void Awake() {
        camera = Camera.main;
		var wallHidden = 1 << LayerMask.NameToLayer("WallHidden");
		var ignoreRaycast = 1 << LayerMask.NameToLayer("Ignore Raycast");
		layerMask = ~(wallHidden | ignoreRaycast);
    }

	private void Start() {
		canvasController = References.UI.Controllers.canvasController;
		gameTrapsController = References.Game.gameTraps;
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

        if (Physics.Raycast(cameraToMouse, out hit, Mathf.Infinity, layerMask)) {
            GameObject currentHit = hit.transform.gameObject;
            Highlightable highlightScript = currentHit.GetComponent<Highlightable>();
			
			if (highlightScript == null) {
				ClearLastHighlighted();
				return;
			}

			if (currentHit != lastHighlighted) {
				ClearLastHighlighted();

				highlightScript.SetHighlightingMe(true);
				lastHighlighted = currentHit;
			}
        }
    }

    private void ClearLastHighlighted() {
        if (lastHighlighted != null) {
            lastHighlighted.GetComponent<Highlightable>().SetHighlightingMe(false);
            lastHighlighted = null;
        }
    }

    #endregion
}
