using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateHandler : MonoBehaviour
{   
    private void Start() {
        GameManager.Instance.OnEscapePressed += HandleGameState;   
    }

    private void OnDisable() {
        GameManager.Instance.OnEscapePressed += HandleGameState;
    }
    public void HandleGameState() {
        GameState currentState = GameManager.Instance.currentGameState;

        if(currentState == GameState.PauseState) {
            GameManager.Instance.SetGameState(GameState.PlayState);
            DisableCursor();
            Time.timeScale = 1f;
        }
        else{
            GameManager.Instance.SetGameState(GameState.PauseState);
            EnableCursor();
            Time.timeScale = 0f;
        }
    }

    public void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
