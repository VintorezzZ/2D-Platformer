using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //private LevelGenerator _levelGenerator;
    public int levelPlatsCount = 15;
    public int obstacleSpawnChance = 30;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);

        PlayerController.onWin += IncreaseDifficult;
    }

    void Start()
    {
        //_levelGenerator = FindObjectOfType<LevelGenerator>();
    }

    void IncreaseDifficult()
    {
        levelPlatsCount += 3;
        obstacleSpawnChance += 5;
    }

}
