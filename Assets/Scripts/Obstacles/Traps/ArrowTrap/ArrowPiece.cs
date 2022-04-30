using UnityEngine;

public class ArrowPiece : MonoBehaviour
{
	[HideInInspector]
    public bool shouldShrink;
	
	private float shrinkTarget;
	private float shrinkRate;

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
}
