using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour {
    public Collider ResumeCollider;
    public Collider RestartCollider;
    private GameManagerScript _gameManagerScript;
    private AudioManager _audioManager;

    // Use this for initialization
    private void Start() {
        _gameManagerScript = GameObject
            .Find("GrandDaddy/Menu Parent/Menu/Game Manager")
            .GetComponent<GameManagerScript>();
        _audioManager = GameObject.Find("Audio Manager")
            .GetComponent<AudioManager>();
    }

    // Update is called once per frame
    private void Update() {
        if (!Input.GetMouseButtonDown(0)) return;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit)) return;
        if (hit.collider == ResumeCollider) {
            _audioManager.Play("Tap");
            if (Vibration.HasVibrator()) Vibration.Vibrate(20);
            _gameManagerScript.Resume();
            return;
        }

        if (hit.collider != RestartCollider) return;
        _audioManager.Play("Tap");
        if (Vibration.HasVibrator()) Vibration.Vibrate(20);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}