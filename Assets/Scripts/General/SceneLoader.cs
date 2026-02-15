using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(int value)
        => SceneManager.LoadScene(value);
    public void LoadScene(string value)
        => SceneManager.LoadScene(value);
    public void RestartScene()
        => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}
