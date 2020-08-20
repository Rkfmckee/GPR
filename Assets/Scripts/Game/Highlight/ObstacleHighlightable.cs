public class ObstacleHighlightable : HighlightableObject {
    #region Properties

    private PickUpController pickupController;

    #endregion

    #region Events

    protected override void Awake() {
        base.Awake();

        pickupController = GetComponent<PickUpController>();
    }

    #endregion

    #region Methods

    protected override bool DontSelect() {
        bool dontSelect = pickupController.currentState == PickUpController.State.Held;

        return dontSelect;
    }

    protected override void ObjectClicked() {
        if (pickupController == null) {
            print(gameObject + "can't be picked up");
            return;
        }

        pickupController.SetCurrentState(PickUpController.State.Held);
    }

    #endregion
}