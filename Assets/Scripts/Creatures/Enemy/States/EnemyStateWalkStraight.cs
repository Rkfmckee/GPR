using UnityEngine;

public class EnemyStateWalkStraight : EnemyState {

    #region Constructor

    public EnemyStateWalkStraight(GameObject gameObj) : base(gameObj) {
    }

    #endregion

    #region Events

    public override void Update() {
        ChaseTargetIfInFieldOfView();
    }

    public override void FixedUpdate() {
        movementDirection = WalkForward();

        Vector3 movementAmount = movementDirection * (movementSpeed * Time.deltaTime);
        Vector3 newPosition = transform.position + movementAmount;
        rigidbody.MovePosition(newPosition);
        transform.LookAt(newPosition);
    }

    public override void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "WallDecoration") {
            ChangeDirectionAfterHittingWall(collision);
        }
    }

    #endregion

    #region Methods

    private Vector3 WalkForward() {
        var directionToTarget = transform.forward;
        return directionToTarget;
    }

    public void ChangeDirectionAfterHittingWall(Collision collision) {
        var direction = collision.contacts[0].normal;

        direction = Quaternion.AngleAxis(Random.Range(-70.0f, 70.0f), Vector3.up) * direction;

        movementDirection = direction;
        transform.rotation = Quaternion.LookRotation(movementDirection);
    }

    #endregion
}
