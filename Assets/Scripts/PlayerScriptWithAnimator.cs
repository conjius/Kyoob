using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerScriptWithAnimator : MonoBehaviour {
    [Range(0.0f, 100.0f)] public float JumpForce = 50;

    [Range(9.81f, 100.0f)] public float NormalGravity;

    [Range(9.81f, 500.0f)] public float FallGravity;
    public float Score;
    public float HighScore;
    public float MaxMagnetismDistance;
    public float MagnetismForce;
    private TextMeshProUGUI _scoreText;
    private TextMeshProUGUI _highScoreText;
    private TextMeshProUGUI _lastScoreText;
    private GameManagerScript _gameManagerScript;
    private GameTimer _timer;
    private ScoreStreak _scoreStreak;
    private ParticleSystem _magnetismParticles;
    private ParticleSystem _projectilesParticles;
    private ParticleSystem _debrisParticles;
    private Transform _parent;
    private Rigidbody _rb;
    private Animator _anim;
    private Animator _boostAnim;
    private Animator _parentAnim;
    private Animator _magnetismBarAnim;
    private Animator _destructionBarAnim;
    private Animator _projectilesBarAnim;
    private List<GameObject> _coins;
    private bool _isJumpKeyReleased;
    public bool IsBoosted;
    public bool IsFirstFrameOfBoost;
    public bool IsDestructive;
    public bool IsMagnetising;
    public bool IsProjectiles;
    public bool IsFwdBoost;
    public bool IsRespawning;
    public bool IsAddingLosingLife;
    private float _boostSpeed;

    // Use this for initialization
    private void Start() {
        if (!PlayerPrefs.HasKey("highscore")) {
            PlayerPrefs.SetInt("highscore", 0);
        }
        HighScore = PlayerPrefs.GetInt("highscore");
        _gameManagerScript = GameObject
            .Find("GrandDaddy/Menu Parent/Menu/Game Manager")
            .GetComponent<GameManagerScript>();
        _timer = _gameManagerScript.Timer;
        _magnetismParticles =
            GameObject.Find("Player Animation Parent/Magnet Particle System")
                .GetComponent<ParticleSystem>();
        _projectilesParticles = GameObject.Find(
                "Projectiles Particle System")
            .GetComponent<ParticleSystem>();
        _magnetismParticles.Stop(true);
        _projectilesParticles.Stop(true);
        _debrisParticles =
            GameObject.Find("Player Animation Parent/Debris Particle System")
                .GetComponent<ParticleSystem>();
        _debrisParticles.Stop(true);
        _parent = gameObject.transform.parent.gameObject
            .GetComponentInParent<Transform>();
        Score = 0.0f;
        _scoreStreak = _gameManagerScript.ScoreStreakObj;
        if (_scoreStreak == null) Debug.LogWarning("ScoreStreakObj is null");

        _scoreText = GameObject.Find("ScoreCanvas/Score")
            .GetComponent<TextMeshProUGUI>();
        _highScoreText = GameObject.Find("ScoreCanvas/High Score")
            .GetComponent<TextMeshProUGUI>();
        _lastScoreText = GameObject.Find("ScoreCanvas/Last Score")
            .GetComponent<TextMeshProUGUI>();
        if (!PlayerPrefs.HasKey("lastScore")) {
            GameObject.Find("ScoreCanvas/Last Score").GetComponent<Animator>()
                .Play("NoLastScoreAnimation");
            PlayerPrefs.SetInt("lastScore", 0);
        }

        _highScoreText.text = "BEST: " + PlayerPrefs.GetInt("highscore");
        _lastScoreText.text = "LAST: " + PlayerPrefs.GetInt("lastScore");
        _rb = _parent.gameObject.GetComponentInParent<Rigidbody>();
        _anim = gameObject.GetComponent<Animator>();
        _boostAnim = GameObject.Find("Boost Stretcher")
            .GetComponent<Animator>();
        _parentAnim =
            _parent.transform.parent.gameObject.GetComponent<Animator>();
        _magnetismBarAnim = GameObject.Find("Magnetism Bar Parent")
            .GetComponent<Animator>();
        _destructionBarAnim = GameObject.Find("Destruction Bar Parent")
            .GetComponent<Animator>();
        _projectilesBarAnim = GameObject.Find("Projectiles Bar Parent")
            .GetComponent<Animator>();
        _coins = GameObject.Find("Coin Manager").GetComponent<CoinManager>()
            .Coins;
        _isJumpKeyReleased = true;
        IsBoosted = false;
        IsFirstFrameOfBoost = true;
        IsDestructive = false;
        IsMagnetising = false;
        IsProjectiles = false;
        IsRespawning = false;
        IsFwdBoost = true;
        IsAddingLosingLife = false;
        _boostSpeed = GameObject.Find("Power Up Manager")
            .GetComponent<PowerUpManager>().BoostSpeed;
        GravityTweak();
    }

    private void JumpKeyPressed() {
        if (!_isJumpKeyReleased) return;
        if (IsRespawning) return;
        _anim.Play(IsDestructive
            ? "JumpAnimationWithDestruction"
            : "JumpAnimation");
        _isJumpKeyReleased = false;
        _rb.velocity = new Vector3(_rb.velocity.x, JumpForce, 0);
    }

    private void JumpKeyReleased() {
        _isJumpKeyReleased = true;
    }

    private bool GetKeyboardInput() {
        if (Input.GetKey(KeyCode.Return)) {
            PlayerPrefs.SetInt("highscore", 0);
        }

        if (Input.GetKey(KeyCode.Space)) {
            JumpKeyPressed();
            return true;
        }

        if (!Input.GetKeyUp(KeyCode.Space)) return false;
        JumpKeyReleased();
        return true;
    }

    private void GetUserTouch() {
        if (Input.touchCount == 0) return;
        var touch = Input.GetTouch(0);
        switch (touch.phase) {
            case TouchPhase.Moved:
            case TouchPhase.Stationary:
            case TouchPhase.Began:
                JumpKeyPressed();
                break;
            case TouchPhase.Canceled:
            case TouchPhase.Ended:
                JumpKeyReleased();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void GetInputAndApplyMovement() {
        if (!GetKeyboardInput()) {
            GetUserTouch();
        }
    }

    private void GravityTweak() {
        if (IsBoosted) {
            Physics.gravity = new Vector3(0.0f, 0.0f, 0.0f);
            return;
        }

        Physics.gravity = _rb.velocity.y < 0.1f
            ? new Vector3(0.0f, -FallGravity, 0.0f)
            : new Vector3(0.0f, -NormalGravity, 0.0f);
    }

    private void BoostFwd() {
        if (IsFirstFrameOfBoost) {
            IsFirstFrameOfBoost = false;
            _boostAnim.Play("BoostAnimation");
        }

        _rb.velocity = new Vector3(_boostSpeed, 0.0f, 0.0f);
    }

    private void BoostBack() {
        if (IsFirstFrameOfBoost) {
            IsFirstFrameOfBoost = false;
            _boostAnim.Play("BoostAnimation");
        }

        _rb.velocity = new Vector3(-(_boostSpeed + 10.0f), 0.0f, 0.0f);
    }

    private void Unboost() {
        _boostAnim.Play("UnboostAnimation");
        IsFirstFrameOfBoost = true;
        _rb.velocity = new Vector3(0.0f, 5.0f, 0.0f);
    }


    public void AddToScore(float amount, bool isTimeBonus) {
        Score += amount;
        _scoreText.text = ((int) Score).ToString();
        if (!isTimeBonus) {
            GameObject.Find("ScoreCanvas/Score").GetComponent<Animator>()
                .Play("ScoreAnimation");
            _gameManagerScript.BroadcastMessageOrScore("+" + amount, true);
        }

        _scoreStreak.IncreaseStreakAndTryAdvanceToNextTier(
            Mathf.RoundToInt(amount));
        if (Mathf.RoundToInt(Score) <= HighScore) return;
        HighScore = Mathf.RoundToInt(Score);
        if (!isTimeBonus)
            GameObject.Find("ScoreCanvas/High Score")
                .GetComponent<Animator>()
                .Play("HighScoreAnimation");
        _highScoreText.text = "BEST: " + HighScore;
    }

    private void UpdateTimeScore() {
        AddToScore(Time.deltaTime, true);
    }

    private void ApplyBoost() {
        _timer.TickBoost();
        if (_timer.IsBoostTimeUp) {
            _timer.ZeroBoost();
            IsBoosted = false;
            Unboost();
            return;
        }

        if (IsFwdBoost) {
            BoostFwd();
        }
        else BoostBack();
    }

    private void ApplyDestruction() {
        _timer.TickDestruction();
        if (!_timer.IsDestructionTimeUp) return;
        _timer.ZeroDestruction();
        IsDestructive = false;
        _anim.Play("IdleDestructionEnd");
        _parentAnim.Play("Idle");
        _destructionBarAnim.Play("Turn Off");
    }

    private void ApplyMagnetism() {
        _timer.TickMagnet();
        if (_timer.IsMagnetTimeUp) {
            IsMagnetising = false;
            _timer.ZeroMagnet();
            PowerUpManager.StopMagnets();
            _magnetismParticles.Stop(true);
            _magnetismBarAnim.Play("Turn Off");
        }

        foreach (var coin in _coins) {
            if (coin == null) continue;
            var distance = Vector3.Distance(
                coin.transform.position,
                gameObject.transform.position);
            var coinRb = coin.GetComponent<Rigidbody>();
            if (coin.transform.position.x < -19.0f ||
                distance > MaxMagnetismDistance) {
                coinRb.isKinematic = true;
                coinRb.useGravity = false;
                coinRb.velocity = Vector3.zero;
                continue;
            }

            var forceVector = (1.0f / Mathf.Pow(distance, 2)) *
                              MagnetismForce *
                              (gameObject.GetComponent<Transform>().position -
                               coin.GetComponent<Transform>().position)
                              .normalized;
            coinRb.isKinematic = false;
            coinRb.useGravity = false;
            coinRb.AddForce(forceVector);
        }
    }

    public void Respawn() {
        _parent.transform.parent.position = new Vector3(0.0f, 1.5f, 0.0f);
        _parentAnim.Play("PlayerParentRespawnAnimation");
    }


    private void ApplyProjectiles() {
        _projectilesParticles.transform.position = transform.position;
        _timer.TickProjectiles();
        if (!_timer.IsProjectilesTimeUp) return;
        IsProjectiles = false;
        _timer.ZeroProjectiles();
        _projectilesParticles.Stop(true);
        _projectilesBarAnim.Play("Turn Off");
    }

    // Update is called once per frame
    private void Update() {
        if (_gameManagerScript.IsPaused) {
            return;
        }

        UpdateTimeScore();
        if (IsBoosted) ApplyBoost();
        if (IsDestructive) ApplyDestruction();
        if (IsMagnetising) ApplyMagnetism();
        if (IsProjectiles) ApplyProjectiles();
        GravityTweak();
        GetInputAndApplyMovement();
    }
}