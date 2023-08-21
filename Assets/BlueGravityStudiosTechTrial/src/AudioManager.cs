using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private AudioSource _audioSource;

    private void Awake() {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayOneShot(AudioClip clip) {
        _audioSource.PlayOneShot(clip);
    }

    public void ChangeMusic(AudioClip newMusicClip) {
        _audioSource.clip = newMusicClip;
        _audioSource.Play();
    }
}
