using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState{ 
    PauseState,
    PlayState
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Action OnEscapePressed;
    public GameState currentGameState = GameState.PlayState;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else{
            Destroy(this.gameObject);
        }
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            Instance.OnEscapePressed?.Invoke();
        }
    }
    public void StartGameScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SetGameState(GameState state) {
        Instance.currentGameState = state;
    }
}
