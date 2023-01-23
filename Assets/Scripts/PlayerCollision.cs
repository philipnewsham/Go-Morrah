using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public Vector2 originPoint;
    public GenerateLevel generateLevel;

    private void Start()
    {
        generateLevel = FindObjectOfType<GenerateLevel>();
    }

    public int[] ReturnSurroundingCellIndexes()
    {
        float x = transform.position.x - (originPoint.x - 8);
        float y = transform.position.y - (originPoint.y - 8);

        x = Mathf.FloorToInt(x / 16);
        y = Mathf.FloorToInt(y / 16);

        int targetCellIndex = Mathf.RoundToInt(x + (y * generateLevel.rowWidth));

        return ReturnSurroundingCellIndexes(targetCellIndex, generateLevel.rowWidth, generateLevel.rowCount);
    }

    int[] ReturnSurroundingCellIndexes(int targetCellIndex, int rowWidth, int rowCount)
    {
        int[] surroundingCellIndexes = new int[9];

        bool isLeftColumn   = targetCellIndex % rowWidth == 0;
        bool isRightColumn  = (targetCellIndex + 1) % rowWidth == 0;

        surroundingCellIndexes[0] = isLeftColumn ? -1 : targetCellIndex + rowWidth - 1;
        surroundingCellIndexes[1] = targetCellIndex + rowWidth;
        surroundingCellIndexes[2] = isRightColumn ? -1 : targetCellIndex + rowWidth + 1;
        surroundingCellIndexes[3] = isLeftColumn ? -1 : targetCellIndex - 1;
        surroundingCellIndexes[4] = targetCellIndex;
        surroundingCellIndexes[5] = isRightColumn ? -1 : targetCellIndex + 1;
        surroundingCellIndexes[6] = isLeftColumn ? -1 : targetCellIndex - rowWidth - 1;
        surroundingCellIndexes[7] = targetCellIndex - rowWidth;
        surroundingCellIndexes[8] = isRightColumn ? -1 : targetCellIndex - rowWidth + 1;

        return surroundingCellIndexes;
    }
}
