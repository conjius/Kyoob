using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {
    public string Name;

    public AudioClip Clip;

    [Range(0f, 1f)] public float Volume = .75f;
    [Range(0f, 1f)] public float VolumeVariance = .1f;

    [Range(.1f, 3f)] public float Pitch = 1f;
    [Range(0f, 1f)] public float PitchVariance = .1f;

    public bool Loop = false;

    public AudioMixerGroup MixerGroup;

    [HideInInspector] public AudioSource Source;
}