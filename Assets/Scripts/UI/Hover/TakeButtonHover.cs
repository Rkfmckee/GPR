using UnityEngine.EventSystems;

public class TakeButtonHover : ButtonHover {
	#region Properties

	private TakeButton button;

	#endregion
	
	#region Events

	protected override void Awake() {
		base.Awake();

		button = GetComponent<TakeButton>();
	}

	public override void OnPointerEnter(PointerEventData eventData) {
		if (button.IsButtonPressed())
			return;

		base.OnPointerEnter(eventData);
	}

	public override void OnPointerExit(PointerEventData eventData) {
		if (button.IsButtonPressed())
			return;

		base.OnPointerExit(eventData);
	}

	#endregion
}
