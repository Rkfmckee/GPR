using UnityEngine;

public static class GeneralHelper
{
    public static Vector3 NormaliseVectorToKeepDeceleration(Vector3 vector) {
		// Normalizing a decimal vector rounds it to 1, which causes weird deceleration
		// So don't do that if it's between 1 and -1

		if (Mathf.Abs(vector.magnitude) > 1) {
			vector = vector.normalized;
		}

		return vector;
	}
}
