public class PlayerHighlightable : HighlightableObject {
    #region Properties

    private PlayerBehaviour playerBehaviour;

    #endregion

    #region Events

    protected override void Awake() {
        base.Awake();

        playerBehaviour = GetComponent<PlayerBehaviour>();
    }

    #endregion

    #region Methods

    public override bool DontSelect() {
        bool dontSelect = playerBehaviour.currentlyBeingControlled;

        return dontSelect || base.DontSelect();
    }

    protected override void ObjectClicked() {
        if (playerBehaviour == null) {
            print(gameObject + "doesn't have a PlayerBehaviour");
            return;
        }

        foreach (var player in References.Player.players) {
            player.GetComponent<PlayerBehaviour>().SetCurrentlyBeingControlled(false);
        }

        playerBehaviour.SetCurrentlyBeingControlled(true);
    }

    #endregion
}
