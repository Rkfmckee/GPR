public class PlayerHighlightable : HighlightableObject {
    #region Properties

    private GoblinBehaviour playerBehaviour;

    #endregion

    #region Events

    protected override void Awake() {
        base.Awake();

        playerBehaviour = GetComponent<GoblinBehaviour>();
    }

    #endregion

    #region Methods

    public override bool DontSelect() {
        return base.DontSelect();
    }

    protected override void ObjectClicked() {
        if (playerBehaviour == null) {
            print(gameObject + "doesn't have a PlayerBehaviour");
            return;
        }
    }

    #endregion
}
