using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int startingLives;
    public int startMaxLives;
    public int startingMissiles;
    public Transform spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.lives = startingLives;
        GameManager.instance.maxLives = startMaxLives;
        GameManager.instance.numMissiles = startingMissiles;
        GameManager.instance.currentLevel = this;
        GameManager.instance.SpawnPlayer(spawnPoint);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
