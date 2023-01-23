using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningScreen : MonoBehaviour
{
    public GameObject openingScreen;

    private void OnEnable()
    {
        openingScreen.SetActive(!PlayerPrefs.HasKey("NewGame"));
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1.0f);
        openingScreen.SetActive(false);
    }
}
