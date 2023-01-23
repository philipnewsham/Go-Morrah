using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaySpriteLayout : MonoBehaviour
{
    public Sprite[] sprites;

    public void SetBlockSprites(GameObject[] blocks, int[] blockValues, int rowCount, int rowWidth)
    {
        int blockValueLength = blockValues.Length;

        for (int i = 0; i < blockValueLength; i++)
        {
            if(blockValues[i] == 0)
            {
                continue;
            }

            SetBlockSprite(blocks[i].GetComponent<SpriteRenderer>(), blockValues, i, rowCount, rowWidth);
        }
    }

    void SetBlockSprite(SpriteRenderer spriteRenderer, int[] blockValues, int i, int rowCount, int rowWidth)
    {
        int up      = i >= rowWidth * (rowCount - 1)    ? 0 : blockValues[i + rowWidth] ;
        int down    = i < rowWidth                      ? 0 : blockValues[i - rowWidth] ;
        int left    = i % rowWidth == 0                 ? 0 : blockValues[i - 1]        ;
        int right   = (i + 1) % rowWidth == 0           ? 0 : blockValues[i + 1]        ;
        int spriteIndex = 15 - (up + (down * 2) + (left * 4) + (right * 8));
        
        spriteRenderer.sprite = sprites[spriteIndex];
    }
}
