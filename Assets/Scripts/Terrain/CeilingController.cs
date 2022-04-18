using UnityEngine;

public class CeilingController : MonoBehaviour {
	#region Properties

	private new BoxCollider collider;

	#endregion
	
	#region Events

	private void Awake() {
		collider = GetComponent<BoxCollider>();
		Vector2 levelSize, levelMidpoint;

		(levelSize, levelMidpoint) = GeneralHelper.GetLevelSize();
		collider.size = new Vector3(levelSize.x, 2, levelSize.y);
		transform.position = new Vector3(levelMidpoint.x, 7, levelMidpoint.y);
	}

	#endregion
}
