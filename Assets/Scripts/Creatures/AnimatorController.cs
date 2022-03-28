using UnityEngine;

public class AnimatorController : MonoBehaviour
{
	#region Properties

	private int vertical;
	private int horizontal;

	private Animator animator;

	#endregion
	
	#region Events

	private void Awake() {
		animator = GetComponentInChildren<Animator>();
		vertical = Animator.StringToHash("Vertical");
		horizontal = Animator.StringToHash("Horizontal");
	}

	#endregion

	#region Methods

	public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement) {
			float v = 0;

			if (verticalMovement > 0 && verticalMovement < 0.55f) {
				v = 0.5f;
			} else if (verticalMovement > 0.55f) {
				v = 1;
			} else if (verticalMovement < 0 && verticalMovement > -0.55f) {
				v = -0.5f;
			} else if (verticalMovement < -0.55f) {
				v = -1;
			} else {
				v = 0;
			}

			float h = 0;

			if (horizontalMovement > 0 && horizontalMovement < 0.55f) {
				h = 0.5f;
			} else if (horizontalMovement > 0.55f) {
				h = 1;
			} else if (horizontalMovement < 0 && horizontalMovement > -0.55f) {
				h = -0.5f;
			} else if (horizontalMovement < -0.55f) {
				h = -1;
			} else {
				h = 0;
			}

			animator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
			animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
		}

	#endregion
    
}
