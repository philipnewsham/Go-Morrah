using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayLevelNumber : MonoBehaviour
{
    public Sprite[] numberSprites;
    public SpriteRenderer levelSprite;

    private void OnEnable()
    {
        GenerateLevel.OnLevelGenerated += OnLevelGenerated;
    }

    private void OnLevelGenerated(LevelData levelData)
    {
        if(levelData.levelIndex - 1 >= numberSprites.Length)
        {
            return;
        }

        levelSprite.sprite = numberSprites[levelData.levelIndex - 1];
    }

    private void OnDisable()
    {
        GenerateLevel.OnLevelGenerated -= OnLevelGenerated;
    }
}
