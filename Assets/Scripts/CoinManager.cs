using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour {
    private PlatformManagerScript _platformManager;
    public List<GameObject> Coins;
    [Range(0.0f, 10.0f)] public float MinSpawnDelay = 5.0f;
    [Range(0.0f, 10.0f)] public float MaxSpawnDelay = 10.0f;
    public GameObject CoinPrefab;
    public GameObject CoinParentPrefab;
    private float _timeToNextSpawn;
    private float _timer;
    private float _spawnPosX;
    private float _minSpawnY;
    private float _maxSpawnY;
    private float _coinSpeed;
    private int _distanceBetweenPlatforms;
    private GameManagerScript _gameManagerScript;
    private AudioManager _audioManager;


    // Use this for initialization
    private void Start() {
        Coins = new List<GameObject>();
        var platformManagerObj = GameObject.Find("Platform Manager");
        _platformManager =
            platformManagerObj.GetComponent<PlatformManagerScript>();
        _minSpawnY = _platformManager.MinSpawnY;
        _maxSpawnY = _platformManager.MaxSpawnY;
        _spawnPosX = _platformManager.SpawnPosX;
        _timer = 0.0f;
        _timeToNextSpawn = Random.Range(MinSpawnDelay, MaxSpawnDelay);
        _coinSpeed = _platformManager.PlatformSpeed;
        _distanceBetweenPlatforms = _platformManager.DistanceBetweenPlatforms;
        _gameManagerScript = GameObject
            .Find("GrandDaddy/Menu Parent/Menu/Game Manager")
            .GetComponent<GameManagerScript>();

    }

    private static int RoundToMultipleOfN(int n, float input) {
        var x = Mathf.RoundToInt(input);

        return x % n == 0 ? x : x + (n - x % n);
    }


    private void SpawnCoin() {
        var newCoinY =
            RoundToMultipleOfN(_distanceBetweenPlatforms,
                Random.Range(_minSpawnY, _maxSpawnY)) +
            (float) _distanceBetweenPlatforms / 2;
        var newCoinParentObj = Instantiate(CoinParentPrefab,
            new Vector3(_spawnPosX, newCoinY, 0), Quaternion.identity);
        var newCoinObj = Instantiate(CoinPrefab,
            new Vector3(_spawnPosX, newCoinY, 0), Quaternion.identity,
            newCoinParentObj.transform);
        newCoinObj.transform.eulerAngles = new Vector3(90f, 0f, 0f);
        newCoinObj.GetComponent<Animator>().Play("CoinState");
        newCoinObj.tag = "Coin";
        Coins.Add(newCoinParentObj);
    }


    private void MoveCoins() {
        _coinSpeed = _platformManager.PlatformSpeed;
        foreach (var coin in Coins) {
            coin.transform.Translate(
                Vector3.left * _coinSpeed * Time.deltaTime);
        }
    }

    // Update is called once per frame
    private void Update() {
        if (_gameManagerScript.IsPaused) return;
        MoveCoins();
        _timer += Time.deltaTime;
        if (!(_timer >= _timeToNextSpawn)) return;
        _timer = 0.0f;
        _timeToNextSpawn = Random.Range(MinSpawnDelay, MaxSpawnDelay);
        SpawnCoin();
    }
}