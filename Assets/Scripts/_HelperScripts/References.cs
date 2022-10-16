using System.Collections.Generic;
using UnityEngine;

public static class References
{
	public static GameObject storageRoom;

	public static class Camera
	{
		public static UnityEngine.Camera camera;
		public static CameraController cameraController;
	}

	public static class FriendlyCreature
	{
		public static List<GameObject> goblins = new List<GameObject>();
	}

	public static class HostileCreature
	{
		public static List<GameObject> enemies = new List<GameObject>();
		public static GameObject spawnArea;
	}

	public static class Obstacles
	{
		public static Transform parentGroup;
		public static List<GameObject> trapsAndTriggers = new List<GameObject>();
		public static List<GameObject> traps = new List<GameObject>();
		public static List<GameObject> triggers = new List<GameObject>();
	}

	public static class Game
	{
		public static GameObject globalGameObject;
		public static GlobalObstaclesController globalObstacles;
		public static RoundStageController roundStage;
		public static ResourceController resources;
		public static CursorController cursor;
	}

	public static class UI
	{
		public static GameObject canvas;
		public static CanvasController canvasController;
		public static FriendlyListeningUIController friendlyListeningUIController;
		public static NotificationController notifications;
		public static CraftingMenu craftingMenu;
		public static ResourcesUI resources;
	}
}
