﻿using System.Collections.Generic;
using UnityEngine;
using static CameraController;
using static CursorData;

public abstract class Highlightable : MonoBehaviour
{
	#region Fields

	private bool highlightingMe;
	private bool cameraInHighlightableState;
	protected Outline outline;
	protected Dictionary<CameraControllingState, List<string>> statesAndUiText;
	protected CursorType? highlightCursor;

	protected GlobalObstaclesController globalObstacles;
	protected CameraController cameraController;
	private CanvasController canvasController;
	private CursorController cursor;

	#endregion

	#region Properties

	public bool HighlightingMe {
		get => highlightingMe;
		set
		{
			highlightingMe = value;

			if (!highlightCursor.HasValue || !CameraInHighlightableState)
				return;

			if (value)
				cursor.SetCursor(highlightCursor.Value);
			else
				cursor.SetCursor(CursorType.Basic);
		}
	}

	public bool CameraInHighlightableState { get => cameraInHighlightableState; }

	#endregion

	#region Events

	protected virtual void Awake()
	{
		statesAndUiText  = new Dictionary<CameraControllingState, List<string>>();
		cameraController = Camera.main.GetComponent<CameraController>();

		outline              = gameObject.AddComponent<Outline>();
		outline.OutlineMode  = Outline.Mode.OutlineVisible;
		outline.OutlineColor = Color.yellow;
		outline.OutlineWidth = 5f;
		outline.enabled      = false;

		highlightCursor = null;

		HighlightingMe = false;
	}

	protected virtual void Start()
	{
		globalObstacles  = References.Game.globalObstacles;
		canvasController = References.UI.canvasController;
		cursor           = References.Game.cursor;
	}

	protected virtual void Update()
	{
		if (DontHighlight())
			HighlightingMe = false;

		if (!HighlightingMe)
		{
			if (outline.enabled)
			{
				outline.enabled = false;

				canvasController.DisableActionText(statesAndUiText);
			}
			return;
		}

		if (!outline.enabled)
		{
			outline.enabled = true;

			if (!canvasController.IsActionTextActive())
				canvasController.EnableActionText(statesAndUiText);
		}

		if (globalObstacles.IsTrapLinkingLineActive())
		{
			return;
		}

		if (Input.GetButtonDown("Fire1"))
		{
			LeftClicked();
		}

		if (Input.GetButtonDown("Fire2"))
		{
			RightClicked();
		}
	}

	#endregion

	#region Methods

	protected virtual void LeftClicked()
	{
		canvasController.DisableActionText(statesAndUiText);
	}
	protected virtual void RightClicked()
	{
		canvasController.DisableActionText(statesAndUiText);
	}

	protected virtual bool DontHighlight()
	{
		cameraInHighlightableState = statesAndUiText.ContainsKey(cameraController.ControllingState);

		return !cameraInHighlightableState;
	}

	#endregion
}