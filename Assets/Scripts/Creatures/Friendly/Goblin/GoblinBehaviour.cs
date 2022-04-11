public class GoblinBehaviour : FriendlyBehaviour {
	#region Properties

	

	#endregion

	#region Events

	private void Awake() {
		References.FriendlyCreature.goblins.Add(gameObject);
		animatorController = GetComponent<AnimatorController>();

		SetCurrentState(new FriendlyStateIdle(gameObject));
	}

	private void Update() {
		GetCurrentState().Update();
	}

	#endregion

	#region Methods


	#endregion
}
