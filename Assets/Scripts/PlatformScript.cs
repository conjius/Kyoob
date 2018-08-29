using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour {
    private PlayerScriptWithAnimator _playerScript;
    private GameManagerScript _gameManager;

    private void Start() {
        _playerScript = GameObject.Find("Player")
            .GetComponent<PlayerScriptWithAnimator>();
        _gameManager = GameObject.Find("Game Manager")
            .GetComponent<GameManagerScript>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.name != "Player") return;
        if (!_playerScript.IsDestructive) _gameManager.LoseLife();
        Destroy(transform.parent.gameObject);
        Destroy(gameObject);
    }
}