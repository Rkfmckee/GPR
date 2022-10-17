using UnityEngine;

public class FloorController : MonoBehaviour
{
	#region Events

	private void Awake()
	{
		var positionToCheckForWall = transform.position + new Vector3(0, 0, -4);
		var wallMask               = 1 << LayerMask.NameToLayer("Wall");
		var hitColliders           = Physics.OverlapSphere(positionToCheckForWall, 1, wallMask);

		foreach (var collider in hitColliders)
		{
			collider.gameObject.layer = LayerMask.NameToLayer("WallShouldHide");
		}
	}

	#endregion
}
