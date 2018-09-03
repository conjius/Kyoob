using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class BarManager : MonoBehaviour {
    private Transform _destructionBarTransform;
    private Transform _magnetismBarTransform;
    private Transform _projectilesBarTransform;
    private PlayerScriptWithAnimator _playerScript;
    private GameManagerScript _gameManagerScript;
    private float _destructionTimeLimit;
    private float _magnetTimeLimit;


    // Use this for initialization
    private void Start() {
        _destructionBarTransform = GameObject
            .Find("Destruction Bar Parent/Destruction Bar")
            .GetComponent<Transform>();
        _magnetismBarTransform = GameObject
            .Find("Magnetism Bar Parent/Magnetism Bar")
            .GetComponent<Transform>();
        _projectilesBarTransform = GameObject
            .Find("Projectiles Bar Parent/Projectiles Bar")
            .GetComponent<Transform>();
        _playerScript = GameObject.Find("Player Animation Parent/Boost Stretcher/Player")
            .GetComponent<PlayerScriptWithAnimator>();
        _gameManagerScript = GameObject.Find("GrandDaddy/Menu Parent/Menu/Game Manager")
            .GetComponent<GameManagerScript>();
    }

    private static void CalculateScaleByTime(Transform transform,
        float timeLimit, float curTime) {
        var newScaleX = (3.0f / timeLimit) * curTime;
        transform.localScale = new Vector3(newScaleX, transform.localScale.y,
            transform.localScale.z);
    }

    private static void CalculatePositionByTime(Transform transform,
        float timeLimit, float curTime) {
        var newPositionX = (1.5f / timeLimit) * curTime -1.5f;
        transform.localPosition = new Vector3(newPositionX, transform.localPosition.y,
            transform.localPosition.z);
    }


    // Update is called once per frame
    private void Update() {
        if (_playerScript.IsDestructive) {
            CalculateScaleByTime(_destructionBarTransform,
                _gameManagerScript.Timer.DestructionTimeLimit,
                _gameManagerScript.Timer.DestructionTimer);
            CalculatePositionByTime(_destructionBarTransform,
                _gameManagerScript.Timer.DestructionTimeLimit,
                _gameManagerScript.Timer.DestructionTimer);

        }

        if (_playerScript.IsProjectiles) {
            CalculateScaleByTime(_projectilesBarTransform,
                _gameManagerScript.Timer.ProjectilesTimeLimit,
                _gameManagerScript.Timer.ProjectilesTimer);
            CalculatePositionByTime(_projectilesBarTransform,
                _gameManagerScript.Timer.ProjectilesTimeLimit,
                _gameManagerScript.Timer.ProjectilesTimer);
        }

        if (!_playerScript.IsMagnetising) return;
        CalculateScaleByTime(_magnetismBarTransform,
            _gameManagerScript.Timer.MagnetTimeLimit,
            _gameManagerScript.Timer.MagnetTimer);
        CalculatePositionByTime(_magnetismBarTransform,
            _gameManagerScript.Timer.MagnetTimeLimit,
            _gameManagerScript.Timer.MagnetTimer);
    }
}