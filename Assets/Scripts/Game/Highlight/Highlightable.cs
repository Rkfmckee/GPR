using System.Collections.Generic;
using UnityEngine;
using static CameraController;

public abstract class Highlightable : MonoBehaviour {
    #region Properties

    private bool hightlightingMe;
    protected Outline outline;
	protected List<ControllingState> highlightableStates;

    protected GameTrapsController gameTraps;
	protected List<GameObject> highlightTextObjects;
	private CanvasController canvasController;
	private CameraController cameraController;

    #endregion

    #region Events

    protected virtual void Awake() {
		highlightTextObjects = new List<GameObject>();
		highlightableStates = new List<ControllingState>();
		cameraController = Camera.main.GetComponent<CameraController>();

		outline = gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAndSilhouette;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 5f;
        outline.enabled = false;

        SetHighlightingMe(false);
    }

    protected virtual void Start() {
        gameTraps = References.GameController.gameTraps;
		canvasController = References.UI.Controllers.canvasController;
    }

    protected virtual void Update() {
        if (DontHighlight())
			SetHighlightingMe(false);

		if (!IsHighlightingMe()) {
			if (outline.enabled) {
                outline.enabled = false;
				canvasController.DisableHighlightText();
            }
			return;
		}

		if (!outline.enabled) {
			outline.enabled = true;
			canvasController.EnableHighlightText(highlightTextObjects);
		}

		if (gameTraps.IsTrapLinkingLineActive()) {
			return;
		}

		if (Input.GetButtonDown("Fire1")) {
			Clicked();
		}
    }

    #endregion

    #region Methods

		#region Get/Set

		public List<GameObject> GetHighlightTextObjects() {
			return highlightTextObjects;
		}

		public bool IsHighlightingMe() {
			return hightlightingMe;
		}

		public void SetHighlightingMe(bool highlighting) {
			hightlightingMe = highlighting;
		}

		#endregion

    protected abstract void Clicked();
	protected virtual bool DontHighlight() {
		return gameTraps.objectPlacement != null || !highlightableStates.Contains(cameraController.GetControllingState());
	}

    #endregion
}