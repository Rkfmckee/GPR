using System;
using UnityEngine;

public static class GeneralHelper
{
    public static Vector3 NormaliseVectorToKeepDeceleration(Vector3 vector) {
		// Normalizing a decimal vector rounds it to 1, which causes weird deceleration
		// So don't do that if it's between 1 and -1

		if (Mathf.Abs(vector.magnitude) > 1) {
			vector = vector.normalized;
		}

		return vector;
	}

	public static (Vector2, Vector2) GetFloorObjectBoundaries(bool getColliderEdge) {
		GameObject[] allFloorObjects = GameObject.FindGameObjectsWithTag("Floor");
		var xBounds = new Vector2();
		var zBounds = new Vector2();
		float minX, maxX, minZ, maxZ;
		float colliderOffsetX = 0, colliderOffsetZ = 0;
		
		minX = maxX = allFloorObjects[0].transform.position.x;
		minZ = maxZ = allFloorObjects[0].transform.position.z;

		foreach(var floor in allFloorObjects) {
			var position = floor.transform.position;
			
			if (getColliderEdge) {
				var colliderSize = floor.GetComponent<Collider>().bounds.size;
				colliderOffsetX = colliderSize.x / 2;
				colliderOffsetZ = colliderSize.z / 2;
			}

			if (position.x < minX)
				minX = position.x;
			
			if (position.x > maxX)
				maxX = position.x;

			if (position.z < minZ)
				minZ = position.z;
			
			if (position.z > maxZ)
				maxZ = position.z;
		}

		xBounds.x = minX - colliderOffsetX;
		xBounds.y = maxX + colliderOffsetX;
		zBounds.x = minZ - colliderOffsetZ;
		zBounds.y = maxZ + colliderOffsetZ;

		return (xBounds, zBounds);
	}

	public static (Vector2, Vector2) GetLevelSize() {
		var xBounds = new Vector2();
		var zBounds = new Vector2();
		(xBounds, zBounds) = GetFloorObjectBoundaries(true);

		var sizeX = Mathf.Round(Mathf.Abs(xBounds.y) + Mathf.Abs(xBounds.x));
		var sizeZ = Mathf.Round(Mathf.Abs(zBounds.y) + Mathf.Abs(zBounds.x));
		var midX = Mathf.Round(Mathf.Abs(xBounds.y) - Mathf.Abs(xBounds.x)) / 2;
		var midZ = Mathf.Round(Mathf.Abs(zBounds.y) - Mathf.Abs(zBounds.x)) / 2;

		var size = new Vector2(sizeX, sizeZ);
		var midPoint = new Vector2(midX, midZ);

		return (size, midPoint);
	}

	public static string GetDeterminer(string word) {
		string[] vowels = { "a", "e", "i", "o", "u" };
		string determiner = "a";

		if (word.ToLower().EndsWith("s")) {
			determiner = "some";
		} else {
			if (Array.IndexOf(vowels, word[0]) > -1) {
				determiner = "an";
			}
		}

		return determiner;
	}
}
