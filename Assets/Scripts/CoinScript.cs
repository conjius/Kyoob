using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour {
    private Collider _collider;
    private List<GameObject> _coins;
    private float _score;

    private bool _hasVibrator;
//	private Animator _animator;

    // Use this for initialization
    private void Start() {
        _hasVibrator = Vibration.HasVibrator();
        _collider = GetComponent<Collider>();
        _coins = GameObject.Find("Coin Manager")
            .GetComponent<CoinManager>().Coins;
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.gameObject.CompareTag("Player")) return;
        if (_hasVibrator) Vibration.Vibrate(10);
        GameObject.Find("Player Animation Parent/Player")
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