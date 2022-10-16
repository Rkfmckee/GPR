using UnityEngine;

public class Sawblade : MonoBehaviour
{
	#region Fields

	private float rotationThreshold;

	#endregion

	#region Events

	private void Awake()
	{
		rotationThreshold = 0.5f;
	}

	private void OnCollisionStay(Collision other)
	{
		// Only take damage if we collide in the same direction the blade is facing
		var absoluteDot = Mathf.Abs(Vector3.Dot(other.contacts[0].normal, transform.parent.forward));

		if (absoluteDot > rotationThreshold)
		{
			var targetsHealthSystem = other.gameObject.GetComponent<HealthSystem>();
			if (targetsHealthSystem != null)
			{
				targetsHealthSystem.TakeDamageOverTime(2, 1, false);
			}
		}
	}

	#endregion
}
