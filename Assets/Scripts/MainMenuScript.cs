using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {
    public bool IsWaiting;
    public Collider PlayCollider;
    private Animator _mainMenuAnim;

    // Use this for initialization
    private void Start() {
        IsWaiting = true;
        _mainMenuAnim = gameObject.GetComponent<Animator>();
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
        if (Vibration.HasVibrator()) Vibration.Vibrate(10);
        _mainMenuAnim.Play("StartGameAnimation");
    }
}