public class ChestHighlightable : HighlightableObject {
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

    public override bool DontSelect() {
        bool dontSelect = false;

        if (chestController.GetCurrentState() == Chest.ChestState.Open) {
            dontSelect = true;
        }

        return dontSelect || base.DontSelect();
    }

    protected override void ObjectClicked() {
        if (chestController.GetCurrentState() == Chest.ChestState.Closed) {
            chestController.Open();

            gameTraps.ShouldShowCaveInventory(true);
        }
    }

    #endregion
}