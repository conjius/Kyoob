using System;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour {
    private GameObject _player;
    private PlayerScriptWithAnimator _playerScript;
    private PlatformManagerScript _platformManager;
    private AudioManager _audioManager;
    private GameManagerScript _gameManager;
    private Animator _anim;
    private Animator _parentAnim;
    private Animator _destructionBarAnim;
    private Animator _projectilesBarAnim;
    private Animator _magnetismBarAnim;
    private Animator _camAnim;
    private Collider _collider;
    private GameTimer _timer;
    private ParticleSystem _magnetismParticles;
    private ParticleSystem _projectiles;
    private List<PowerUpManager.PowerUp> _powerUps;
    private PowerUpManager.PowerUp.Type _type;
    private bool _hasVibrator;

    public float SlowdownFactor;

    // Use this for initialization
    private void Start() {
        _hasVibrator = Vibration.HasVibrator();
        _player =
            GameObject.Find("Player Animation Parent/Boost Stretcher/Player");
        _platformManager = GameObject.Find("Platform Manager")
            .GetComponent<PlatformManagerScript>();
        _anim = _player.GetComponent<Animator>();
        _parentAnim =
            GameObject.Find("Player Animation Parent").GetComponent<Animator>();
        _destructionBarAnim = GameObject.Find("Destruction Bar Parent")
            .GetComponent<Animator>();
        _magnetismBarAnim = GameObject.Find("Magnetism Bar Parent")
            .GetComponent<Animator>();
        _projectilesBarAnim = GameObject.Find("Projectiles Bar Parent")
            .GetComponent<Animator>();
        _camAnim = Camera.main.GetComponent<Animator>();
        _playerScript = _player.GetComponent<PlayerScriptWithAnimator>();
        _audioManager = GameObject.Find("Audio Manager")
            .GetComponent<AudioManager>();
        _gameManager = GameObject
            .Find("GrandDaddy/Menu Parent/Menu/Game Manager")
            .GetComponent<GameManagerScript>();
        _collider = GetComponent<Collider>();
        _timer = GameObject.Find("GrandDaddy/Menu Parent/Menu/Game Manager")
            .GetComponent<GameManagerScript>().Timer;
        _magnetismParticles =
            GameObject.Find("Player Animation Parent/Magnet Particle System")
                .GetComponent<ParticleSystem>();
        _projectiles = GameObject.Find("Projectiles Particle System")
            .GetComponent<ParticleSystem>();
        _powerUps = GameObject.Find("Power Up Manager")
            .GetComponent<PowerUpManager>().PowerUps;
        if (_powerUps == null) return;
        foreach (var powerup in _powerUps) {
            if (powerup._object == gameObject) _type = powerup._type;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.gameObject.CompareTag("Player")) return;
        switch (_type) {
            case PowerUpManager.PowerUp.Type.BoostFwd:
                _audioManager.Play("BoostFwd");
                if (!_playerScript.IsAddingLosingLife &&
                    !_playerScript.IsRespawning)
                    _camAnim.CrossFadeInFixedTime("BoostFwdAnimation", 0.18f);
                _playerScript.IsBoosted = true;
                _playerScript.IsFwdBoost = true;
                break;
            case PowerUpManager.PowerUp.Type.BoostBack:
                _audioManager.Play("BoostBack");
                if (!_playerScript.IsAddingLosingLife &&
                    !_playerScript.IsRespawning)
                    _camAnim.CrossFadeInFixedTime("BoostBackAnimation", 0.18f);
                _playerScript.IsBoosted = true;
                _playerScript.IsFwdBoost = false;
                break;
            case PowerUpManager.PowerUp.Type.Destruction:
                _audioManager.Play("Destruction");
                _camAnim.CrossFadeInFixedTime("DestructionAnimation", 0.18f);
                _playerScript.IsDestructive = true;
                _anim.Play("IdleDestructionStart");
                _parentAnim.Play("PlayerParentIdleAnimationWithDestruction");
                _destructionBarAnim.Play("Turn On");
                _timer.ZeroDestruction();
                break;
            case PowerUpManager.PowerUp.Type.Magnet:
                _audioManager.Play("Magnetism");
                _camAnim.CrossFadeInFixedTime("MagnetismAnimation", 0.18f);
                _playerScript.IsMagnetising = true;
                _magnetismParticles.Play(true);
                _magnetismBarAnim.Play("Turn On");
                _timer.ZeroMagnet();
                break;
            case PowerUpManager.PowerUp.Type.Projectiles:
                _audioManager.Play("Projectiles");
                _camAnim.CrossFadeInFixedTime("ProjectilesAnimation", 0.18f);
                _playerScript.IsProjectiles = true;
                _projectiles.Play(true);
                _projectilesBarAnim.Play("Turn On");
                _timer.ZeroProjectiles();
                break;
            case PowerUpManager.PowerUp.Type.ExtraLife:
                _audioManager.Play("AddLife");
                _gameManager.AddLife();
                _camAnim.CrossFadeInFixedTime("AddLifeAnimation", 0.18f);
                _parentAnim.Play("AddLifeAnimation");
                break;
            case PowerUpManager.PowerUp.Type.Slowdown:
                _audioManager.Play("Slowdown");
                _camAnim.CrossFadeInFixedTime("SlowdownAnimation", 0.18f);
                _gameManager.Slowdown();
                _platformManager.SlowdownFactor += SlowdownFactor;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (_hasVibrator) Vibration.Vibrate(20);
        for (var i = _powerUps.Count - 1; i >= 0; i--) {
            if (_powerUps[i]._object.GetComponent<Collider>() ==
                _collider)
                _powerUps.RemoveAt(i);
        }

        _playerScript.AddToScore(50f, false);
        Destroy(gameObject.transform.GetChild(0).gameObject);
        Destroy(gameObject);
    }
}