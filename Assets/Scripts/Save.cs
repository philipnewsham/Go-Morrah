using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefs.SetInt("NewGame", 1);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }
}
