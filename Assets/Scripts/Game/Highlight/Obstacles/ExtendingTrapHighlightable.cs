using static ExtendingTrap;

public class ExtendingTrapHighlightable : TrapHighlightable
{
	#region Fields

	private ExtendingTrap extendingTrap;

	#endregion

	#region Events

	protected override void Awake()
	{
		base.Awake();

		extendingTrap = GetComponent<ExtendingTrap>();
	}

	#endregion

	#region Methods

	protected override bool DontHighlight()
	{
		var dontHighlight = extendingTrap.GetCurrentState() != ExtendedState.Retracted;

		return dontHighlight || base.DontHighlight();
	}

	#endregion
}
