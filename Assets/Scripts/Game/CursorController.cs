using System.Collections.Generic;
using UnityEngine;
using static CursorData;

public class CursorController : MonoBehaviour
{
	#region Fields

	[SerializeField]
	private float cursorChangeTime;

	private CursorData currentCursor;
	private Dictionary<CursorType, CursorData> cursors;
	private float cursorChangeTimer;
	private int cursorTextureIndex;

	#endregion

	#region Properties

	public CursorData CurrentCursor { get => currentCursor; }

	#endregion

	#region Events

	private void Awake()
	{
		References.Game.cursor = this;

		cursorChangeTimer  = 0;
		cursorTextureIndex = 0;

		string cursorPath = "Images/Cursors";

		cursors = new Dictionary<CursorType, CursorData>();
		cursors.Add(CursorType.Basic, new CursorData(Resources.Load<Texture2D>($"{cursorPath}/Basic"), new Vector2(82, 22)));
		cursors.Add(CursorType.BasicGreen, new CursorData(Resources.Load<Texture2D>($"{cursorPath}/BasicGreen"), new Vector2(82, 22)));
		cursors.Add(CursorType.BasicRed, new CursorData(Resources.Load<Texture2D>($"{cursorPath}/BasicRed"), new Vector2(82, 22)));
		cursors.Add(CursorType.Pickup, new CursorData(Resources.Load<Texture2D>($"{cursorPath}/Pickup"), new Vector2(80, 30)));
		cursors.Add(CursorType.Craft, new CursorData(Resources.Load<Texture2D>($"{cursorPath}/Craft"), new Vector2(30, 25)));

		cursors.Add(CursorType.Move, new CursorData(new Texture2D[] {
			Resources.Load<Texture2D>($"{cursorPath}/MoveBig"),
			Resources.Load<Texture2D>($"{cursorPath}/MoveSmall")
		}, new Vector2(128, 131)));

		SetCursor(CursorType.Basic);
	}

	private void Update()
	{
		// Only applicable to cursors with multiple textures
		if (currentCursor.cursorTextures == null)
		{
			return;
		}

		if (cursorChangeTimer < cursorChangeTime)
		{
			cursorChangeTimer += Time.deltaTime;
			return;
		}

		cursorChangeTimer  = 0;
		cursorTextureIndex = (cursorTextureIndex + 1) % currentCursor.cursorTextures.Length;
		SetCursor(currentCursor.cursorType, cursorTextureIndex);
	}

	#endregion

	#region Methods

	public void SetCursor(CursorType cursorType)
	{
		// Store the current cursor information
		cursors[cursorType].cursorType = cursorType;
		currentCursor = cursors[cursorType];

		// If there are multiple cursor textures, set the cursor to the first one
		if (cursors[cursorType].cursorTextures != null)
		{
			Cursor.SetCursor(cursors[cursorType].cursorTextures[0], cursors[cursorType].hotspot, CursorMode.Auto);
			return;
		}

		// Otherwise, just set it as expected
		Cursor.SetCursor(cursors[cursorType].cursorTexture, cursors[cursorType].hotspot, CursorMode.Auto);
	}

	public void SetCursor(CursorType cursorType, int index)
	{
		if (cursors[cursorType].cursorTextures == null)
			return;

		Cursor.SetCursor(cursors[cursorType].cursorTextures[index], cursors[cursorType].hotspot, CursorMode.Auto);
	}

	#endregion
}
