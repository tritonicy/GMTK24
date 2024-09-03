using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerProperties : MonoBehaviour
{
    // burada bazilari instance bazilari degil.
    [SerializeField] Transform pivot;
    public float SCALETOGET = 2;
    private int currentLevel = 0;
    public int health;
    [Range(1f, 2f)]
    private bool isGrowing;
    float elapsedTime;
    Vector3 firstScale;
    [HideInInspector] public Vector3 newScale;
    [SerializeField] Slider slider;

    private void Update() {
        if(newScale.y >= SCALETOGET) {
            currentLevel++;
            switch (currentLevel) {
                case 1:
                Debug.Log("cekmece kirildi");
                Debug.Log(currentLevel);
                SCALETOGET = 6f;
                break;

                case 2:
                // Masa kirilacak
                break;

                case 3:
                // boss gelecek
                break;
            }
        }
    }
    private void Start() {
        newScale = Vector3.one;

        slider.maxValue = health;
    }
    private void FixedUpdate() {
        if(isGrowing) {
            elapsedTime += Time.deltaTime;
            pivot.localScale = Vector3.Lerp(firstScale, newScale, elapsedTime / 1f);
        } else{
            elapsedTime = 0f;
        }
    }
    public void GiveHealth(int amountToHeal) {
        health += amountToHeal;
        slider.value = health;        
    }
    public void TakeDamage(int damage) {
        health -= damage;

        slider.value = health;

        if(health <= 0 ){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    public void GainExperience(float yAxisGrowAmount, float xzAxisGrowAmount) {    
        StartGrow(yAxisGrowAmount, xzAxisGrowAmount);
        Invoke(nameof(EndGrow), 1f);
    }
    public void StartGrow(float yAxisGrowAmount, float xzAxisGrowAmount) {
        isGrowing = true;
        firstScale = pivot.localScale;
        newScale = new Vector3(pivot.localScale.x * xzAxisGrowAmount, newScale.y * yAxisGrowAmount, pivot.localScale.z * xzAxisGrowAmount);
    }
    public void EndGrow() {
        isGrowing = false;
    }
}
