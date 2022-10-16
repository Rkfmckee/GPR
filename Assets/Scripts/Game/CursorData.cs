using UnityEngine;
using static FriendlyStateListening;

public class CursorData
{
	#region Fields

	public CursorType cursorType;
	public Texture2D cursorTexture;
	public Texture2D[] cursorTextures;
	public Vector2 hotspot;

	#endregion

	#region Constructor

	public CursorData(Texture2D cursorTexture, Vector2 hotspot)
	{
		this.cursorTexture = cursorTexture;
		this.hotspot       = hotspot;
	}

	public CursorData(Texture2D[] cursorTextures, Vector2 hotspot)
	{
		this.cursorTextures = cursorTextures;
		this.hotspot        = hotspot;
	}

	#endregion

	#region Methods

	public static CursorType? ListeningCommandToCursorType(ListeningCommands command)
	{
		switch (command)
		{
			case ListeningCommands.Move:
				return CursorType.Move;

			case ListeningCommands.PickUp:
				return CursorType.Pickup;

			default:
				return null;
		}
	}

	#endregion

	#region Enums

	public enum CursorType
	{
		Basic,
		BasicGreen,
		BasicRed,
		Move,
		Pickup,
		Craft
	}

	#endregion
}