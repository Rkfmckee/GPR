using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {
	#region Properties
	
	private bool progressFinished;
	private float timeUntilFinished;
	private float currentTime;

	private Image currentProgress;
	private TextMeshProUGUI actionText;
	private Image cogWheel;

	private Transform parentTransform;
	private new Camera camera;

	#endregion

	#region Events

	private void Awake() {
		currentProgress = transform.Find("BarAmount").GetComponent<Image>();
		actionText = transform.Find("Action").GetComponent<TextMeshProUGUI>();
		cogWheel = transform.Find("Cog").GetComponent<Image>();

		progressFinished = false;
		currentTime = 0;
	}

	private void Start() {
		camera = References.Camera.camera;
	}

	private void Update() {		
		if (currentTime < timeUntilFinished) {
			currentTime += Time.deltaTime;

			if (parentTransform != null) {
				transform.position = camera.WorldToScreenPoint(parentTransform.position + new Vector3(0, 3, 0));
			}

			cogWheel.rectTransform.Rotate(new Vector3(0, 0, -1));
			currentProgress.rectTransform.localScale = new Vector3(currentTime / timeUntilFinished, 1, 1);
		} else {
			progressFinished = true;
			Destroy(gameObject);
		}
	}

	#endregion

	#region Methods

		#region Get/Set

		public bool IsProgressFinished() {
			return progressFinished;
		}

		#endregion

	public void SetProgressBar(string action, float timeInSeconds, Transform parent = null) {
		actionText.text = action;
		timeUntilFinished = timeInSeconds;
		parentTransform = parent;
	}

	#endregion
}
	