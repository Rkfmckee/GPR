using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGroup : MonoBehaviour {
	private void Awake() {
		References.Obstacles.parentGroup = transform;
	}
}
