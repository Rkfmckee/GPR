using UnityEngine;

public class ArrowLauncher : LaunchingTrap {

    #region Events

    protected override void Awake() {
		base.Awake();
		
        ammoPrefab = Resources.Load("Prefabs/Obstacles/Traps/LaunchingTraps/ArrowLauncher/Arrow") as GameObject;
    }

    #endregion
}
