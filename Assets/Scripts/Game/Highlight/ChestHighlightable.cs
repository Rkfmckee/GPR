public class ChestHighlightable : HighlightableObject {
    #region Properties

    private ChestController chestController;

    #endregion

    #region Events

    protected override void Awake() {
        base.Awake();

        chestController = GetComponent<ChestController>();
    }

    #endregion

    #region Methods

    protected override bool DontSelect() {
        bool dontSelect = false;

        if (chestController.GetCurrentState() == ChestController.ChestState.Open) {
            dontSelect = true;
        }

        return dontSelect;
    }

    protected override void ObjectClicked() {
        if (chestController.GetCurrentState() == ChestController.ChestState.Closed) {
            chestController.Open();

            gameTraps.ShouldShowCaveInventory(true);
        }
    }

    #endregion
}