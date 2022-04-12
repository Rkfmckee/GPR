using System.Collections.Generic;
using UnityEngine;

public abstract class Highlightable : MonoBehaviour {
    #region Properties

    private bool hightlightingMe;
    protected Outline outline;
    protected GameTrapsController gameTraps;
	protected List<GameObject> highlightTextObjects;

    #endregion

    #region Events

    protected virtual void Awake() {
		highlightTextObjects = new List<GameObject>();

		outline = gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAndSilhouette;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 5f;
        outline.enabled = false;

        SetHighlightingMe(false);
    }

    protected virtual void Start() {
        gameTraps = References.GameController.gameTraps;
    }

    protected virtual void Update() {
        if (gameTraps.objectPlacement != null) return;
        if (DontSelect()) return;

        if (IsHighlightingMe()) {
            if (!outline.enabled) {
                outline.enabled = true;
            }

            if (gameTraps.IsTrapLinkingLineActive()) {
                return;
            }

            if (Input.GetButtonDown("Fire1")) {
                Clicked();
            }

        } else {
            if (outline.enabled) {
                outline.enabled = false;
            }
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

    public virtual bool DontSelect() {
        bool dontSelect = false;

        return dontSelect;
    }

    protected abstract void Clicked();

    #endregion
}