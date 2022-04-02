using UnityEngine;

public class Arrow : MonoBehaviour
{
    #region Properties

    private float speed;
    private new Rigidbody rigidbody;
    private Collider[] collidersToIgnore;
	private GameObject[] arrowPieces;

    #endregion

    #region Events

    private void Awake() {
        rigidbody = gameObject.GetComponent<Rigidbody>();
		arrowPieces = new GameObject[] {
			Resources.Load("Prefabs/Objects/Traps/ArrowTrap/ArrowPiece-Body") as GameObject,
			Resources.Load("Prefabs/Objects/Traps/ArrowTrap/ArrowPiece-Feather") as GameObject,
			Resources.Load("Prefabs/Objects/Traps/ArrowTrap/ArrowPiece-Tip") as GameObject
		};
    }

    private void Update() {
        transform.rotation = Quaternion.LookRotation(rigidbody.velocity);
    }

    private void OnCollisionEnter(Collision collision) {
        foreach(GameObject piece in arrowPieces)  {
			var currentPiece = Instantiate(piece, transform.position, transform.GetChild(0).rotation);
			currentPiece.transform.parent = transform.parent;
			currentPiece.transform.localScale = transform.localScale;
			currentPiece.GetComponent<ArrowPiece>().shouldShrink = true;
		}
		
		Destroy(gameObject);
    }

    #endregion

    #region Methods

    public void SetCollidersToIgnore(Collider[] colliders) {
        collidersToIgnore = colliders;

        foreach(Collider collider in collidersToIgnore) {
            Physics.IgnoreCollision(GetComponent<Collider>(), collider);
        }
    }

    #endregion
}
