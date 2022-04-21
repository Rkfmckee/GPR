using System.Collections.Generic;
using UnityEngine;
using static CursorData;

public class CursorController : MonoBehaviour {
	#region Properties

	private Dictionary<CursorType, CursorData> cursors;

	#endregion
	
	#region Events

	private void Awake() {
		References.Game.cursor = this;
		
		string cursorPath = "Images/Cursors";

		cursors = new Dictionary<CursorType, CursorData>();
		cursors.Add(CursorType.Basic, new CursorData( Resources.Load<Texture2D>($"{cursorPath}/Basic"), new Vector2(82, 22) ));
		cursors.Add(CursorType.BasicGreen, new CursorData( Resources.Load<Texture2D>($"{cursorPath}/Basic-green"), new Vector2(82, 22) ));
		cursors.Add(CursorType.BasicRed, new CursorData( Resources.Load<Texture2D>($"{cursorPath}/Basic-red"), new Vector2(82, 22) ));
		cursors.Add(CursorType.Move, new CursorData( Resources.Load<Texture2D>($"{cursorPath}/Move"), new Vector2(128, 131) ));
		cursors.Add(CursorType.Pickup, new CursorData( Resources.Load<Texture2D>($"{cursorPath}/Pickup"), new Vector2(80, 30) ));

		SetCursor(CursorType.Basic);
	}

	#endregion

	#region Methods

	public void SetCursor(CursorType cursorType) {
		Cursor.SetCursor(cursors[cursorType].cursorTexture, cursors[cursorType].hotspot, CursorMode.Auto);
	}

	#endregion
}

public class CursorData {
	#region Properties

	public Texture2D cursorTexture;
	public Vector2 hotspot;

	#endregion

	#region Constructor

	public CursorData(Texture2D ct, Vector2 hs) {
		cursorTexture = ct;
		hotspot = hs;
	}

	#endregion

	#region Enums

	public enum CursorType {
		Basic,
		BasicGreen,
		BasicRed,
		Move,
		Pickup
	}

	#endregion
}