using UnityEngine;

public class WallController : MonoBehaviour {
	#region Events

	private void Awake() {
		Vector3 positionToCheckForFloor = transform.position + new Vector3(0, 0, 4);
		int floorMask = 1 << LayerMask.NameToLayer("Floor");
		
		Collider[] hitColliders = Physics.OverlapSphere(positionToCheckForFloor, 1, floorMask);
		if (hitColliders.Length > 0) {
			gameObject.layer = LayerMask.NameToLayer("WallShouldHide");
		}
	}

	#endregion
}
