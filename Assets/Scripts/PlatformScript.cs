using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour {
    private PlayerScriptWithAnimator _playerScript;
    private void Start() {
        _playerScript = GameObject.Find("Player")
            .GetComponent<PlayerScriptWithAnimator>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.name != "Player") return;
        if (_playerScript.IsDestructive) {
            if (Vibration.HasVibrator()) Vibration.Vibrate(60);
            Destroy(transform.parent.gameObject);
            Destroy(gameObject);
            return;
        }
        GameManagerScript.RestartLevel();
    }
}
