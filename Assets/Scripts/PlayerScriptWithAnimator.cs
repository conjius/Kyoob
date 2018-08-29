using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScriptWithAnimator : MonoBehaviour {
    [Range(0.0f, 100.0f)] public float JumpForce = 50;

    [Range(9.81f, 100.0f)] public float NormalGravity;

    [Range(9.81f, 500.0f)] public float FallGravity;
    public float Score;
    public float MaxMagnetismDistance;
    public float MagnetismForce;
    private TextMeshProUGUI _scoreText;
    private TextMeshProUGUI _highScoreText;
    private GameManagerScript.GameTimer _timer;
    private ParticleSystem _magnetismParticles;
    private Transform _parent;
    private Rigidbody _rb;
    private Animator _anim;
    private Animator _magnetismBarAnim;
    private Animator _destructionBarAnim;
    private List<GameObject> _coins;
    private bool _isReleased;
    public bool IsBoosted;
    public bool IsDestructive;
    public bool IsMagnetising;
    public bool IsFwdBoost;
    private float _boostSpeed;

    // Use this for initialization
    private void Start() {
        if (!PlayerPrefs.HasKey("highscore")) {
            PlayerPrefs.SetInt("highscore", 0);
        }

        _timer = GameObject.Find("Game Manager")
            .GetComponent<GameManagerScript>().Timer;
        _magnetismParticles =
            GameObject.Find("Player Animation Parent/Magnet Particle System")
                .GetComponent<ParticleSystem>();
        _magnetismParticles.Stop(true);
        _parent = transform.parent.transform;
        Score = 0.0f;
        _scoreText = GameObject.Find("ScoreCanvas/Score")
            .GetComponent<TextMeshProUGUI>();
        _highScoreText = GameObject.Find("ScoreCanvas/High Score")
            .GetComponent<TextMeshProUGUI>();
        _highScoreText.text = "BEST: " + PlayerPrefs.GetInt("highscore");
        _rb = _parent.gameObject.GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _magnetismBarAnim = GameObject.Find("Magnetism Bar Parent")
            .GetComponent<Animator>();
        _destructionBarAnim = GameObject.Find("Destruction Bar Parent")
            .GetComponent<Animator>();
        _coins = GameObject.Find("Coin Manager").GetComponent<CoinManager>()
            .Coins;
        _isReleased = true;
        IsBoosted = false;
        IsDestructive = false;
        IsMagnetising = false;
        IsFwdBoost = true;
        _boostSpeed = GameObject.Find("Power Up Manager")
            .GetComponent<PowerUpManager>().BoostSpeed;
        GravityTweak();
    }

    private void Pressed() {
        if (!_isReleased) return;
        _anim.Play(IsDestructive
            ? "JumpAnimationWithDestruction"
            : "JumpAnimation");
        _isReleased = false;
//        _rb.MovePosition(transform.position + transform.up * Time.deltaTime * JumpForce);
        _rb.velocity = new Vector3(_rb.velocity.x, JumpForce, 0);
    }

    private void Released() {
        _isReleased = true;
    }

    private bool GetKeyboardInput() {
        if (Input.GetKey(KeyCode.Escape)) {
            GameManagerScript.RestartLevel();
        }

        if (Input.GetKey(KeyCode.Return)) {
            PlayerPrefs.SetInt("highscore", 0);
        }

        if (Input.GetKey(KeyCode.Space)) {
            Pressed();
            return true;
        }

//        var input = -Input.GetAxis("Horizontal");
//        if (input < 0.01f && input > -0.01f) return false;
//        _rb.velocity = new Vector3(0f, input * MovementSpeed, 0f);
//        return true;

        if (!Input.GetKeyUp(KeyCode.Space)) return false;
        Released();
        return true;
    }

    private void GetUserTouch() {
        if (Input.touchCount == 0) return;
        var touch = Input.GetTouch(0);
        switch (touch.phase) {
            case TouchPhase.Moved:
            case TouchPhase.Stationary:
            case TouchPhase.Began:
                Pressed();
                break;
            case TouchPhase.Canceled:
            case TouchPhase.Ended:
                Released();
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
        _parent.transform.localScale = new Vector3(2.0f, 0.9f, 1.0f);
        _rb.velocity = new Vector3(_boostSpeed, 0.0f, 0.0f);
    }

    private void BoostBack() {
        _parent.transform.localScale = new Vector3(2.0f, 0.9f, 1.0f);
        _rb.velocity = new Vector3(-1.0f * _boostSpeed, 0.0f, 0.0f);
    }

    private void Unboost() {
        _parent.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        _rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        _rb.AddForce(new Vector3(0.0f, 10.0f, 0.0f));
    }

    public void AddToScore(float amount, bool isTimeBonus) {
        Score += amount;
        _scoreText.text = ((int) Score).ToString();
        if (!isTimeBonus) {
            GameObject.Find("ScoreCanvas/Score").GetComponent<Animator>()
                .Play("ScoreState");
        }

        if (Mathf.RoundToInt(Score) < PlayerPrefs.GetInt("highscore")) return;
        PlayerPrefs.SetInt("highscore", Mathf.RoundToInt(Score));
        if (!isTimeBonus)
            GameObject.Find("ScoreCanvas/High Score")
                .GetComponent<Animator>()
                .Play("HighScoreAnimation");
        _highScoreText.text = "BEST: " + PlayerPrefs.GetInt("highscore");
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
            if (coin.transform.position.x < -19 ||
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

    // Update is called once per frame
    private void Update() {
        UpdateTimeScore();
        if (IsBoosted) ApplyBoost();
        if (IsDestructive) ApplyDestruction();
        if (IsMagnetising) ApplyMagnetism();
        GravityTweak();
        GetInputAndApplyMovement();
    }
}