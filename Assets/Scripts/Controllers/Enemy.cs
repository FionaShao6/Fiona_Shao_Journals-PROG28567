using UnityEngine;
using System.Collections;
using PlasticPipe.PlasticProtocol.Messages;

public class Enemy : MonoBehaviour
{
    public Transform playerTransform;
    public float yOffset;
    public float xOffset;
    public float moveSpeed = 3f;
    
    private void Update()
    {

        EnemyMovement();
    }
    public Vector2 CalculateTargetPosition()
    {
        float targetX = playerTransform.position.x + xOffset;
        float targetY = playerTransform.position.y + yOffset;
        return new Vector2(targetX, targetY);
    }
    public void MoveToTarget(Vector3 target)
    {
        // Calculate the direction and distance from the enemy's current position to the target position
        Vector3 directionToTarget = target - transform.position;

        Vector3 moveStep = directionToTarget.normalized * moveSpeed * Time.deltaTime;
  
        transform.position += moveStep;
    }
    public void EnemyMovement()
    {
        if (playerTransform == null)
        {
            return;
        }
        Vector2 targetPosition = CalculateTargetPosition();
        MoveToTarget(targetPosition);
    }


   
}
