using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<Transform> asteroidTransforms;
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public Transform bombsTransform;
    public Vector2 bombOffset;
    public int numberOfBombs;
    public float bombSpacing;

    void Update()
    {

        float speed = 0.5f;
        Vector2 targetPosition = enemyTransform.position;
        Vector2 startPosition = transform.position;
        Vector2 directionToMove = targetPosition - startPosition;

        if (Input.GetKeyDown(KeyCode.M))
        {
            transform.position += (Vector3)directionToMove.normalized * speed;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            SpawnBombAtOffset(bombOffset);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnBombTrail(bombSpacing, numberOfBombs);
        }
    }

    public void SpawnBombAtOffset(Vector3 inOffset)
    {
        
        Vector2 spawnPosition = transform.position + inOffset;
        Instantiate(bombPrefab,spawnPosition,Quaternion.identity,bombsTransform);

    }
    public void SpawnBombTrail(float inBombSpacing, int inNumberOfBombs)
    {
        Vector2 backwardDirection = -transform.up;
        for(int i=0; i < inNumberOfBombs; i++)
        {
            float distance = (i + 1) * inBombSpacing;
            Vector2 bombOffset = backwardDirection * distance;
            SpawnBombAtOffset(bombOffset);
        }
    }

}
