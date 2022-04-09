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

	private void FixedUpdate() {
		GetCurrentState().FixedUpdate();
	}

	#endregion

	#region Methods


	#endregion
}
