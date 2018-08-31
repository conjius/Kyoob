using System;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour {
    private GameObject _player;
    private PlayerScriptWithAnimator _playerScript;
    private PowerUpManager _powerUpManager;
    private Animator _anim;
    private Animator _parentAnim;
    private Animator _destructionBarAnim;
    private Animator _magnetismBarAnim;
    private Collider _collider;
    private GameManagerScript.GameTimer _timer;
    private ParticleSystem _magnetismParticles;

    private List<PowerUpManager.PowerUp> _powerUps;

    private PowerUpManager.PowerUp.Type _type;
    private bool _hasVibrator;

    // Use this for initialization
    private void Start() {
        _hasVibrator = Vibration.HasVibrator();
        _player = GameObject.Find("Player Animation Parent/Player");
        _anim = _player.GetComponent<Animator>();
        _parentAnim =
            _player.transform.parent.gameObject.GetComponent<Animator>();
        _destructionBarAnim = GameObject.Find("Destruction Bar Parent")
            .GetComponent<Animator>();
        _magnetismBarAnim = GameObject.Find("Magnetism Bar Parent")
            .GetComponent<Animator>();
        _playerScript = _player.GetComponent<PlayerScriptWithAnimator>();
        _powerUpManager = GameObject.Find("Power Up Manager")
            .GetComponent<PowerUpManager>();
        _collider = GetComponent<Collider>();
        _timer = GameObject.Find("GrandDaddy/Menu/Game Manager")
            .GetComponent<GameManagerScript>().Timer;
        _magnetismParticles =
            GameObject.Find("Player Animation Parent/Magnet Particle System")
                .GetComponent<ParticleSystem>();
        _powerUps = GameObject.Find("Power Up Manager")
            .GetComponent<PowerUpManager>().PowerUps;
        if (_powerUps == null) return;
        foreach (var powerup in _powerUps) {
            if (powerup._object == gameObject) _type = powerup._type;
        }
    }

    private void LateUpdate() {
        transform.GetChild(0).gameObject.GetComponent<Renderer>().material
            .color = _powerUpManager.GetColorByType(_type);
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.gameObject.CompareTag("Player")) return;
        if (_hasVibrator) Vibration.Vibrate(20);
        switch (_type) {
            case PowerUpManager.PowerUp.Type.BoostFwd:
                _playerScript.IsBoosted = true;
                _playerScript.IsFwdBoost = true;
                break;
            case PowerUpManager.PowerUp.Type.BoostBack:
                _playerScript.IsBoosted = true;
                _playerScript.IsFwdBoost = false;
                break;
            case PowerUpManager.PowerUp.Type.Destruction:
                _playerScript.IsDestructive = true;
                _anim.Play("IdleDestructionStart");
                _parentAnim.Play("PlayerParentIdleAnimationWithDestruction");
                _destructionBarAnim.Play("Turn On");
                _timer.ZeroDestruction();
                break;
            case PowerUpManager.PowerUp.Type.Magnet:
                _playerScript.IsMagnetising = true;
                _magnetismParticles.Play(true);
                _magnetismBarAnim.Play("Turn On");
                _timer.ZeroMagnet();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        for (var i = _powerUps.Count - 1; i >= 0; i--) {
            if (_powerUps[i]._object.GetComponent<Collider>() ==
                _collider)
                _powerUps.RemoveAt(i);
        }

        _playerScript.AddToScore(100f, false);
        Destroy(gameObject.transform.GetChild(0).gameObject);
        Destroy(gameObject);
    }
}