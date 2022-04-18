public class GoblinBehaviour : FriendlyBehaviour {

	#region Events

	protected override void Awake() {
		base.Awake();
		
		References.FriendlyCreature.goblins.Add(gameObject);

		SetCurrentState(new FriendlyStateIdle(gameObject));
	}

	private void Update() {
		GetCurrentState().Update();
	}

	#endregion

	#region Methods


	#endregion
}
