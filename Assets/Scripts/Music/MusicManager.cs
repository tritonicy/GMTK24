using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
public enum MusicType {
    MenuMusic,
    InGameMusic
}
[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] musicList;
    private static MusicManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        if(instance == null) {
            instance = this;
        }
    }
    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlayMusic(SoundType sound, float volume = 1) {
        instance.StartCoroutine(instance.EaseBetween(instance.audioSource,sound));
    }

    private IEnumerator EaseBetween(AudioSource audioSource, SoundType sound) {
        float elapsedTime = 0f;
        float initialVolume = audioSource.volume;

        while(audioSource.volume > 0.05f) {
            audioSource.volume = Mathf.Lerp(initialVolume, 0f, elapsedTime / 1f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        audioSource.clip = instance.musicList[(int) sound];

        elapsedTime = 0f;
        while (audioSource.volume < initialVolume) {
            audioSource.volume = Mathf.Lerp(0f, initialVolume, elapsedTime / 1f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
