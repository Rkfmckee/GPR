using UnityEngine;
using static CameraController;

public class FriendlyStatePickupObject : FriendlyState {
	#region Properties
	
	private bool isHoldingObject;
	private float interactionDistance;

	private GameObject obstacleToPickup;
	private GameObject heldObject;
	private ObstacleController heldObjectObstacleController;
	private PickUpObject heldObjectPickup;
	private GameObject obstaclePlacement;
	private ObstaclePlacementController obstaclePlacementController;

	#endregion

	#region Constructor

	public FriendlyStatePickupObject(GameObject gameObj, GameObject obsToPickup) : base(gameObj){
		References.Camera.cameraController.SetControllingState(ControllingState.ControllingObstaclePlacement);
		
		obstacleToPickup = obsToPickup;
		isHoldingObject = false;
		interactionDistance = 3;

		EnableObstaclePlacement(obstacleToPickup);
	}

	#endregion

	#region Events

	public override void Update() {
		base.Update();

		if (!isHoldingObject) {
			navMeshAgent.SetDestination(obstacleToPickup.transform.position);

			if (IsWithinInteractionDistance(transform.position, obstacleToPickup.transform.position)) {
				navMeshAgent.isStopped = true;
				SetHeldObject();
			}
		} else {
			if (obstaclePlacement == null || obstaclePlacementController == null)
				return;

			if (obstaclePlacementController.IsPositionFinalized()) {
				var positionToPlace = obstaclePlacement.transform.position;
				var rotationToPlace = obstaclePlacement.transform.rotation;
				navMeshAgent.isStopped = false;
				navMeshAgent.SetDestination(positionToPlace);

				if (IsWithinInteractionDistance(transform.position, positionToPlace)) {
					PlaceHeldObject(positionToPlace, rotationToPlace);
				}
			}
		}
	}



	#endregion

	#region Methods

	private void EnableObstaclePlacement(GameObject obstacle) {
		obstaclePlacement = References.Game.globalObstacles.EnableObstaclePlacement(obstacle);
		obstaclePlacementController = obstaclePlacement.GetComponent<ObstaclePlacementController>();

		obstaclePlacementController.SetHeldObject(obstacle);
	}

	private bool IsWithinInteractionDistance(Vector3 position, Vector3 targetPosition) {
		targetPosition.y = 0;
		return Vector3.Distance(position, targetPosition) < interactionDistance;
	}

	private void SetHeldObject() {
		heldObject = obstacleToPickup;
		heldObjectObstacleController = heldObject.GetComponent<ObstacleController>();
        heldObjectPickup = heldObject.GetComponent<PickUpObject>();

		isHoldingObject = true;
		heldObjectPickup.SetCurrentState(PickUpObject.PickUpState.Held, transform);
		obstacleToPickup = null;
	}

	private void PlaceHeldObject(Vector3 position, Quaternion rotation) {
		heldObjectObstacleController.SetObstacleDisabled(false);
		heldObjectPickup.SetCurrentState(PickUpObject.PickUpState.Idle);
		heldObject.transform.position = position;
		heldObject.transform.rotation = rotation;

		ForgetHeldObject();

		behaviour.SetCurrentState(new FriendlyStateIdle(gameObject));
	}

	private void ForgetHeldObject() {
        if (heldObject != null) {
            heldObject = null;

            References.Game.globalObstacles.DisableObstaclePlacement(obstaclePlacement);
        }
    }

	#endregion
}