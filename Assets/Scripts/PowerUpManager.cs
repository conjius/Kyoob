using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerUpManager : MonoBehaviour {
    public class PowerUp {
        public enum Type {
            BoostFwd,
            BoostBack,
            Destruction,
            Magnet,
            Projectiles
        }

        public Type _type;
        private float _duration;
        public GameObject _object;
        private Color _color;

        public PowerUp(Type type, float duration, GameObject obj, Color color) {
            _type = type;
            _duration = duration;
            _object = obj;
            _color = color;
        }

        public static Type RandomType() {
            var rand = Random.Range(0, 5);
            switch (rand) {
                case 0:
                    return Type.BoostBack;
                case 1:
                    return Type.BoostFwd;
                case 2:
                    return Type.Destruction;
                case 3:
                    return Type.Magnet;
                case 4:
                    return Type.Projectiles;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    [Range(0.0f, 100.0f)] public float BoostSpeed = 5.0f;
    public float BoostPowerUpDuration;
    public float DestructionPowerUpDuration;
    public float MagnetPowerUpDuration;
    public float ProjectilesPowerUpDuration;
    public Color BoostFwdColor;
    public Color BoostBackColor;
    public Color DestructionColor;
    public Color MagnetColor;
    public Color ProjectilesColor;
    private int _distanceBetweenPlatforms;
    private PlatformManagerScript _platformManager;
    public GameObject PowerUpPrefab;
    public GameObject PowerUpParentPrefab;
    public List<PowerUp> PowerUps;
    private static List<GameObject> _coins;
    [Range(0.0f, 20.0f)] public float MinSpawnDelay = 5.0f;
    [Range(0.0f, 20.0f)] public float MaxSpawnDelay = 10.0f;
    private float _boostTimer;
    private float _timeToNextSpawn;
    private float _timer;
    private float _spawnPosX;
    private float _minSpawnY;
    private float _maxSpawnY;
    private float _powerUpSpeed;
    private GameManagerScript _gameManagerScript;


    // Use this for initialization
    private void Start() {
        PowerUps = new List<PowerUp>();
        _coins = GameObject.Find("Coin Manager").GetComponent<CoinManager>()
            .Coins;
        var platformManagerObj = GameObject.Find("Platform Manager");
        _platformManager =
            platformManagerObj.GetComponent<PlatformManagerScript>();
        _minSpawnY = _platformManager.MinSpawnY;
        _maxSpawnY = _platformManager.MaxSpawnY;
        _spawnPosX = _platformManager.SpawnPosX;
        _timer = 0.0f;
        _timeToNextSpawn = Random.Range(MinSpawnDelay, MaxSpawnDelay);
        _powerUpSpeed = _platformManager.PlatformSpeed;
        _distanceBetweenPlatforms = _platformManager.DistanceBetweenPlatforms;
        _gameManagerScript = GameObject.Find("GrandDaddy/Menu Parent/Menu/Game Manager")
            .GetComponent<GameManagerScript>();
    }

    private static int RoundToMultipleOfN(int n, float input) {
        var x = Mathf.RoundToInt(input);

        return x % n == 0 ? x : x + (n - x % n);
    }

    public Color GetColorByType(PowerUp.Type type) {
        switch (type) {
            case PowerUp.Type.BoostFwd: return BoostFwdColor;
            case PowerUp.Type.BoostBack: return BoostBackColor;
            case PowerUp.Type.Destruction: return DestructionColor;
            case PowerUp.Type.Magnet: return MagnetColor;
            case PowerUp.Type.Projectiles: return ProjectilesColor;
            default:
                throw new ArgumentOutOfRangeException("type", type, null);
        }
    }

    private float GetDurationByType(PowerUp.Type type) {
        switch (type) {
            case PowerUp.Type.BoostBack:
            case PowerUp.Type.BoostFwd: return BoostPowerUpDuration;
            case PowerUp.Type.Destruction: return DestructionPowerUpDuration;
            case PowerUp.Type.Magnet: return MagnetPowerUpDuration;
            case PowerUp.Type.Projectiles: return ProjectilesPowerUpDuration;

            default:
                throw new ArgumentOutOfRangeException("type", type, null);
        }
    }

    private void SpawnPowerUp() {
        var newPowerUpY =
            RoundToMultipleOfN(_distanceBetweenPlatforms,
                Random.Range(_minSpawnY, _maxSpawnY)) +
            (float) _distanceBetweenPlatforms / 2;
        var newPowerUpParentObj = Instantiate(PowerUpParentPrefab,
            new Vector3(_spawnPosX, newPowerUpY, 0), Quaternion.identity);
        var newPowerUpObj = Instantiate(PowerUpPrefab,
            new Vector3(_spawnPosX, newPowerUpY, 0), Quaternion.identity,
            newPowerUpParentObj.transform);
        newPowerUpObj.tag = "PowerUp";
        newPowerUpParentObj.tag = "PowerUp";
        var newType = PowerUp.RandomType();
        newPowerUpObj.GetComponent<Renderer>().material.SetColor("_Color",
            GetColorByType(newType));
        var newPowerUpDuration = GetDurationByType(newType);
        newPowerUpObj.GetComponent<Animator>().Play("PowerUpState");
        PowerUps.Add(new PowerUp(newType, newPowerUpDuration,
            newPowerUpParentObj,
            newPowerUpObj.GetComponent<Renderer>().material.color));
    }


    private void MovePowerUps() {
        _powerUpSpeed = _platformManager.PlatformSpeed;
        foreach (var powerup in PowerUps) {
            powerup._object.transform.Translate(
                Vector3.left * _powerUpSpeed * Time.deltaTime, Space.World);
        }
    }

    public static void StopMagnets() {
        foreach (var coin in _coins) {
            if (coin == null) continue;
            var coinRb = coin.GetComponent<Rigidbody>();
            coinRb.useGravity = false;
        }
    }

    // Update is called once per frame
    private void Update() {
        if (_gameManagerScript.IsPaused) return;
        MovePowerUps();
        _timer += Time.deltaTime;
        if (!(_timer >= _timeToNextSpawn)) return;
        _timer = 0.0f;
        _timeToNextSpawn = Random.Range(MinSpawnDelay, MaxSpawnDelay);
        SpawnPowerUp();
    }
}