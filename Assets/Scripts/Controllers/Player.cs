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
            SpawnBombAtOffset(new Vector3(0, 1));
        }

    }

    void SpawnBombAtOffset(Vector3 inOffset)
    {
        //Vector2 bsPos = transform.position + inOffset;
        Vector2 spawnposition = transform.position + inOffset;
        Instantiate(bombPrefab,inOffset,Quaternion.identity);

    }

}
