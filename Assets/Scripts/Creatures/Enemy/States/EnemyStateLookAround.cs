using UnityEngine;

public class EnemyStateLookAround : EnemyState 
{
    #region Properties

    private float secondsToLook;
    private float currentTime;
    //private Vector3 startingLookDirection;

    //// Values for figuring out how fast to turn
    //private float turnOnceAngle;
    //private float secondsToTurnOnce;

    //private float angleAmountToChange;
    //private float minAngle;
    //private float maxAngle;

    #endregion

    #region Methods

    #region Get/Set

    public float GetSecondsToLook() {
        return secondsToLook;
    }

    public void SetSecondsToLook(float seconds) {
        secondsToLook = seconds;
    }

    #endregion

    public EnemyStateLookAround(GameObject enemyObj, float secondsToLookAround) : base(enemyObj) {
        secondsToLook = secondsToLookAround;
        //startingLookDirection = enemyObj.transform.eulerAngles;
        //turnOnceAngle = 45;
        //secondsToTurnOnce = Random.Range(0, secondsToLook);

        //angleAmountToChange = turnOnceAngle * (Time.deltaTime / secondsToTurnOnce);
        //minAngle = startingLookDirection.y - turnOnceAngle;
        //maxAngle = startingLookDirection.y + turnOnceAngle;
    }


    public override void StateUpdate() {
        base.StateUpdate();
        
        currentTime += Time.deltaTime;

        //float currentXRotation = enemyObject.transform.eulerAngles.x;
        //float currentYRotation = enemyObject.transform.eulerAngles.y;
        //float currentZRotation = enemyObject.transform.eulerAngles.z;

        if (currentTime < secondsToLook) {
            //if ((currentYRotation > maxAngle && angleAmountToChange > 0) || (currentYRotation < minAngle && angleAmountToChange < 0)) {
            //    angleAmountToChange *= -1;
            //} else {
            //    enemyObject.transform.eulerAngles = new Vector3(currentXRotation, currentYRotation += angleAmountToChange, currentZRotation);
            //}
        } else {
            behaviour.SetCurrentState(new EnemyStateWander(enemyObject));
        }
    }

    public override void StateFixedUpdate() {
        return;
    }

    protected override Vector3? FindMovementTarget() {
        return null;
    }

    #endregion
}