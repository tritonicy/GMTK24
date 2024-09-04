using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Linq;
using Unity.VisualScripting;

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

public class SFXManager : MonoBehaviour{
    public static SFXManager Instance;
    public GameObject oneShotClip;
    public AudioSource audioSource; 
    [SerializeField] public AudioClips[] clips;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else{
            Destroy(this.gameObject);
        }
    }
    
    public static void PlaySound3D(SoundType sound, Vector3 position, float volume = 1) {
        GameObject soundObject = new GameObject(sound.ToString());
        soundObject.transform.parent = Instance.transform;
        soundObject.transform.position = position;
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.volume = volume;
        audioSource.clip = Instance.clips.FirstOrDefault((clip) => clip.soundType == sound).clip;
        audioSource.maxDistance = 100f;
        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.dopplerLevel = 0f;
        audioSource.Play();

        Destroy(soundObject.gameObject, audioSource.clip.length);
    }
    public static void PlaySound(SoundType sound, float volume = 1) {
        if(Instance.oneShotClip == null) {
            Instance.oneShotClip = new GameObject("OneShotSound");
            Instance.oneShotClip.transform.parent = Instance.transform;
            Instance.audioSource = Instance.oneShotClip.AddComponent<AudioSource>();
        }
        Instance.audioSource.volume = volume;
        Instance.audioSource.PlayOneShot(Instance.clips.FirstOrDefault((clip) => clip.soundType == sound).clip);
    }
}
[System.Serializable]
public class AudioClips {
    public SoundType soundType;
    public AudioClip clip;
}
