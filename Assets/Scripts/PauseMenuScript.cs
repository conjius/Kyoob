using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour {
    public Collider ResumeCollider;
    public Collider RestartCollider;
    public Collider SoundCollider;
    public GameObject SoundNLetter;
    public GameObject SoundF1Letter;
    public GameObject SoundF2Letter;
    public GameObject SoundCube;
    public Material SoundOnMaterial;
    public Material SoundOffMaterial;
    private GameManagerScript _gameManagerScript;
    private AudioManager _audioManager;

    // Use this for initialization
    private void Start() {
        _gameManagerScript = GameObject
            .Find("GrandDaddy/Menu Parent/Menu/Game Manager")
            .GetComponent<GameManagerScript>();
        _audioManager = GameObject.Find("Audio Manager")
            .GetComponent<AudioManager>();
        if (!_audioManager.IsMuted) return;
        SoundNLetter.SetActive(false);
        SoundF1Letter.SetActive(true);
        SoundF2Letter.SetActive(true);
        SoundCube.GetComponent<Renderer>().material = SoundOffMaterial;
    }

    // Update is called once per frame
    private void Update() {
        if (!Input.GetMouseButtonDown(0)) return;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit)) return;
        if (hit.collider == ResumeCollider) {
            _audioManager.Play("Tap");
            if (Vibration.HasVibrator()) Vibration.Vibrate(20);
            _gameManagerScript.Resume();
            return;
        }
        
        if (hit.collider == SoundCollider) {
            if (Vibration.HasVibrator()) Vibration.Vibrate(20);
            if (!_audioManager.IsMuted) {
                _audioManager.IsMuted = true;
                SoundNLetter.SetActive(false);
                SoundF1Letter.SetActive(true);
                SoundF2Letter.SetActive(true);
                SoundCube.GetComponent<Renderer>().material = SoundOffMaterial;
            }
            else {
                _audioManager.IsMuted = false;
                _audioManager.Play("Tap");
                SoundNLetter.SetActive(true);
                SoundF1Letter.SetActive(false);
                SoundF2Letter.SetActive(false);
                SoundCube.GetComponent<Renderer>().material = SoundOnMaterial;
            }
            return;
        }
        if (hit.collider != RestartCollider) return;
        _audioManager.Play("Tap");
        if (Vibration.HasVibrator()) Vibration.Vibrate(20);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}