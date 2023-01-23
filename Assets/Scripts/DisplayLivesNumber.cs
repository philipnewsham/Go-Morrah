using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayLivesNumber : MonoBehaviour
{
    public Sprite[] numberSprites;
    public SpriteRenderer livesSprite;

    private void OnEnable()
    {
        LifeTracker.OnLivesUpdated += OnLivesUpdated;
    }

    private void OnLivesUpdated(int currentLives)
    {
        livesSprite.sprite = numberSprites[currentLives];
    }

    private void OnDisable()
    {
        LifeTracker.OnLivesUpdated -= OnLivesUpdated;
    }
}
