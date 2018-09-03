using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseLimitScript : MonoBehaviour {

    private GameManagerScript _gameManager;
    private PlayerScriptWithAnimator _playerScript;

    private void Start() {
        _gameManager = GameObject.Find("GrandDaddy/Menu Parent/Menu/Game Manager")
            .GetComponent<GameManagerScript>();
        _playerScript = GameObject.Find("Player Animation Parent/Boost Stretcher/Player")
            .GetComponent<PlayerScriptWithAnimator>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.name != "Player") return;
        _gameManager.LoseLife();
        _playerScript.Respawn();
    }
}