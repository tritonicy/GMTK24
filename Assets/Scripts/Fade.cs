using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    public void ChangeScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void SetOff() {
        canvas.gameObject.SetActive(false);
    }
    public void SetOn() {
        canvas.gameObject.SetActive(true);
    }
}
