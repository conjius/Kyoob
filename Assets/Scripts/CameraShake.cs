using UnityEngine;

public class CameraShake : MonoBehaviour {
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform CamTransform;

    // How long the object should shake for.
    private float _shakeDuration;
    public float InitialShakeDuration = 0.5f;
    public float InitialShakeAmount = 0.5f;
    public float DampeningAmount = 1f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float ShakeAmount = 0.7f;

    private Vector3 _originalPos;

    private void Awake() {
        CamTransform = GetComponent<Transform>();
        _shakeDuration = InitialShakeDuration;
    }

    private void OnEnable() {
        _originalPos = CamTransform.localPosition;
    }

    private void OnDisable() {
        _shakeDuration = InitialShakeDuration;
        ShakeAmount = InitialShakeAmount;
        CamTransform.localPosition = _originalPos;
    }

    private void Update() {
        if (_shakeDuration > 0) {
            CamTransform.localPosition =
                _originalPos + Random.insideUnitSphere * ShakeAmount;

            _shakeDuration -= Time.deltaTime;
            ShakeAmount -= Time.deltaTime * DampeningAmount;
        }
        else {
            _shakeDuration = 0f;
            CamTransform.localPosition = _originalPos;
        }
    }
}