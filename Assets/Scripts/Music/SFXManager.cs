using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType {
    PlayerAttack,
    Walk1,
    Walk2,
    Walk3,
    Dash1,
    Dash2,
    Jump1,
    Jump2,
    YerdenItemAlma,
    KillEnemy,
}

[RequireComponent(typeof(AudioSource))]
public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] SFXList;
    private static SFXManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public static void SetVolume(float volume) {
        instance.audioSource.volume = volume;
    }
    public static void PlaySoundFX(SoundType sound, float volume = 1)
    {
        instance.audioSource.PlayOneShot(instance.SFXList[(int)sound], volume);
    }

}
