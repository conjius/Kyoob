using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {
    public bool IsWaiting;
    public Collider PlayCollider;
    private Animator _mainMenuAnim;
    private AudioManager _audioManager;

    // Use this for initialization
    private void Start() {
        IsWaiting = true;
        _mainMenuAnim = gameObject.GetComponent<Animator>();
        _audioManager = GameObject.Find("Audio Manager")
            .GetComponent<AudioManager>();
    }

    // Update is called once per frame
    private void Update() {
        if (!IsWaiting) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +
                                   1);
            return;
        }

        if (Input.GetKey(KeyCode.Escape)) {
            Application.Quit();
        }

        if (!Input.GetMouseButtonDown(0)) return;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit) &&
            hit.collider != PlayCollider) return;
        _audioManager.Play("Tap");
        if (Vibration.HasVibrator()) Vibration.Vibrate(20);
        _mainMenuAnim.CrossFadeInFixedTime("StartGameAnimation", 0.3f);
    }
}