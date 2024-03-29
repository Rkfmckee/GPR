﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
	#region Fields

	[SerializeField]
	private float viewRadius;
	[SerializeField][Range(0, 360)]
	private float viewAngle;
	[SerializeField]
	private float viewHeightOffset;
	[SerializeField]
	private bool shouldDrawFieldOfView;
	[SerializeField]
	private LayerMask targetMask;
	[SerializeField]
	private float meshResolution;
	[SerializeField]
	private int edgeResolveIterations;
	
	private float edgeDistanceThreshold = 0.5f;
	private List<GameObject> visibleTargets = new List<GameObject>();
	private int blockingMask;
	private int obstacleMask;
	private int wallMask;
	private MeshFilter viewMeshFilter;
	private Mesh viewMesh;

	#endregion

	#region Properties

	public float ViewRadius { get => viewRadius; }

	public float ViewAngle { get => viewAngle; }

	public List<GameObject> VisibleTargets { get => visibleTargets; }

	#endregion

	#region Events

	private void Start()
	{
		CreateViewVisualisationChild();

		var layerMasks = GeneralHelper.GetLayerMasks();
		blockingMask = layerMasks["Obstacle"]
		 			| layerMasks["Wall"]
					| layerMasks["WallHidden"];

		viewMesh = new Mesh();
		viewMesh.name = "View Mesh";
		viewMeshFilter.mesh = viewMesh;

		StartCoroutine("FindTargetsWithDelay", .2f);
	}

	private void LateUpdate()
	{
		DrawFieldOfView(shouldDrawFieldOfView);
	}

	#endregion

	#region Methods

	private void CreateViewVisualisationChild()
	{
		GameObject viewVisualisation = new GameObject();
		viewVisualisation.transform.parent = gameObject.transform;
		viewVisualisation.name = "View Visualisation boi";
		viewVisualisation.transform.position = gameObject.transform.position + new Vector3(0, viewHeightOffset, 0);
		viewVisualisation.transform.rotation = transform.rotation;

		var visualisationFilter = viewVisualisation.AddComponent<MeshFilter>();
		var visualisationRenderer = viewVisualisation.AddComponent<MeshRenderer>();
		visualisationRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		visualisationRenderer.receiveShadows = false;

		visualisationRenderer.material = Resources.Load("Materials/ViewVisualisation") as Material;

		viewMeshFilter = visualisationFilter;
	}

	public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
	{
		if (!angleIsGlobal)
		{
			// If it's not a global angle, keep it relative to the gameObjects rotation
			angleInDegrees += transform.eulerAngles.y;
		}

		float xPosAngle = Mathf.Sin(angleInDegrees * Mathf.Deg2Rad);
		float zPosAngle = Mathf.Cos(angleInDegrees * Mathf.Deg2Rad);

		return new Vector3(xPosAngle, 0, zPosAngle);
	}

	private void FindVisibleTargets()
	{
		visibleTargets.Clear();
		Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

		for (int i = 0; i < targetsInViewRadius.Length; i++)
		{
			GameObject target = targetsInViewRadius[i].gameObject;
			Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

			if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
			{
				float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

				if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, blockingMask))
				{
					visibleTargets.Add(target);
				}
			}
		}
	}

	private void DrawFieldOfView(bool shouldDraw)
	{
		int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
		float stepAngleSize = viewAngle / stepCount;
		List<Vector3> viewPoints = new List<Vector3>();
		ViewCastInfo oldViewCast = new ViewCastInfo();

		if (shouldDraw)
		{
			for (int i = 0; i <= stepCount; i++)
			{
				float angle = transform.eulerAngles.y - viewAngle / 2 + (stepAngleSize * i);
				ViewCastInfo newViewCast = ViewCast(angle);

				if (i > 0)
				{
					bool edgeDistanceThresholdExceeded = Mathf.Abs(oldViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;
					if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDistanceThresholdExceeded))
					{
						EdgeInfo edge = FindEdge(oldViewCast, newViewCast);

						if (edge.pointA != Vector3.zero)
						{
							viewPoints.Add(edge.pointA);
						}

						if (edge.pointB != Vector3.zero)
						{
							viewPoints.Add(edge.pointB);
						}
					}
				}

				viewPoints.Add(newViewCast.point);
				oldViewCast = newViewCast;
			}

			int vertexCount = viewPoints.Count + 1;
			Vector3[] vertices = new Vector3[vertexCount];
			int[] triangles = new int[(vertexCount - 2) * 3];

			vertices[0] = Vector3.zero;
			for (int i = 0; i < vertexCount - 1; i++)
			{
				vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

				if (i < vertexCount - 2)
				{
					triangles[i * 3] = 0;
					triangles[i * 3 + 1] = i + 1;
					triangles[i * 3 + 2] = i + 2;
				}
			}

			viewMesh.Clear();
			viewMesh.vertices = vertices;
			viewMesh.triangles = triangles;
			viewMesh.RecalculateNormals();
		}
		else
		{
			viewMesh.Clear();
			viewMesh.RecalculateNormals();
		}
	}

	private ViewCastInfo ViewCast(float globalAngle)
	{
		Vector3 direction = DirectionFromAngle(globalAngle, true);
		RaycastHit hit;

		if (Physics.Raycast(transform.position, direction, out hit, viewRadius, blockingMask))
		{
			return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
		}
		else
		{
			return new ViewCastInfo(false, transform.position + direction * viewRadius, viewRadius, globalAngle);

		}
	}

	private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
	{
		float minAngle = minViewCast.angle;
		float maxAngle = maxViewCast.angle;
		Vector3 minPoint = Vector3.zero;
		Vector3 maxPoint = Vector3.zero;

		for (int i = 0; i < edgeResolveIterations; i++)
		{
			float angle = (minAngle + maxAngle) / 2;
			ViewCastInfo newViewCast = ViewCast(angle);

			bool edgeDistanceThresholdExceeded = Mathf.Abs(minViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;

			if (newViewCast.hit == minViewCast.hit && !edgeDistanceThresholdExceeded)
			{
				minAngle = angle;
				minPoint = newViewCast.point;
			}
			else
			{
				maxAngle = angle;
				maxPoint = newViewCast.point;
			}
		}

		return new EdgeInfo(minPoint, maxPoint);
	}

	#endregion

	#region Coroutines

	IEnumerator FindTargetsWithDelay(float delay)
	{
		while (true)
		{
			yield return new WaitForSeconds(delay);
			FindVisibleTargets();
		}
	}

	#endregion

	#region Structs

	public struct ViewCastInfo
	{
		public bool hit;
		public Vector3 point;
		public float distance;
		public float angle;

		public ViewCastInfo(bool _hit, Vector3 _point, float _distance, float _angle)
		{
			hit = _hit;
			point = _point;
			distance = _distance;
			angle = _angle;
		}
	}

	public struct EdgeInfo
	{
		public Vector3 pointA;
		public Vector3 pointB;

		public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
		{
			pointA = _pointA;
			pointB = _pointB;
		}
	}

	#endregion
}
