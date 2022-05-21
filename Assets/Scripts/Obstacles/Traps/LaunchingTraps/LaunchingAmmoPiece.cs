using UnityEngine;

public class LaunchingAmmoPiece : MonoBehaviour
{
	#region Properties

    private bool shouldShrink;
	private float shrinkTarget;
	private float shrinkRate;

	#endregion

	#region Events

	private void Awake() {
		shrinkTarget = 0.2f;
		shrinkRate = 5;
	}

	private void Update() {
		if (shouldShrink) {
			transform.localScale -= Vector3.one * shrinkRate * Time.deltaTime;

			if (transform.localScale.magnitude <= shrinkTarget) {
				Destroy(gameObject);
			}
		}
	}

	#endregion

	#region Methods

		#region Get/Set

		public void SetShouldShrink(bool shrink) {
			shouldShrink = shrink;
		}

		#endregion

	#endregion
}
