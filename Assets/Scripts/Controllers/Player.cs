using JetBrains.Annotations;
using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static Codice.Client.Common.EventTracking.TrackFeatureUseEvent.Features.DesktopGUI.Filters;

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

    public float deceleration;
    public float decelerationTime = 2f;
    bool hasInput = false;

    float circlePoint;
    /// <summary>
    /// float radarRadius = 2f;
    /// </summary>

    public GameObject explosionPrefab;
    public GameObject fragmentPrefab;
    public int fragmentCount = 6;
    public float minFragmentSpeed = 3f;
    public float maxFragmentSpeed = 6f;

    void Start()
    {
        acceleration = maxSpeed / accelerationTime;
        deceleration = maxSpeed / decelerationTime;

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

        //When the player presses the arrow keys: the player gradually accelerates to the maximum speed.
        //When the player releases the arrow keys: the player gradually decelerates to a stop.
        if (Input.GetKey(KeyCode.LeftArrow) ||
           Input.GetKey(KeyCode.RightArrow) ||
           Input.GetKey(KeyCode.UpArrow) ||
           Input.GetKey(KeyCode.DownArrow))
        {
            hasInput = true;
        }
        if (hasInput)
        {
            Vector3 targetVelocity = (Vector3)direction * maxSpeed;

            // the speed that can be increased per frame
            float speedStep = acceleration * Time.deltaTime;

            //// Process speed in each direction to ensure that the target speed is not exceeded
           
            // X direction
            if (velocity.x < targetVelocity.x)
            {
                velocity.x += speedStep;
                // Prevent exceeding target speed
                if (velocity.x > targetVelocity.x)
                    velocity.x = targetVelocity.x;
            }
            else if (velocity.x > targetVelocity.x)
            {
                velocity.x -= speedStep;
                // Prevent falling below target speed
                if (velocity.x < targetVelocity.x)
                    velocity.x = targetVelocity.x;
            }

            // Y derection
            if (velocity.y < targetVelocity.y)
            {
                velocity.y += speedStep;
                if (velocity.y > targetVelocity.y)
                    velocity.y = targetVelocity.y;
            }
            else if (velocity.y > targetVelocity.y)
            {
                velocity.y -= speedStep;
                if (velocity.y < targetVelocity.y)
                    velocity.y = targetVelocity.y;
            }
        }
        ///// Deceleration control
        else
        {
            // calculate the deceleration amount for this frame
            float slowDownStep = deceleration * Time.deltaTime;

            // X derection
            if (velocity.x > 0)
            {
                velocity.x -= slowDownStep;
                if (velocity.x < 0) velocity.x = 0; // prevent reverse movement
            }
            else if (velocity.x < 0)
            {
                velocity.x += slowDownStep;
                if (velocity.x > 0) velocity.x = 0;
            }

            // Y derection 
            if (velocity.y > 0)
            {
                velocity.y -= slowDownStep;
                if (velocity.y < 0) velocity.y = 0;
            }
            else if (velocity.y < 0)
            {
                velocity.y += slowDownStep;
                if (velocity.y > 0) velocity.y = 0;
            }
        }

    

    transform.position += velocity * Time.deltaTime;

        


    }

    public void Radar()
    {

        Vector2 centerPoint = transform.position;
        float angleStep = 360f / circlePoint;

        for(int i = 0; circlePoint>=i; i++)
        {
            
        }


    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Planet")
        {
            Explode();
        }
        Debug.Log("touched：" + collision.tag);

       
    }

    public void Explode()
    {
        PlayExplodeEffect();
        Invoke("SpawnFragments",0.1f);
        Debug.Log("Player start destroy");
        Destroy(gameObject,0.2f);
    }
    public void PlayExplodeEffect()
    {
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 0.5f);
        }
    }

    public void SpawnFragments()
    {
        for (int i = 0; i < fragmentCount; i++)
        {
            GameObject fragment = Instantiate(fragmentPrefab, transform.position, Quaternion.identity);
            //Add random direction to fragments
            float randomX = Random.Range(-8f, 8f);
            float randomY = Random.Range(-8f, 8f);
            Vector2 randomDirection = new Vector2(randomX, randomY).normalized;

            Rigidbody2D fragmentRb = fragment.GetComponent<Rigidbody2D>();
            if (fragmentRb != null)
            {

                float randomSpeed = Random.Range(minFragmentSpeed, maxFragmentSpeed);

                fragmentRb.linearVelocity = randomDirection * randomSpeed;
                fragmentRb.angularVelocity = Random.Range(-360f, 360f);
            }

            Destroy(fragment, 5f);
        }
    }

}     