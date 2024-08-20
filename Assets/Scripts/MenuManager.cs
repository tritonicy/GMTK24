using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Animator animator;

    private void Start() {
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0f);
        
        audioMixer.SetFloat("SFXVolume", sfxSlider.value);
        audioMixer.SetFloat("MusicVolume", musicSlider.value);
    }
    public void PlayGame(){
        animator.SetTrigger("FadeOut");
    }

    public void AdjustSFX(float volume) {
        audioMixer.SetFloat("SFXVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void AdjustMusic(float volume) {
        audioMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
}
