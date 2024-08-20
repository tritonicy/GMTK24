using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType {
    PlayerAttack,
    EnemyAttack,
    Walk,
    Walk2,
    Walk3,
    Dash,
    Jump,
    TakeDamage,
    DoneDamage,
    KillEnemy,
    Die
}

[RequireComponent(typeof(AudioSource))]
public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] SFXList;
    private static SFXManager instance;
    private AudioSource audioSource;

    private void Awake() {
        instance = this;
    }
    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }
    public static void PlaySoundFX(SoundType sound, float volume = 1) {
        instance.audioSource.PlayOneShot(instance.SFXList[(int) sound], volume);
    }
}
