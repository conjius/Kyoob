using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour {
    private PlayerScriptWithAnimator _playerScript;
    private Animator _playerParentAnim;
    private ParticleSystem _playerDebrisParticleSystem;
    private ParticleSystem _platformDebrisParticleSystem;
    private GameManagerScript _gameManager;

    private void Start() {
        _playerScript = GameObject.Find("Player Animation Parent/Player")
            .GetComponent<PlayerScriptWithAnimator>();
        _gameManager = GameObject.Find("GrandDaddy/Menu/Game Manager")
            .GetComponent<GameManagerScript>();
        _playerParentAnim = GameObject.Find("Player Animation Parent")
            .GetComponent<Animator>();
        _playerDebrisParticleSystem = GameObject
            .Find("Player Animation Parent/Debris Particle System")
            .GetComponent<ParticleSystem>();
        _platformDebrisParticleSystem =
            gameObject.transform.parent.gameObject
                .GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.name != "Player") return;
        _platformDebrisParticleSystem.Play(true);
        gameObject.transform.parent.gameObject.GetComponent<Animator>()
            .Play("PlatformParentDestroyAnimation");
        if (Vibration.HasVibrator()) Vibration.Vibrate(40);
        if (!_playerScript.IsDestructive && !_playerScript.IsBoosted) {
            _gameManager.LoseLife();
            _playerDebrisParticleSystem.Play(true);
            _playerParentAnim.Play("PlayerParentHitAnimation");
        }

        var components = gameObject.GetComponents<Collider>();
        Destroy(components[0]);
        Destroy(components[1]);
    }
}