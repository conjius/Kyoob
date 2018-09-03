using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlatformManagerScript : MonoBehaviour {
    public float MaxTimeBetweenSpawns = 0.1f;
    public float MinTimeBetweenSpawns = 0.01f;
    public float MaxPlatformSize = 10.0f;
    public float MinPlatformSize = 1.0f;
    public int DistanceBetweenPlatforms;
    public float SpawnPosX = 50;
    public float InitialPlatformSpeed = 5.0f;
    public float PlatformSpeedAcceleration = 0.001f;
    public float PlatformSpeed = 5.0f;
    public float MaxSpawnY;
    public float MinSpawnY;
    public GameObject PlatformDebrisSystemPrefab;
    public GameObject ParentPlatformPrefab;
    public GameObject PlatformPrefab;
    public List<GameObject> Platforms;

    private GameManagerScript _gameManagerScript;
    private ParticleSystem _backgroundParticles;
    private float _timer;
    private float _timeBetweenSpawns = 0.3f;


    private float _lastSpawnedSize;

    private GameObject _templatePlatform;

    // Use this for initialization
    private void Start() {
        _gameManagerScript = GameObject
            .Find("GrandDaddy/Menu Parent/Menu/Game Manager")
            .GetComponent<GameManagerScript>();
        _timer = 0.0f;
        _templatePlatform = GameObject.Find("Start Platform");
        var instructions1 = GameObject.Find("instructions1");
        var instructions2 = GameObject.Find("instructions2");
        Platforms = new List<GameObject> {
            _templatePlatform,
            instructions1,
            instructions2
        };
        _lastSpawnedSize = 3f;
        _backgroundParticles = GameObject.Find("Background Particle System")
            .GetComponent<ParticleSystem>();
    }

    private bool IsTimeUp() {
        _timer += Time.deltaTime;
        return _timer >= _timeBetweenSpawns;
    }

    private void TweakSpawnTime() {
        // An increasing linear relation between last spawned platform size and
        // the time to wait till next spawn. The bigger the last spawned
        // platform was --->  the longer the wait till the next spawn. Below is
        // a simple linear function formula of the form
        // y=((y_2-y_1)/(x_2-x_1))(x-x_1)+y_1, where the output y is the time
        // and the input x is the last spawned size.

        _timeBetweenSpawns = (MaxTimeBetweenSpawns - MinTimeBetweenSpawns) /
                             (MaxPlatformSize - MinPlatformSize) *
                             (_lastSpawnedSize - MinPlatformSize) +
                             MinTimeBetweenSpawns;
    }

    // round a float input r to an int multiple of n and return the result
    private static int RoundToMultipleOfN(int n, float input) {
        var x = Mathf.RoundToInt(input);

        return x % n == 0 ? x : x + (n - x % n);
    }

    private void SpawnPlatform() {
        var newPlatformY =
            RoundToMultipleOfN(DistanceBetweenPlatforms,
                Random.Range(MinSpawnY, MaxSpawnY));
        var newPlatformDebrisSystem = Instantiate(PlatformDebrisSystemPrefab,
            new Vector3(
                SpawnPosX, newPlatformY, 0f), Quaternion.identity);
        var newParentPlatform = Instantiate(ParentPlatformPrefab, new Vector3(
                SpawnPosX, newPlatformY, 0f),
            Quaternion.identity,
            newPlatformDebrisSystem.transform);
        var newPlatform = Instantiate(PlatformPrefab, new Vector3(
                SpawnPosX, newPlatformY, 0f),
            Quaternion.identity, newParentPlatform.transform);
        newPlatformDebrisSystem.tag = "Platform";
        newPlatform.tag = "Platform";
        newParentPlatform.tag = "Platform";
        _lastSpawnedSize = Mathf.RoundToInt(Random.Range(MinPlatformSize,
            MaxPlatformSize));
        newPlatform.transform.localScale = new Vector3(
            _lastSpawnedSize, newParentPlatform.transform.lossyScale.y,
            newParentPlatform.transform.lossyScale.z);
        Platforms.Add(newPlatformDebrisSystem);
        TweakSpawnTime();
    }

    private void MovePlatforms() {
        foreach (var platform in Platforms) {
            if (platform == null) continue;
//            if (platform.name != "Start Platform" &&
//                platform.name != "instructions1" &&
//                platform.name != "instructions2" &&
//                platform
//                    .GetComponent<Animator>()
//                    .GetCurrentAnimatorStateInfo(0)
//                    .IsName(
//                        "PlatformParentDestroyedAnimation")) {
//                Destroy(platform.transform.GetChild(0).gameObject);
//                Destroy(platform);
//                continue;
//            }

            if (platform.CompareTag("Instructions")) {
                platform.transform.Translate(
                    Vector3.left * 1.5f * PlatformSpeed * Time.deltaTime);
            }
            else {
                platform.transform.position -=
                    new Vector3(0f, 0f, platform.transform.position.z);
                platform.transform.Translate(
                    Vector3.left * PlatformSpeed * Time.deltaTime);
            }
        }

//        Platforms.RemoveAll(platform =>
//            platform != null && platform.name != "Start Platform" &&
//            platform.name != "instructions1" &&
//            platform.name != "instructions2" &&
//            platform.GetComponentInChildren<PlatformScript>().IsDestroyed);
    }

    private void UpdatePlatformSpeed() {
        // platform speed increases linearly with score. +1 speed for every
        // 1/PlatformSpeedAcceleration points
        var score = GameObject
            .Find("Player Animation Parent/Boost Stretcher/Player")
            .GetComponent<PlayerScriptWithAnimator>().Score;
        PlatformSpeed =
            PlatformSpeedAcceleration * score + InitialPlatformSpeed;
        var backgroundParticlesMain = _backgroundParticles.main;
        backgroundParticlesMain.startSpeed = PlatformSpeed * 2;
    }

    // Update is called once per frame
    public void Update() {
        if (_gameManagerScript.IsPaused) return;
        UpdatePlatformSpeed();
        MovePlatforms();
        if (!IsTimeUp()) return;
        _timer = 0.0f;
        SpawnPlatform();
    }
}