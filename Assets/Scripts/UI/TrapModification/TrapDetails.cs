using TMPro;
using UnityEngine;

public class TrapDetails : MonoBehaviour {
	#region Properties

	private TrapTriggerBase trap;
	
	private TextMeshProUGUI trapName;

	#endregion

	#region Events

	private void Awake() {
		var background = transform.Find("TrapDetailsBackground");
		trapName = background.Find("TrapName").GetComponent<TextMeshProUGUI>();
	}

	#endregion

	#region Methods

	public void SetTrap(TrapTriggerBase trap) {
		this.trap = trap;

		print($"trapNameComp {trapName}");
		print($"trap {trap}");
		trapName.text = trap.GetName();
	}

	#endregion
}
