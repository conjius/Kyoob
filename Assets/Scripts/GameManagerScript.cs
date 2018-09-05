﻿using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour {
    public class GameTimer {
        private float _boostTimer;
        private float _boostTimeLimit;
        public bool IsBoostTimeUp;

        public float DestructionTimer;
        public float DestructionTimeLimit;
        public bool IsDestructionTimeUp;

        public float MagnetTimer;
        public float MagnetTimeLimit;
        public bool IsMagnetTimeUp;

        public float ProjectilesTimer;
        public float ProjectilesTimeLimit;
        public bool IsProjectilesTimeUp;


        public GameTimer(float boostTimeLimit, float destructionTimeLimit,
            float magnetTimeLimit, float projectilesTimeLimit) {
            _boostTimer = 0.0f;
            _boostTimeLimit = boostTimeLimit;
            IsBoostTimeUp = false;

            DestructionTimer = 0.0f;
            DestructionTimeLimit = destructionTimeLimit;
            IsDestructionTimeUp = false;

            MagnetTimer = 0.0f;
            MagnetTimeLimit = magnetTimeLimit;
            IsMagnetTimeUp = false;

            ProjectilesTimer = 0.0f;
            ProjectilesTimeLimit = projectilesTimeLimit;
            IsProjectilesTimeUp = false;
        }

        public void TickBoost() {
            if (IsBoostTimeUp) return;
            _boostTimer += Time.deltaTime;
            if (_boostTimer < _boostTimeLimit) return;
            IsBoostTimeUp = true;
        }

        public void TickDestruction() {
            if (IsDestructionTimeUp) return;
            DestructionTimer += Time.deltaTime;
            if (DestructionTimer < DestructionTimeLimit) return;
            IsDestructionTimeUp = true;
        }

        public void TickMagnet() {
            if (IsMagnetTimeUp) return;
            MagnetTimer += Time.deltaTime;
            if (MagnetTimer < MagnetTimeLimit) return;
            IsMagnetTimeUp = true;
        }

        public void TickProjectiles() {
            if (IsProjectilesTimeUp) return;
            ProjectilesTimer += Time.deltaTime;
            if (ProjectilesTimer < ProjectilesTimeLimit) return;
            IsProjectilesTimeUp = true;
        }

        public void ZeroDestruction() {
            IsDestructionTimeUp = false;
            DestructionTimer = 0.0f;
        }

        public void ZeroBoost() {
            IsBoostTimeUp = false;
            _boostTimer = 0.0f;
        }

        public void ZeroMagnet() {
            IsMagnetTimeUp = false;
            MagnetTimer = 0.0f;
        }

        public void ZeroProjectiles() {
            IsProjectilesTimeUp = false;
            ProjectilesTimer = 0.0f;
        }
    }

    private Renderer _instructions1;
    private Renderer _instructions2;
    private Animator _pauseMenuAnim;
    private Animator _livesLeftTextAnim;
    private Animator _livesLeftKyoobParentAnim;
    private Animator _camAnim;
    private PowerUpManager _powerUpManager;
    public int LivesLeft;
    private TextMeshProUGUI _livesLeftText;
    public GameTimer Timer;
    public bool IsPaused;
    private bool _isEscKeyReleased;
    private PlayerScriptWithAnimator _playerScript;

    // Use this for initialization
    private void Start() {
        _powerUpManager = GameObject.Find("Power Up Manager")
            .GetComponent<PowerUpManager>();
        _livesLeftText = GameObject.Find("ScoreCanvas/Lives Count")
            .GetComponent<TextMeshProUGUI>();
        _livesLeftTextAnim = GameObject.Find("ScoreCanvas/Lives Count")
            .GetComponent<Animator>();
        _livesLeftKyoobParentAnim = GameObject
            .Find("Lives Count Kyoob Parent").GetComponent<Animator>();
        _pauseMenuAnim = GameObject.Find("GrandDaddy").GetComponent<Animator>();
        _camAnim = Camera.main.GetComponent<Animator>();
        Timer = new GameTimer(_powerUpManager.BoostPowerUpDuration,
            _powerUpManager.DestructionPowerUpDuration,
            _powerUpManager.MagnetPowerUpDuration,
            _powerUpManager.ProjectilesPowerUpDuration);
        _instructions1 =
            GameObject.Find("instructions1").GetComponent<Renderer>();
        _instructions2 =
            GameObject.Find("instructions2").GetComponent<Renderer>();
        _isEscKeyReleased = true;
        _playerScript =
            GameObject.Find(
                    "GrandDaddy/Player Animation Parent/Boost Stretcher/Player")
                .GetComponent<PlayerScriptWithAnimator>();
    }

    public void LoseLife() {
        if (Vibration.HasVibrator()) Vibration.Vibrate(20);
        _camAnim.Play("LoseLifeAnimation");
        LivesLeft--;
        if (LivesLeft < 1) {
            RestartLevel();
            return;
        }
        _livesLeftText.text = "X" + LivesLeft;
        _livesLeftTextAnim.Play("LivesCountTextPickupAnimation");
        _livesLeftKyoobParentAnim.Play("LivesCountKyoobParentPickupAnimation");
    }
    
    public void AddLife() {
        if (Vibration.HasVibrator()) Vibration.Vibrate(20);
        LivesLeft++;
        _livesLeftText.text = "X" + LivesLeft;
        _livesLeftTextAnim.Play("LivesCountTextPickupAnimation");
        _livesLeftKyoobParentAnim.Play("LivesCountKyoobParentPickupAnimation");
    }

    private void RestartLevel() {
        PlayerPrefs.SetInt("lastScore", Mathf.RoundToInt(_playerScript.Score));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ToggleInstructions() {
        if (_instructions1 != null) {
            _instructions1.enabled = !_instructions1.enabled;
        }

        if (_instructions2 != null)
            _instructions2.enabled = !_instructions2.enabled;
    }

    public void Resume() {
        ToggleInstructions();
        _pauseMenuAnim.Play("ResumeAnimation");
    }

    private void Pause() {
        _pauseMenuAnim.Play("PauseAnimation");
        ToggleInstructions();
    }

    private void EscKeyPressed() {
        if (!_isEscKeyReleased) return;
        _isEscKeyReleased = false;
        if (IsPaused) Resume();
        else Pause();
    }

    private void EscKeyReleased() {
        _isEscKeyReleased = true;
    }

    private void Update() {
        if (Input.GetKey(KeyCode.Escape)) {
            EscKeyPressed();
        }

        if (Input.GetKeyUp(KeyCode.Escape)) {
            EscKeyReleased();
        }
    }

//    private void OnApplicationPause(bool pauseStatus) {
//        IsPaused = pauseStatus;
//        ToggleInstructions();
//        if (!IsPaused) Pause();
//        else Resume();
//    }
}