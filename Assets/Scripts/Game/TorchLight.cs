using System.Collections;
using UnityEngine;

public class TorchLight : MonoBehaviour
{
	#region Fields

	[SerializeField]
	private float maxReduction;
	[SerializeField]
	private float maxIncrease;
	[SerializeField]
	private float rateDamping;
	[SerializeField]
	private float strength;
	[SerializeField]
	private bool stopFlickering;

	private Light torchLight;
	private float lightIntensity;
	private bool flickering;

	#endregion

	#region Events

	public void Start()
	{
		torchLight = GetComponent<Light>();
		if (torchLight == null)
		{
			Debug.LogError("Flicker script must have a Light Component on the same GameObject.");
			return;
		}
		lightIntensity = torchLight.intensity;
		StartCoroutine(DoFlicker());
	}

	void Update()
	{
		if (!stopFlickering && !flickering)
		{
			StartCoroutine(DoFlicker());
		}
	}

	#endregion

	#region Methods

	public void Reset()
	{
		maxReduction = 0.15f;
		maxIncrease  = 0.15f;
		rateDamping  = 0.1f;
		strength     = 300;
	}

	#endregion

	#region Coroutines

	private IEnumerator DoFlicker()
	{
		flickering = true;
		while (!stopFlickering)
		{
			torchLight.intensity = Mathf.Lerp(torchLight.intensity, Random.Range(lightIntensity - maxReduction, lightIntensity + maxIncrease), strength * Time.deltaTime);
			yield return new WaitForSeconds(rateDamping);
		}
		flickering = false;
	}

	#endregion
}
