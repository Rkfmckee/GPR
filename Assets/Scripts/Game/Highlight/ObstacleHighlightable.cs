public class ObstacleHighlightable : HighlightableObject {
    #region Properties

    private PickUpObject pickupController;

    #endregion

    #region Events

    protected override void Awake() {
        base.Awake();

        pickupController = GetComponent<PickUpObject>();
    }

    #endregion

    #region Methods

    public override bool DontSelect() {
        bool dontSelect = pickupController.currentState == PickUpObject.State.HELD;

        return dontSelect || base.DontSelect();
    }

    protected override void ObjectClicked() {
        if (pickupController == null) {
            print(gameObject + "can't be picked up");
            return;
        }

        pickupController.SetCurrentState(PickUpObject.State.HELD);
    }

    #endregion
}