using System.Collections.Generic;
using UnityEngine;
using static CameraController;
using static CursorData;

public abstract class Highlightable : MonoBehaviour {
    #region Properties

    private bool highlightingMe;
	private bool cameraInHighlightableState;
    protected Outline outline;
	protected Dictionary<ControllingState, List<GameObject>> statesAndUiText;
	protected CursorType? highlightCursor;

    protected GameTrapsController gameTraps;
	protected CameraController cameraController;
	private CanvasController canvasController;
	private CursorController cursor;

    #endregion

    #region Events

    protected virtual void Awake() {
		statesAndUiText = new Dictionary<ControllingState, List<GameObject>>();
		cameraController = Camera.main.GetComponent<CameraController>();

		outline = gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAndSilhouette;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 5f;
        outline.enabled = false;
		
		highlightCursor = null;

        SetHighlightingMe(false);
    }

    protected virtual void Start() {
        gameTraps = References.Game.gameTraps;
		canvasController = References.UI.Controllers.canvasController;
		cursor = References.Game.cursor;
    }

    protected virtual void Update() {
        if (DontHighlight())
			SetHighlightingMe(false);

		if (!IsHighlightingMe()) {
			if (outline.enabled) {
                outline.enabled = false;
				
				canvasController.DisableActionText();
            }
			return;
		}

		if (!outline.enabled) {
			outline.enabled = true;

			if (!canvasController.IsActionTextActive() && !OtherActionTextActive())
				canvasController.EnableActionText(statesAndUiText);
		}

		if (gameTraps.IsTrapLinkingLineActive()) {
			return;
		}

		if (Input.GetButtonDown("Fire1")) {
			LeftClicked();
		}

		if (Input.GetButtonDown("Fire2")) {
			RightClicked();
		}
    }

    #endregion

    #region Methods

		#region Get/Set

		public bool IsHighlightingMe() {
			return highlightingMe;
		}

		public void SetHighlightingMe(bool highlighting) {
			highlightingMe = highlighting;

			if (!highlightCursor.HasValue || !IsCameraInHighlightableState())
				return;

			if (highlighting)
				cursor.SetCursor(highlightCursor.Value);
			else
				cursor.SetCursor(CursorType.Basic);
		}

		public bool IsCameraInHighlightableState() {
			return cameraInHighlightableState;
		}

		#endregion

    protected abstract void LeftClicked();
	protected abstract void RightClicked();

	protected virtual bool DontHighlight() {
		cameraInHighlightableState = statesAndUiText.ContainsKey(cameraController.GetControllingState());
		
		return !cameraInHighlightableState;
	}

	private bool OtherActionTextActive() {
		return References.Game.gameTraps.IsLinkingTextActive();
	}

    #endregion
}