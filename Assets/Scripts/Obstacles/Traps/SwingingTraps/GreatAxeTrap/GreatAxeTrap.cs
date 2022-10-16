public class GreatAxeTrap : SwingingTrap
{
	#region Events

	protected override void Awake()
	{
		swingingChild = transform.Find("GreatAxe");

		base.Awake();
	}

	#endregion
}
