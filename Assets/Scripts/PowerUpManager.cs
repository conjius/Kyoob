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
            Projectiles,
            Explosion,
            ExtraLife,
            Slowdown
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

        public static Type
            RandomType(float curPlatformSpeed,
                GameObject playerAnimationParent) {
            var rand = Random.Range(0, 23);
            switch (rand) {
                case 0:
                case 1:
                case 2:
                case 3:
                    if (playerAnimationParent.transform.position.x > 25.0f)
                        return Type.BoostBack;
                    return Type.BoostFwd;
                case 4:
                case 5:
                case 6:
                case 7:
                    if (playerAnimationParent.transform.position.x < -7.0f)
                        return Type.BoostFwd;
                    return Type.BoostBack;
                case 8:
                case 9:
                case 10:
                case 11:
                    return Type.Destruction;
                case 12:
                case 13:
                case 14:
                case 15:
                    return Type.Magnet;
                case 16:
                case 17:
                case 18:
                case 19:
                    return Type.Projectiles;
                case 20:
                    return Type.Explosion;
                case 21:
                    return Type.ExtraLife;
                case 22:
                    if (curPlatformSpeed < 20.0f)
                        return Type.ExtraLife;
                    return Type.Slowdown;
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
    public float ExplosionPowerUpDuration;
    public int SlowdownFactor;
    public Color BoostFwdColor;
    public Color BoostBackColor;
    public Color DestructionColor;
    public Color MagnetColor;
    public Color ProjectilesColor;
    public Color ExtraLifeColor;
    public Color SlowDownColor;
    public Color ExplosionColor;
    private int _distanceBetweenPlatforms;
    private PlatformManagerScript _platformManager;
    private GameObject _playerAnimationParent;
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
        _playerAnimationParent =
            GameObject.Find("GrandDaddy/Player Animation Parent");
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
        _gameManagerScript = GameObject
            .Find("GrandDaddy/Menu Parent/Menu/Game Manager")
            .GetComponent<GameManagerScript>();
    }

    private static int RoundToMultipleOfN(int n, float input) {
        var x = Mathf.RoundToInt(input);
        return x % n == 0 ? x : x + (n - x % n);
    }

    private Color GetColorByType(PowerUp.Type type) {
        switch (type) {
            case PowerUp.Type.BoostFwd: return BoostFwdColor;
            case PowerUp.Type.BoostBack: return BoostBackColor;
            case PowerUp.Type.Destruction: return DestructionColor;
            case PowerUp.Type.Magnet: return MagnetColor;
            case PowerUp.Type.Projectiles: return ProjectilesColor;
            case PowerUp.Type.ExtraLife: return ExtraLifeColor;
            case PowerUp.Type.Slowdown: return SlowDownColor;
            case PowerUp.Type.Explosion: return ExplosionColor;
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
            case PowerUp.Type.Explosion:
                return ExplosionPowerUpDuration;
            case PowerUp.Type.ExtraLife: return 0.0f;
            case PowerUp.Type.Slowdown: return 0.0f;
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
        var newType =
            PowerUp.RandomType(_platformManager.PlatformSpeed,
                _playerAnimationParent);
        newPowerUpObj.GetComponent<Renderer>().material.SetColor("_Color",
            GetColorByType(newType));
        var newPowerUpDuration = GetDurationByType(newType);
        switch (newType) {
            case PowerUp.Type.ExtraLife:
            case PowerUp.Type.Slowdown:
                newPowerUpObj.GetComponent<Animator>()
                    .Play("Flashing");
                break;
            case PowerUp.Type.BoostFwd:
            case PowerUp.Type.BoostBack:
            case PowerUp.Type.Destruction:
            case PowerUp.Type.Magnet:
            case PowerUp.Type.Projectiles:
            case PowerUp.Type.Explosion:
            default:
                newPowerUpObj.GetComponent<Animator>()
                    .Play("PowerUpState");
                break;
        }

        newPowerUpParentObj.GetComponent<PowerUpScript>().SlowdownFactor =
            SlowdownFactor;
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
        if (_timer < _timeToNextSpawn) return;
        _timer = 0.0f;
        _timeToNextSpawn = Random.Range(MinSpawnDelay, MaxSpawnDelay);
        SpawnPowerUp();
    }
}