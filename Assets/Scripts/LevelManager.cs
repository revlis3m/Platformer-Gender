using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int numberLevel;

    private void Start()
    {
        numberLevel = SceneManager.GetActiveScene().buildIndex;
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(numberLevel);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(numberLevel + 1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
