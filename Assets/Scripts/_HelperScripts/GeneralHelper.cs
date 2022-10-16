using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class GeneralHelper
{
	public static Vector3 NormaliseVectorToKeepDeceleration(Vector3 vector)
	{
		// Normalizing a decimal vector rounds it to 1, which causes weird deceleration
		// So don't do that if it's between 1 and -1

		if (Mathf.Abs(vector.magnitude) > 1)
		{
			vector = vector.normalized;
		}

		return vector;
	}

	public static (Vector2, Vector2) GetFloorObjectBoundaries(bool getColliderEdge)
	{
		GameObject[] allFloorObjects = GameObject.FindGameObjectsWithTag("Floor");
		var xBounds = new Vector2();
		var zBounds = new Vector2();
		float minX, maxX, minZ, maxZ;
		float colliderOffsetX = 0, colliderOffsetZ = 0;

		minX = maxX = allFloorObjects[0].transform.position.x;
		minZ = maxZ = allFloorObjects[0].transform.position.z;

		foreach (var floor in allFloorObjects)
		{
			var position = floor.transform.position;

			if (getColliderEdge)
			{
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

	public static (Vector2, Vector2) GetLevelSize()
	{
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

	public static string GetDeterminer(string word)
	{
		string[] vowels = { "a", "e", "i", "o", "u" };
		string determiner = "a";

		if (word.ToLower().EndsWith("s"))
		{
			determiner = "some";
		}
		else
		{
			if (Array.IndexOf(vowels, word[0].ToString().ToLower()) > -1)
			{
				determiner = "an";
			}
		}

		return determiner;
	}

	public static Dictionary<string, int> GetLayerMasks()
	{
		var layerMasks = new Dictionary<string, int>();

		layerMasks.Add("FriendlyCreature", 1 << LayerMask.NameToLayer("FriendlyCreature"));
		layerMasks.Add("HostileCreature", 1 << LayerMask.NameToLayer("HostileCreature"));
		layerMasks.Add("Creature", layerMasks["FriendlyCreature"] | layerMasks["HostileCreature"]);
		layerMasks.Add("Obstacle", 1 << LayerMask.NameToLayer("Obstacle"));

		layerMasks.Add("Ignore Raycast", 1 << LayerMask.NameToLayer("Ignore Raycast"));
		layerMasks.Add("Highlightable", 1 << LayerMask.NameToLayer("Highlightable"));

		layerMasks.Add("WallHidden", 1 << LayerMask.NameToLayer("WallHidden"));
		layerMasks.Add("WallShouldHide", 1 << LayerMask.NameToLayer("WallShouldHide"));
		layerMasks.Add("Wall", 1 << LayerMask.NameToLayer("Wall") | layerMasks["WallShouldHide"]);
		layerMasks.Add("WallDecoration", 1 << LayerMask.NameToLayer("WallDecoration"));
		layerMasks.Add("WallWithDecoration", layerMasks["Wall"] | layerMasks["WallDecoration"]);

		layerMasks.Add("Floor", 1 << LayerMask.NameToLayer("Floor"));
		layerMasks.Add("Terrain", layerMasks["Floor"] | layerMasks["WallWithDecoration"]);
		layerMasks.Add("TerrainWithWallHidden", layerMasks["Terrain"] | layerMasks["WallHidden"]);

		// layerMasks = GeneralHelper.GetLayerMasks();
		return layerMasks;
	}

	public static string GetDescription(this Enum GenericEnum)
	{
		Type genericEnumType = GenericEnum.GetType();
		MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
		if ((memberInfo != null && memberInfo.Length > 0))
		{
			var _Attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
			if ((_Attribs != null && _Attribs.Count() > 0))
			{
				return ((System.ComponentModel.DescriptionAttribute)_Attribs.ElementAt(0)).Description;
			}
		}
		return GenericEnum.ToString();
	}
}
