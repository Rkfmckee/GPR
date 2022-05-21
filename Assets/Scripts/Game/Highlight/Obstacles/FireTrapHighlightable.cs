using UnityEngine;

public class FireTrapHighlightable : TrapHighlightable {
	#region Properties

	private GameObject fire;

	#endregion

	protected override void Awake() {
		base.Awake();

		fire = transform.Find("Fire").gameObject;
	}
	
	#region Methods

	protected override bool DontHighlight() {
		var dontHighlight = fire.activeSelf;
		
		return dontHighlight || base.DontHighlight();
	}

	#endregion
}
