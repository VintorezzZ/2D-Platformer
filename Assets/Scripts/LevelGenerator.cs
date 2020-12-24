using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private List<Transform> platformsList;
    [SerializeField] private Transform levelContainer;
    [SerializeField] private Transform finishPlatform;
    [SerializeField] private Transform obstacle;
    [SerializeField] private float yOffset = 2f;
    
    private Transform lastPlatformTransform;
    private Transform randomPlatform;
    private Transform platformTransform;

    private Vector3 lastEndPoint;
    private float[] yOffsetsArray;

    private GameManager _gameManager;
    
    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        
        yOffsetsArray = new[] {0, yOffset, -yOffset};
        
        lastEndPoint = levelContainer.Find("EndPoint").position;
        for (int i = 0; i < _gameManager.levelPlatsCount; i++)
        {
            SpawnPlatform();
            switch (i)
            {
                case 3: yOffset += 0.15f;
                    yOffsetsArray = new[] {0, yOffset, -yOffset}; 
                    break;
                case 7: yOffset += 0.20f;
                    yOffsetsArray = new[] {0, yOffset, -yOffset};  
                    break;
                case 10: yOffset += 0.25f; 
                    yOffsetsArray = new[] {0, yOffset, -yOffset}; 
                    break;
            }
            
        }

        SpawnPlatform(finishPlatform);
    }
    
    private void SpawnPlatform()
    {
        lastPlatformTransform = SpawnPlatform(lastEndPoint);
        SetNewLastEndPoint(yOffset);
    }
    private void SpawnPlatform(Transform finishPlatform)
    {
        lastPlatformTransform = SpawnPlatform(lastEndPoint, finishPlatform);
    }

    private void SetNewLastEndPoint(float yNewOffset)
    {
        float randomY = yOffsetsArray[Random.Range(0, yOffsetsArray.Length)];
        Vector3 yOffset = new Vector3(0, randomY, 0);
        lastEndPoint = lastPlatformTransform.Find("EndPoint").position + yOffset;
    }

    Transform SpawnPlatform(Vector2 spawnPosition)
    {
        randomPlatform = platformsList[Random.Range(0, platformsList.Count)];
        platformTransform = Instantiate(randomPlatform, spawnPosition, Quaternion.identity);
        if (randomPlatform == platformsList[0])
        {
            if (Random.Range(0, 100) < _gameManager.obstacleSpawnChance)
            {
               Transform obst = Instantiate(obstacle, platformTransform.GetChild(0).position, quaternion.identity);
               obst.SetParent(levelContainer);
            }
        }

        platformTransform.SetParent(levelContainer);
        return platformTransform;
    }
    Transform SpawnPlatform(Vector2 spawnPosition, Transform finishPlatform)
    {
        platformTransform = Instantiate(finishPlatform, spawnPosition, Quaternion.identity);
        platformTransform.SetParent(levelContainer);
        return platformTransform;
    }
}
