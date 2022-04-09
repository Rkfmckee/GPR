using UnityEngine;
using static GeneralHelper;

public class GoblinBehaviour : MonoBehaviour {
	#region Properties

	public float movementSpeed;

	private AnimatorController animatorController;

	#endregion

	#region Events

	private void Awake() {
		References.FriendlyCreature.goblins.Add(gameObject);
		animatorController = GetComponent<AnimatorController>();
	}

	private void Update() {
		
	}

	#endregion

	#region Methods


	#endregion
}
