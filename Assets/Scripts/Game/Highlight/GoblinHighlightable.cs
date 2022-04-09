public class GoblinHighlightable : Highlightable {
    #region Properties

    private GoblinBehaviour goblinBehaviour;

    #endregion

    #region Events

    protected override void Awake() {
        base.Awake();

        goblinBehaviour = GetComponent<GoblinBehaviour>();
    }

	protected override void Update() {
		base.Update();
	}

    #endregion

    #region Methods

    protected override void Clicked() {
        if (goblinBehaviour == null) {
            print($"{gameObject} doesn't have a GoblinBehaviour");
            return;
        }

		goblinBehaviour.SetCurrentlyControlled(true);
    }

    #endregion
}
