using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LifeTracker : MonoBehaviour
{
    public int currentLives;
    public static event Action OnGameOver;
    public static event Action<int> OnLivesUpdated;

    private void OnEnable()
    {
        Movement.OnDeath += OnPlayerDeath;
        GenerateLevel.OnLevelGenerated += OnLevelGenerated;
    }

    private void OnLevelGenerated(LevelData levelData)
    {
        currentLives = levelData.lives;
        OnLivesUpdated?.Invoke(currentLives);
    }

    public void OnPlayerDeath()
    {
        currentLives--;
        OnLivesUpdated?.Invoke(currentLives);
        if (currentLives <= 0)
        {
            OnGameOver?.Invoke();
        }
    }

    private void OnDisable()
    {
        Movement.OnDeath -= OnPlayerDeath;
        GenerateLevel.OnLevelGenerated -= OnLevelGenerated;
    }
}
