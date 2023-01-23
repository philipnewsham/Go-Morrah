using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GenerateLevel : MonoBehaviour
{
    public LevelData[] levelData;
    public int levelIndex = 0;
    public GameObject block;
    public Transform blockParent;
    public Vector2 startingPosition;
    public GameObject player;
    public GameObject exit;
    public Vector2 playerPosition;
    public Vector2 exitPosition;
    public Sprite exitSprite;
    public List<GameObject> blocks = new List<GameObject>();
    public int[] blockValues;
    public static event Action<LevelData> OnLevelGenerated;
    public static event Action<Transform> OnPlayerSpawned;
    public static event Action OnExitReached;
    public int rowWidth;
    public int rowCount;
    public List<GameObject> players = new List<GameObject>();

    public bool isGameOver = false;

    private void OnEnable()
    {
        LifeTracker.OnGameOver += OnGameOver;
    }

    private void OnGameOver()
    {
        isGameOver = true;
    }

    private void Start()
    {
        if (levelIndex == -1)
        {
            levelIndex = PlayerPrefs.HasKey("Level") ? PlayerPrefs.GetInt("Level") : 0;
        }
        GenerateLevelFromData(levelData[levelIndex]);
    }

    void GenerateLevelFromData(LevelData levelData)
    {
        string[] lines = levelData.levelLayout.Split('\n');
        rowCount = lines.Length;
        rowWidth = lines[0].Split(',').Length;
        blockValues = new int[rowCount * rowWidth];
        int index = 0;
        for (int y = rowCount - 1; y >= 0; y--)
        {
            string[] cells = lines[y].Split(',');
            for (int x = 0; x < cells.Length; x++)
            {
                switch (cells[x])
                {
                    case "1":
                        GameObject blockClone = Instantiate(block, blockParent);
                        blockClone.transform.position = ReturnPosition(x, y, rowCount);
                        blockValues[index] = 1;
                        blocks.Add(blockClone);
                        break;
                    case "S":
                        GameObject playerClone = Instantiate(player, null);
                        playerClone.GetComponent<Movement>().blockParent = blockParent;
                        playerPosition = ReturnPosition(x, y, rowCount);
                        playerClone.transform.position = playerPosition;
                        OnPlayerSpawned?.Invoke(playerClone.transform);
                        blockValues[index] = 0;
                        blocks.Add(null);
                        break;
                    case "E":
                        exitPosition = ReturnPosition(x, y, rowCount);
                        GameObject exitClone = Instantiate(exit, exitPosition, Quaternion.identity);
                        exitClone.name = "Exit";
                        exitClone.GetComponent<SpriteRenderer>().sprite = exitSprite;
                        blockValues[index] = 0;
                        blocks.Add(null);
                        break;
                    default:
                        blockValues[index] = 0;
                        blocks.Add(null);
                        break;
                }
                index++;
            }
        }
        FindObjectOfType<DisplaySpriteLayout>().SetBlockSprites(blocks.ToArray(), blockValues, rowCount, rowWidth);
        FindObjectOfType<Movement>().exitPosition = exitPosition;
        OnLevelGenerated?.Invoke(levelData);
    }

    Vector2 ReturnPosition(int x, int y, int length)
    {
        return startingPosition + new Vector2(x * 16, ((length - 1) - y) * 16);
    }

    public void SpawnPlayer()
    {
        if (!isGameOver)
        {
            GameObject newPlayer = Instantiate(player, playerPosition, Quaternion.identity);
            newPlayer.GetComponent<Movement>().blockParent = blockParent;
            newPlayer.GetComponent<Movement>().exitPosition = exitPosition;
            OnPlayerSpawned?.Invoke(newPlayer.transform);
        }
    }

    public IEnumerator ExitReached()
    {
        OnExitReached?.Invoke();
        yield return new WaitForSeconds(0.5f);
        SetLevelIndex();
        if (PlayerPrefs.GetInt("Level") < levelData.Length)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            yield break;
        }

        FindObjectOfType<GameComplete>().OnGameComplete();
    }

    void SetLevelIndex()
    {
        if (PlayerPrefs.HasKey("Level"))
        {
            PlayerPrefs.SetInt("Level", (PlayerPrefs.GetInt("Level") + 1));
            return;
        }

        PlayerPrefs.SetInt("Level", 1 % levelData.Length);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    private void OnDisable()
    {
        LifeTracker.OnGameOver -= OnGameOver;
    }
}