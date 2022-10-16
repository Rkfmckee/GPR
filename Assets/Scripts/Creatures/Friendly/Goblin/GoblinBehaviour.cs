public class GoblinBehaviour : FriendlyBehaviour
{
	#region Events

	protected override void Awake()
	{
		base.Awake();

		References.FriendlyCreature.goblins.Add(gameObject);

		CurrentState = new FriendlyStateIdle(gameObject);
	}

	private void Update()
	{
		CurrentState.Update();
	}

	#endregion
}
