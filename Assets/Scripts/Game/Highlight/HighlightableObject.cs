using UnityEngine;

public abstract class HighlightableObject : MonoBehaviour {
    #region Properties

    [HideInInspector]
    public bool currentlyHightlightingMe;
    public float maxDistanceFromPlayer;

    protected Outline outline;
    protected GameTrapsController gameTraps;

    #endregion

    #region Events

    protected virtual void Awake() {
        outline = gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAndSilhouette;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 5f;
        outline.enabled = false;

        currentlyHightlightingMe = false;
    }

    protected virtual void Start() {
        gameTraps = References.GameController.gameTraps;
    }

    protected virtual void Update() {
        if (gameTraps.objectPlacement != null) return;
        if (DontSelect()) return;

        if (currentlyHightlightingMe) {
            if (!outline.enabled) {
                outline.enabled = true;
            }

            if (gameTraps.IsTrapLinkingLineActive()) {
                return;
            }

            if (Input.GetButtonDown("Fire1")) {
                ObjectClicked();
            }

        } else {
            if (outline.enabled) {
                outline.enabled = false;
            }
        }
    }

    #endregion

    #region Methods

    public virtual bool DontSelect() {
        bool dontSelect = false;

        return dontSelect;
    }

    protected abstract void ObjectClicked();

    #endregion
}