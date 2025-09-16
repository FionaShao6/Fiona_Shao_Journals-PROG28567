using JetBrains.Annotations;
using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

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
    public float warpRatio = 0.5f;
    public float moveSpeed = 1f;
    public Transform asteroidParent;
    public List<Transform> asteroids = new List<Transform>();

    public Vector3 velocity = new Vector3(0, 0, 0);

    public float maxSpeed= 10f;
    public float accelerationTime = 2f;
    public float acceleration;

    public float declerationTime;

    void Start()
    {
        acceleration = maxSpeed / accelerationTime;

        if (asteroidParent != null)
        {
         
            foreach (Transform child in asteroidParent)
            {
                asteroids.Add(child);
            }
        }
       // transform.position += new Vector3(1, 0, 0);
    }
    void Update()
    {
       
        DetectAsteroids(10f, asteroids);

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
        if (Input.GetKeyDown(KeyCode.L))
        {
            WarpPlayer (enemyTransform, warpRatio);
        }
        
         PlayerMovement ();
        
        


    }

    public void SpawnBombAtOffset(Vector3 inOffset)
    {

        Vector2 spawnPosition = transform.position + inOffset;
        Instantiate(bombPrefab, spawnPosition, Quaternion.identity, bombsTransform);

    }
    public void SpawnBombTrail(float inBombSpacing, int inNumberOfBombs)
    {
        Vector2 backwardDirection = -transform.up;
        for (int i = 0; i < inNumberOfBombs; i++)
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
        cornerDirection[3] = new Vector2(1, -1).normalized;

        int randomIndex = Random.Range(0, cornerDirection.Length);

        Vector2 bombOffset = cornerDirection[randomIndex] * inDistance;
        SpawnBombAtOffset(bombOffset);
    }

    public void WarpPlayer(Transform target, float ratio)
    {
        Vector2 startPoint = transform.position;
        Vector2 endPoint = target.position;
        Vector2 newPosition = Vector2.Lerp(startPoint, endPoint, ratio);
        transform.position = new Vector2(newPosition.x,newPosition.y);
    }

    public void DetectAsteroids(float inMaxRange, List<Transform> inAsteroids)
    {
        if (inAsteroids == null) return;

        foreach(Transform asteroid in inAsteroids)
        {
            if (asteroid == null) continue;
            float distance = Vector3.Distance(transform.position, asteroid.position);
            if(distance <= inMaxRange)
            {
                Vector3 direction = (asteroid.position - transform.position).normalized;
                Vector3 lineEnd = transform.position + direction * 2.5f;
                Debug.DrawLine(transform.position, lineEnd, Color.yellow);

            }
        }
    }
    
    public void PlayerMovement( )
    {
        Vector2 direction = Vector2.zero;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction+= Vector2.left;
        }
       
        if (Input.GetKey(KeyCode.RightArrow))
        {
            direction += Vector2.right;
        }
      
        if (Input.GetKey(KeyCode.UpArrow))
        {
            direction += Vector2.up;
        }
     
        if (Input.GetKey(KeyCode.DownArrow))
        {
            direction += Vector2.down;
        }
     
        velocity += (Vector3)direction.normalized * acceleration*Time.deltaTime;

        transform.position += velocity * Time.deltaTime;

        


    }


    

} 
     
    


