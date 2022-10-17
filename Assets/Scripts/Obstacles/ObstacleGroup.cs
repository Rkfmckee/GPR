using UnityEngine;

public class ObstacleGroup : MonoBehaviour
{
	#region Events

	private void Awake()
	{
		References.Obstacles.parentGroup = transform;
	}

	#endregion
}
