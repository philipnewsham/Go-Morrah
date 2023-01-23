using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackRemainingLives : MonoBehaviour
{
    public LifeTracker lifeTracker;

    private void OnEnable()
    {
        GenerateLevel.OnExitReached += OnExitReached;
    }

    private void OnExitReached()
    {
        PlayerPrefs.SetInt("SavedLives", PlayerPrefs.GetInt("SavedLives") + lifeTracker.currentLives);
        Debug.Log("SavedLives");
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("SavedLives"))
        {
            PlayerPrefs.SetInt("SavedLives", 0);
        }
    }

    private void OnDisable()
    {
        GenerateLevel.OnExitReached -= OnExitReached;
    }

}
