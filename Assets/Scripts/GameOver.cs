using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverText;

    private void OnEnable()
    {
        LifeTracker.OnGameOver += OnGameOver;
    }
    
    private void OnGameOver()
    {
        gameOverText.SetActive(true);
        StartCoroutine(ResetGame());
    }

    IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(0);
    }

    private void OnDisable()
    {
        LifeTracker.OnGameOver -= OnGameOver;
    }

}
