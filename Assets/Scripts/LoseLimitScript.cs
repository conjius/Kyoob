using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseLimitScript : MonoBehaviour {

    private GameManagerScript _gameManager;

    private void Start() {
        _gameManager = GameObject.Find("Game Manager")
            .GetComponent<GameManagerScript>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.name != "Player") return;
        _gameManager.LoseLife();
    }
}