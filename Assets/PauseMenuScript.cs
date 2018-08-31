using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour {
    public Collider ResumeCollider;
    public Collider RestartCollider;
    private Animator _mainMenuAnim;

    // Use this for initialization
    private void Start() {
        _mainMenuAnim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update() {
        if (!Input.GetMouseButtonDown(0)) return;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit)) return;
        if (hit.collider == ResumeCollider) {
            _mainMenuAnim.Play("ResumeAnimation");
            return;
        }

        if (hit.collider != RestartCollider) return;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}