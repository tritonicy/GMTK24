using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : MonoBehaviour
{
    // burada bazilari instance bazilarid egil.
    public static PlayerProperties Instance;
    [SerializeField] Transform pivot;
    private int currentLevel = 0;
    private readonly int EXPTOGET = 100;
    [HideInInspector] public int currentExperience = 0;
    public int health;
    [SerializeField] private float growAmountPerKill;
    private bool isGrowing;
    float elapsedTime;
    Vector3 firstScale;
    public Vector3 newScale;

    private void Update() {
        if(Instance.currentExperience >= EXPTOGET) {
            currentLevel++;
            switch (currentLevel) {
                case 1:
                Debug.Log("cekmece kirildi");
                Debug.Log(currentExperience);
                Debug.Log(currentLevel);
                break;

                case 2:
                // Masa kirilacak
                break;

                case 3:
                // boss gelecek
                break;
            }
            Instance.currentExperience = 0;
        }
    }
    private void Start() {
        if(Instance == null) {
            Instance = this;
        }
        else{
            Destroy(this.gameObject);
        }
    }
    private void FixedUpdate() {
        if(isGrowing) {
            elapsedTime += Time.deltaTime;
            pivot.localScale = Vector3.Lerp(firstScale, newScale, elapsedTime / 1f);
        } else{
            elapsedTime = 0f;
        }
    }
    public void TakeDamage(int damage) {
        health -= damage;

        if(health <= 0 ){
            // Debug.Log("died");
        }
    }
    public void GainExperience(int amount) {
        currentExperience += amount;
    }
    public void StartGrow() {
        isGrowing = true;
        firstScale = pivot.localScale;
        newScale = new Vector3(pivot.localScale.x, pivot.localScale.y + growAmountPerKill, pivot.localScale.z);
    }
    public void EndGrow() {
        isGrowing = false;
    }
}
