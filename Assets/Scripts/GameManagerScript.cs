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

    private PowerUpManager _powerUpManager;

    public GameTimer Timer;

    // Use this for initialization
    private void Start() {
        _powerUpManager = GameObject.Find("Power Up Manager")
            .GetComponent<PowerUpManager>();
        Timer = new GameTimer(_powerUpManager.BoostPowerUpDuration,
            _powerUpManager.DestructionPowerUpDuration,
            _powerUpManager.MagnetPowerUpDuration);
    }


    public static void RestartLevel() {
        Vibration.Vibrate(20);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}