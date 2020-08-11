using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLightController : MonoBehaviour
{
    public float MaxReduction;
    public float MaxIncrease;
    public float RateDamping;
    public float Strength;
    public bool StopFlickering;

    private Light torchLight;
    private float lightIntensity;
    private bool flickering;

    public void Reset() {
        MaxReduction = 0.2f;
        MaxIncrease = 0.2f;
        RateDamping = 0.1f;
        Strength = 300;
    }

    public void Start() {
        torchLight = GetComponent<Light>();
        if (torchLight == null) {
            Debug.LogError("Flicker script must have a Light Component on the same GameObject.");
            return;
        }
        lightIntensity = torchLight.intensity;
        StartCoroutine(DoFlicker());
    }

    void Update() {
        if (!StopFlickering && !flickering) {
            StartCoroutine(DoFlicker());
        }
    }

    private IEnumerator DoFlicker() {
        flickering = true;
        while (!StopFlickering) {
            torchLight.intensity = Mathf.Lerp(torchLight.intensity, Random.Range(lightIntensity - MaxReduction, lightIntensity + MaxIncrease), Strength * Time.deltaTime);
            yield return new WaitForSeconds(RateDamping);
        }
        flickering = false;
    }
}
