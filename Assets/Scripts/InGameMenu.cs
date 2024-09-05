using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] CameraMovement cameraMovement;
    [SerializeField] GameObject inGameMenu;

    private void Update() {
        if(GameManager.Instance.currentGameState == GameState.PauseState) {
            inGameMenu.SetActive(true);
        } 
        else{
            inGameMenu.SetActive(false);
        }
    }
    public void ChangeSensivity(float value) {
        cameraMovement.sens = value;
    } 
}
