using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Create Level Data")]
public class LevelData : ScriptableObject
{
    public int levelIndex;
    public int lives;
    [TextArea]
    public string levelLayout;
}
