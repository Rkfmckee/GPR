using UnityEngine;

public class LaunchingAmmo : MonoBehaviour
{
	#region Fields

	protected float damage;
	protected GameObject[] ammoPieces;

	private new Rigidbody rigidbody;
	private Collider[] collidersToIgnore;

	#endregion

	#region Events

	protected virtual void Awake()
	{
		rigidbody = gameObject.GetComponent<Rigidbody>();
	}

	private void Update()
	{
		transform.rotation = Quaternion.LookRotation(rigidbody.velocity);
	}

	private void OnCollisionEnter(Collision other)
	{
		var amountToChangePosition = transform.forward;
		var positionToCreate       = transform.position - amountToChangePosition;

		foreach (GameObject piece in ammoPieces)
		{
			var currentPiece       = Instantiate(piece, positionToCreate, transform.GetChild(0).rotation);
			var launchingAmmoPiece = currentPiece.GetComponent<LaunchingAmmoPiece>();

			currentPiece.transform.parent     = transform.parent;
			currentPiece.transform.localScale = transform.localScale;
			launchingAmmoPiece.ShouldShrink   = true;

			positionToCreate += amountToChangePosition;
		}

		var targetsHealthSystem = other.gameObject.GetComponent<HealthSystem>();
		if (targetsHealthSystem != null)
		{
			targetsHealthSystem.TakeDamageOverTime(damage, 0.5f, false);
		}

		Destroy(gameObject);
	}

	#endregion

	#region Methods

	public void SetCollidersToIgnore(Collider[] colliders)
	{
		collidersToIgnore = colliders;

		foreach (Collider collider in collidersToIgnore)
		{
			Physics.IgnoreCollision(GetComponent<Collider>(), collider);
		}
	}

	#endregion
}
