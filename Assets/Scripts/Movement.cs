using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Movement : MonoBehaviour
{
    public Transform blockParent;
    [Header("Vertical")]
    public float verticalSpeed = 0;
    public float jumpHeight = 30.0f;
    public float gravity = 80.0f;
    public float gravityCap = -80.0f;
    public bool isGrounded = false;
    public bool isJumping = false;
    public SpriteRenderer spriteRenderer;
    public Sprite saltSprite;
    public bool isFrozen = false;
    public GameObject player;
    public bool canMove = false;
    public Vector2 exitPosition;
    public GenerateLevel generateLevel;
    private Coroutine movementUpdateCoroutine;
    public event Action OnJump;
    public event Action OnFreeze;
    public event Action OnLand;
    public event Action OnExit;
    public event Action OnFall;
    public static event Action OnDeath;
    public bool wasGrounded = true;
    public bool hasDied = false;
    public event Action OnStartWalking;
    public event Action OnStopWalking;

    IEnumerator Start()
    {
        generateLevel = FindObjectOfType<GenerateLevel>();
        yield return new WaitForSeconds(0.1f);
        canMove = true;
        //movementUpdateCoroutine = StartCoroutine(MovementUpdate());
    }

    void Update()
    {
        //return;
        if (!canMove)
        {
            return;
        }

        if (CheckForLookingBack())
        {
            return;
        }

        CheckWalking();
        CheckForJumping();
        SetPlayerPosition();
        CheckForExitReached();
        CheckIfOffScreen();
        wasGrounded = isGrounded;
    }

    bool CheckForLookingBack()
    {
        if (Input.GetButtonDown("LeftKey"))
        {
            StartCoroutine(OnLeftKeyPressed());
            return true;
        }

        return false;
    }

    private void CheckForJumping()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
            isGrounded = false;
            verticalSpeed = jumpHeight;
            OnJump?.Invoke();
        }
    }

    IEnumerator OnLeftKeyPressed()
    {
        spriteRenderer.flipX = false;
        spriteRenderer.sprite = saltSprite;
        OnStopWalking?.Invoke();
        OnFreeze?.Invoke();
        PlayerDeath();
        generateLevel.SpawnPlayer();
        yield return StartCoroutine(WaitForLand(Input.GetButton("RightKey")));
        generateLevel.players.Add(gameObject);
        canMove = false;
    }

    IEnumerator WaitForLand(bool isMovingRight)
    {
        while (!isGrounded)
        {
            verticalSpeed = Mathf.Max(verticalSpeed - (gravity * Time.deltaTime), gravityCap);
            float x = isMovingRight ? 1 : 0;
            float y = verticalSpeed * Time.deltaTime;
            Vector3 direction = new Vector2(x, y);
            Vector3 targetPosition = transform.position + direction;
            targetPosition = CollisionDetection(targetPosition);
            transform.position = targetPosition;
            yield return null;
        }
        transform.parent = blockParent;
        this.enabled = false;
    }

    Vector3 CollisionDetection(Vector3 targetPosition)
    {
        isGrounded = false;
        int[] surroundingBlockIndexes = GetComponent<PlayerCollision>().ReturnSurroundingCellIndexes();

        for (int i = 0; i < surroundingBlockIndexes.Length; i++)
        {
            int blockIndex = surroundingBlockIndexes[i];
            if((blockIndex < 0 || blockIndex >= generateLevel.blocks.Count) || generateLevel.blockValues[blockIndex] == 0)
            {
                continue;
            }
            
            Transform block = generateLevel.blocks[blockIndex].transform;
            CollisionWithBlock(block, ref targetPosition);
        }

        for (int i = 0; i < generateLevel.players.Count; i++)
        {
            CollisionWithBlock(generateLevel.players[i].transform, ref targetPosition);
        }

        return targetPosition;
    }

    void CollisionWithBlock(Transform block, ref Vector3 targetPosition)
    {
        float xBlock = block.position.x;
        float yBlock = block.position.y;
        float xDistance = targetPosition.x - xBlock;
        float yDistance = targetPosition.y - yBlock;

        if (!IsCollidingWithBlock(xDistance, yDistance))
        {
            return;
        }

        if (Mathf.Abs(xDistance) > Mathf.Abs(yDistance))
        {
            targetPosition.x = xBlock + (xDistance < 0 ? -16 : 16);
            return;
        }

        targetPosition.y = yBlock + (yDistance < 0 ? -16 : 16);

        if (yDistance > 0)
        {
            isJumping = false;
            verticalSpeed = 0.0f;
            if (!wasGrounded)
            {
                OnLand?.Invoke();
            }
            isGrounded = true;
        }
    }

    void CheckIfOffScreen()
    {
        if (IsOffScreen())
        {
            if (!hasDied)
            {
                PlayerDeath();
                generateLevel.SpawnPlayer();
            }
            OnFall?.Invoke();
            StartCoroutine(DelayDestroyOnFalling());
        }
    }

    IEnumerator DelayDestroyOnFalling()
    {
        canMove = false;
        yield return new WaitForSeconds(1.0f);
        gameObject.SetActive(false);
    }

    bool IsOffScreen()
    {
        return transform.position.y <= -100.0f;
    }

    void CheckForExitReached()
    {
        if (HasReachedExit())
        {
            OnExit?.Invoke();
            StartCoroutine(generateLevel.ExitReached());
            canMove = false;
        }
    }

    bool HasReachedExit()
    {
        float xBlock = exitPosition.x;
        float yBlock = exitPosition.y;
        float xDistance = transform.position.x - xBlock;
        float yDistance = transform.position.y - yBlock;

        return IsCollidingWithBlock(xDistance, yDistance);
    }

    bool IsCollidingWithBlock(float xDistance, float yDistance)
    {
        return Mathf.Abs(xDistance) < 16 && Mathf.Abs(yDistance) < 16;
    }

    void SetPlayerPosition()
    {
        verticalSpeed = Mathf.Max(verticalSpeed - (gravity * Time.deltaTime), gravityCap);
        float x = Input.GetButton("RightKey") ? 1 : 0;
        float y = verticalSpeed * Time.deltaTime;
        Vector3 direction = new Vector2(x, y);
        Vector3 targetPosition = transform.position + direction;
        targetPosition = CollisionDetection(targetPosition);
        transform.position = targetPosition;
    }

    void PlayerDeath()
    {
        if (!hasDied)
        {
            hasDied = true;
            OnDeath?.Invoke();
        }
    }

    void CheckWalking()
    {
        if (Input.GetButtonDown("RightKey"))
        {
            OnStartWalking?.Invoke();
        }

        if (Input.GetButtonUp("RightKey"))
        {
            OnStopWalking?.Invoke();
        }
    }
}