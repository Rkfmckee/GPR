using UnityEngine;

public class CeilingController : MonoBehaviour
{
	#region Fields

	private new BoxCollider collider;

	#endregion

	#region Events

	private void Awake()
	{
		collider = GetComponent<BoxCollider>();

		var (levelSize, levelMidpoint) = GeneralHelper.GetLevelSize();
		collider.size              	   = new Vector3(levelSize.x, 2, levelSize.y);
		transform.position         	   = new Vector3(levelMidpoint.x, 7, levelMidpoint.y);
	}

	#endregion
}
