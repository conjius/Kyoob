using TMPro;
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


        public GameTimer(float boostTimeLimit, float destructionTimeLimit,
            float magnetTimeLimit) {
            _boostTimer = 0.0f;
            _boostTimeLimit = boostTimeLimit;
            IsBoostTimeUp = false;

            DestructionTimer = 0.0f;
            DestructionTimeLimit = destructionTimeLimit;
            IsDestructionTimeUp = false;

            MagnetTimer = 0.0f;
            MagnetTimeLimit = magnetTimeLimit;
            IsMagnetTimeUp = false;
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
    }

    private Animator _livesLeftTextAnimator;
    private PowerUpManager _powerUpManager;
    public int LivesLeft;
    private TextMeshProUGUI _livesLeftText;
    public GameTimer Timer;

    // Use this for initialization
    private void Start() {
        _powerUpManager = GameObject.Find("Power Up Manager")
            .GetComponent<PowerUpManager>();
        _livesLeftText = GameObject.Find("ScoreCanvas/Lives Count")
            .GetComponent<TextMeshProUGUI>();
        _livesLeftTextAnimator = GameObject.Find("ScoreCanvas/Lives Count")
            .GetComponent<Animator>();
        Timer = new GameTimer(_powerUpManager.BoostPowerUpDuration,
            _powerUpManager.DestructionPowerUpDuration,
            _powerUpManager.MagnetPowerUpDuration);
    }

    public void LoseLife() {
        if (Vibration.HasVibrator()) Vibration.Vibrate(40);
        LivesLeft--;
        if (LivesLeft < 0) {
            RestartLevel();
            return;
        }
        _livesLeftText.text = "X" + LivesLeft;
        _livesLeftTextAnimator.Play("LivesCountTextPickupAnimation");
    }
    
    public static void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}