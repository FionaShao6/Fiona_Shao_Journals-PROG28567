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
    public float cornerBombDistance = 1f;
    

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
        if (Input.GetKeyDown(KeyCode.C))
        {
            SpawnBombOnRandomCorner(cornerBombDistance);
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
    public void SpawnBombOnRandomCorner(float inDistance)
    {
        Vector2[] cornerDirection = new Vector2[4];
        cornerDirection[0] = new Vector2(1, 1).normalized;
        cornerDirection[1] = new Vector2(-1, 1).normalized;
        cornerDirection[2] = new Vector2(-1, -1).normalized;
        cornerDirection[3] = new Vector2(1,- 1).normalized;

        int randomIndex = Random.Range(0, cornerDirection.Length);

        Vector2 bombOffset = cornerDirection[randomIndex] * inDistance;
        SpawnBombAtOffset(bombOffset);
    }

}
