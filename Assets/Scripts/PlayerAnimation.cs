using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public SpriteRenderer playerSpriteRenderer;
    public Sprite[] walkingSprites;
    public Movement movement;
    public float timeBetweenFrames;

    private void OnEnable()
    {
        movement.OnStartWalking += OnStartWalking;
        movement.OnStopWalking += OnStopWalking;
    }

    private void OnStopWalking()
    {
        StopAllCoroutines();
    }

    private void OnStartWalking()
    {
        StartCoroutine(WalkingAnimation());
    }

    IEnumerator WalkingAnimation()
    {
        int index = 0;
        while (true)
        {
            playerSpriteRenderer.sprite = walkingSprites[index];
            index = (index + 1) % walkingSprites.Length;
            yield return new WaitForSeconds(timeBetweenFrames);
        }
    }
}
