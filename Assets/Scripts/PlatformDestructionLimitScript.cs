using System.Collections.Generic;
using UnityEngine;

public class PlatformDestructionLimitScript : MonoBehaviour {
    private List<GameObject> _platforms;
    private List<GameObject> _coins;
    private List<PowerUpManager.PowerUp> _powerUps;

    private void Start() {
        var platformManagerScript = GameObject.Find("Platform Manager")
            .GetComponent<PlatformManagerScript>();
        var powerUpManagerScript = GameObject.Find("Power Up Manager")
            .GetComponent<PowerUpManager>();
        var coinManager = GameObject.Find("Coin Manager")
            .GetComponent<CoinManager>();
        _platforms = platformManagerScript.Platforms;
        _powerUps = powerUpManagerScript.PowerUps;
        _coins = coinManager.Coins;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.name == "Start Platform") return;
        switch (other.gameObject.tag) {
            case "PowerUp":
                for (var i = _powerUps.Count - 1; i >= 0; i--)
                {
                    if (_powerUps[i]._object.GetComponent<Collider>() == other)
                        _powerUps.RemoveAt(i);
                }
                Destroy(other.gameObject);
                break;
            case "Platform":
                _platforms.Remove(other.gameObject);
                Destroy(other.gameObject.transform.parent.gameObject);
                break;
            case "Coin":
                _coins.Remove(other.gameObject);
                Destroy(other.gameObject);
                break;
            case "Instructions": break;
        }

        Destroy(other.gameObject);
    }
}