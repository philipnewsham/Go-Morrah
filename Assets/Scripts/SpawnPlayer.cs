using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject player;
    public Transform blockParent;
    public void Start()
    {
        SpawnAPlayer();
    }

    public void SpawnAPlayer()
    {
        GameObject newPlayer = Instantiate(player, new Vector2(-72, -16), Quaternion.identity);
        newPlayer.GetComponent<Movement>().blockParent = blockParent;
    }
}
