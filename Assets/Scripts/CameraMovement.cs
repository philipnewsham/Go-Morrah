using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform cameraTransform;
    public GenerateLevel generateLevel;
    private float cameraClampX = 0.0f;
    public Transform currentPlayer;

    private void OnEnable()
    {
        GenerateLevel.OnLevelGenerated += OnLevelGenerated;
        GenerateLevel.OnPlayerSpawned += OnPlayerSpawned;
    }

    private void OnPlayerSpawned(Transform playerTransform)
    {
        currentPlayer = playerTransform;
    }

    private void OnLevelGenerated(LevelData levelData)
    {
        cameraClampX = (generateLevel.rowWidth - 10) * 16;
    }

    private void Update()
    {
        cameraTransform.position = new Vector3(Mathf.Clamp(currentPlayer.position.x, 0.0f, cameraClampX), 0.0f, -10.0f);
    }

    private void OnDisable()
    {
        GenerateLevel.OnLevelGenerated -= OnLevelGenerated;
        GenerateLevel.OnPlayerSpawned -= OnPlayerSpawned;
    }
}
