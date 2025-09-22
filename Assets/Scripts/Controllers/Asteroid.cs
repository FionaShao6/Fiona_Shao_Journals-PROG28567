using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float moveSpeed;
    public float arrivalDistance;
    public float maxFloatDistance;
    private Vector3 currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        GenerateNewTarget();
    }

    // Update is called once per frame
    void Update()
    {
        AsteroidMovement();
    }
    public void GenerateNewTarget()
    {
        Vector3 randomDirection = new Vector3(
            Random.Range(-1f, 1f),Random.Range(-1f, 1f), 0).normalized;
        currentTarget = transform.position + randomDirection * maxFloatDistance;
    }
    public void AsteroidMovement()
    {
        
        Vector3 currentPosition = transform.position;
        Vector2 directionToTarget = currentTarget - currentPosition;

        float xDistence = directionToTarget.x;
        float yDistence = directionToTarget.y;
        float distanceToTarget = Mathf.Sqrt(xDistence * xDistence + yDistence * yDistence);

        if (distanceToTarget < arrivalDistance)
        {
            GenerateNewTarget();
            return;
        }

        
        Vector2 moveStep = directionToTarget.normalized * moveSpeed * Time.deltaTime;
        transform.position += new Vector3(moveStep.x, moveStep.y, 0);

    }
}
