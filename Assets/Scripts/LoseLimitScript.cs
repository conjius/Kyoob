using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseLimitScript : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if (other.name != "Player") return;
        GameManagerScript.RestartLevel();
    }
}