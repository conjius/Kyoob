using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour {
    private Collider _collider;
    private List<GameObject> _coins;
    private float _score;
    private AudioManager _audioManager;

    private bool _hasVibrator;
//	private Animator _animator;

    // Use this for initialization
    private void Start() {
        _hasVibrator = Vibration.HasVibrator();
        _collider = GetComponent<Collider>();
        _coins = GameObject.Find("Coin Manager")
            .GetComponent<CoinManager>().Coins;
        _audioManager = GameObject.Find("Audio Manager")
            .GetComponent<AudioManager>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("PowerUp")) {
            for (var i = _coins.Count - 1; i >= 0; i--) {
                if (_coins[i].GetComponent<Collider>() == _collider)
                    _coins.RemoveAt(i);
            }

            Destroy(gameObject.transform.GetChild(0).gameObject);
            Destroy(gameObject);
            return;
        }
        if (!other.gameObject.CompareTag("Player")) return;
        _audioManager.Play("Coin");
        if (_hasVibrator) Vibration.Vibrate(20);
        GameObject.Find("Player Animation Parent/Boost Stretcher/Player")
            .GetComponent<PlayerScriptWithAnimator>()
            .AddToScore(10f, false);
        for (var i = _coins.Count - 1; i >= 0; i--) {
            if (_coins[i].GetComponent<Collider>() == _collider)
                _coins.RemoveAt(i);
        }

        Destroy(gameObject.transform.GetChild(0).gameObject);
        Destroy(gameObject);
    }
}