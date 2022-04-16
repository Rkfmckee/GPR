using System.Collections.Generic;
using UnityEngine;
using static CameraController;

public abstract class Highlightable : MonoBehaviour {
    #region Properties

    private bool hightlightingMe;
    protected Outline outline;
	protected Dictionary<ControllingState, List<GameObject>> statesAndUiText;

    protected GameTrapsController gameTraps;
	private CanvasController canvasController;
	protected CameraController cameraController;

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
			return hightlightingMe;
		}

		public void SetHighlightingMe(bool highlighting) {
			hightlightingMe = highlighting;
		}

		#endregion

    protected abstract void LeftClicked();
	protected abstract void RightClicked();

	protected virtual bool DontHighlight() {
		return gameTraps.objectPlacement != null || !statesAndUiText.ContainsKey(cameraController.GetControllingState());
	}

	private bool OtherActionTextActive() {
		return References.GameController.gameTraps.IsLinkingTextActive();
	}

    #endregion
}