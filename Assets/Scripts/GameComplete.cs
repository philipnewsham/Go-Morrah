using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameComplete : MonoBehaviour
{
    public GameObject openingScreen;
    public GameObject restartText;

    [Header("Congratulations")]
    public GameObject congratualationsText;
    public int flashCount;
    public float flashDuration;

    [Header("RemainingLives")]
    public GameObject remainingLivesText;
    public GameObject remainingLivesNumber;
    public GameObject remainingLivesNumber2;
    public SpriteRenderer tenNumberSpriteRenderer;
    public SpriteRenderer singleNumberSpriteRenderer;
    public Sprite[] numberSprites;

    public void OnGameComplete()
    {
        openingScreen.SetActive(true);
        StartCoroutine(CompleteAnimation());
    }

    IEnumerator CompleteAnimation()
    {
        yield return StartCoroutine(FlashCongratulationsText());
        yield return StartCoroutine(EnableRemaingLives());
        StartCoroutine(WaitForRestart());
    }

    IEnumerator FlashCongratulationsText()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(flashDuration);
        for (int i = 0; i < flashCount; i++)
        {
            congratualationsText.SetActive(true);
            yield return waitForSeconds;
            congratualationsText.SetActive(false);
            yield return waitForSeconds;
        }
        congratualationsText.SetActive(true);
    }

    IEnumerator EnableRemaingLives()
    {
        yield return new WaitForSeconds(1.0f);
        remainingLivesText.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        int savedLives = PlayerPrefs.GetInt("SavedLives");
        tenNumberSpriteRenderer.sprite = numberSprites[Mathf.FloorToInt((float)savedLives / 10.0f)];
        singleNumberSpriteRenderer.sprite = numberSprites[savedLives % 10];
        remainingLivesNumber.SetActive(true);
        remainingLivesNumber2.SetActive(true);
    }

    IEnumerator WaitForRestart()
    {
        yield return new WaitForSeconds(1.0f);
        yield return new WaitUntil(() => Input.anyKey);
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }
}
