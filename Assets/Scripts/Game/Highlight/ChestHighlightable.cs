public class ChestHighlightable : Highlightable {
    #region Properties

    private Chest chestController;

    #endregion

    #region Events

    protected override void Awake() {
        base.Awake();

        chestController = GetComponent<Chest>();
    }

    #endregion

    #region Methods

    protected override void LeftClicked() {
        if (chestController.GetCurrentState() == Chest.ChestState.Closed) {
            chestController.Open();

            gameTraps.ShouldShowCaveInventory(true);
        }
    }

	protected override void RightClicked() {
	}

	#endregion
}