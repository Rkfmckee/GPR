using UnityEngine;

public class SpikeTrapHighlightable : TrapHighlightable {
	#region Properties

	private SpikeTrap spikeTrap;

	#endregion
	
	#region Events

	protected override void Awake() {
        base.Awake();

		spikeTrap = GetComponent<SpikeTrap>();
	}

	#endregion

	#region Methods

	protected override bool DontHighlight() {
		var dontHighlight = spikeTrap.currentState != SpikeTrap.SpikeState.SpikesDown;
		
		return dontHighlight || base.DontHighlight();
	}

	#endregion
}
