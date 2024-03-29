using UnityEngine.EventSystems;

public class StartRoundButtonHover : ButtonHover
{
	#region Fields

	private StartRoundButton button;

	#endregion

	#region Events

	protected override void Awake()
	{
		base.Awake();

		button = GetComponent<StartRoundButton>();
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		if (button.IsButtonPressed())
			return;

		base.OnPointerEnter(eventData);
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		if (button.IsButtonPressed())
			return;

		base.OnPointerExit(eventData);
	}

	#endregion
}
