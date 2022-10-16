using UnityEngine;

public class EnemyStateChase : EnemyState
{
	public EnemyStateChase(GameObject gameObj) : base(gameObj)
	{
	}

	public override void Update()
	{
		if (fieldOfView.VisibleTargets.Count <= 0)
		{
			behaviour.CurrentState = new EnemyStateWander(gameObject);
		}
	}

	public override void FixedUpdate()
	{
		MoveTowardsTarget();
	}

	public override void OnCollisionEnter(Collision collision)
	{
	}

	private void MoveTowardsTarget()
	{
		movementDirection = DirectionToMovementTarget();

		var movementAmount = movementDirection * (movementSpeed * Time.deltaTime);
		var newPosition    = transform.position + movementAmount;
		rigidbody.MovePosition(newPosition);
		transform.LookAt(newPosition);
	}

	private Vector3? FindMovementTarget()
	{
		var playersInEyesight = fieldOfView.VisibleTargets;

		if (playersInEyesight.Count <= 0) { return null; }

		var nearestPlayer           = playersInEyesight[0];
		var distanceToNearestPlayer = Vector3.Distance(transform.position, nearestPlayer.transform.position);

		for (int i = 1; i < playersInEyesight.Count; i++)
		{
			var currentPlayer           = playersInEyesight[i];
			var distanceToCurrentPlayer = Vector3.Distance(transform.position, currentPlayer.transform.position);

			if (distanceToCurrentPlayer < distanceToNearestPlayer)
			{
				nearestPlayer           = currentPlayer;
				distanceToNearestPlayer = distanceToCurrentPlayer;
			}
		}

		return nearestPlayer.transform.position;
	}

	private Vector3 DirectionToMovementTarget()
	{
		Vector3? movementTarget = FindMovementTarget();

		if (movementTarget.HasValue)
		{
			var xDirectionToTarget = movementTarget.Value.x - transform.position.x;
			var zDirectionToTarget = movementTarget.Value.z - transform.position.z;

			var directionToTarget = new Vector3(xDirectionToTarget, 0, zDirectionToTarget);
			directionToTarget 	  = directionToTarget.normalized;

			return directionToTarget;
		}
		else
		{
			return Vector3.zero;
		}
	}
}
