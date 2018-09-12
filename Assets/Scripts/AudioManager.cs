using UnityEngine.Audio;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;

    public AudioMixerGroup MixerGroup;
    public bool IsMuted;
    public Sound[] Sounds;
    public bool FirstGameFrame = true;

    private void Awake() {
        IsMuted = false;
        if (Instance != null) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        foreach (var s in Sounds) {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.Clip;
            s.Source.loop = s.Loop;

            s.Source.outputAudioMixerGroup = MixerGroup;
        }
    }

    public void Play(string sound) {
        if (IsMuted) return;
        var s = Array.Find(Sounds, item => item.Name == sound);
        if (s == null) {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.Source.volume = s.Volume *
                          (1f + UnityEngine.Random.Range(-s.VolumeVariance / 2f,
                               s.VolumeVariance / 2f));
        s.Source.pitch = s.Pitch *
                         (1f + UnityEngine.Random.Range(-s.PitchVariance / 2f,
                              s.PitchVariance / 2f));

        if (s.Source.isPlaying) s.Source.Stop();
        s.Source.PlayOneShot(s.Clip, 1f);
    }

    public void PauseAll() {
        foreach (var s in Sounds) {
            if (s == null) continue;
            if (!s.Source.isPlaying) continue;
            s.Source.Pause();
            s.IsPaused = true;
        }
    }

    public void ResumeAll() {
        if (IsMuted) return;
        foreach (var s in Sounds) {
            if (s == null) continue;
            if (!s.IsPaused) continue;
            s.Source.UnPause();
            s.IsPaused = false;
        }
    }

    public void StopSounds() {
        foreach (var s in Sounds) {
            if (s == null) continue;
            s.Source.Stop();
        }
    }

    private void Update() {
        if (!FirstGameFrame ||
            SceneManager.GetActiveScene().buildIndex == 0) return;
        if (!IsMuted) Play("Turn On");
        FirstGameFrame = false;
    }
}