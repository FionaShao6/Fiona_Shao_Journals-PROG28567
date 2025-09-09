using UnityEngine;

public class MathFunctions:MonoBehaviour
{
    private void Start()
    {
        Vector2 position = transform.position;
        float magnitude = GetMagnitude(position);

        //
    }

    public float healthPoint = 0f;
    
        
    

    public static float GetMagnitude(Vector2 position)
    {
        return Mathf.Sqrt(position.x * position.x + position.y * position.y);
    }

    public static void DrawSquare(Vector2 position, float size, Color color,float duration)
    {
        Vector2 topLeftPoint = position + Vector2.up * size / 2f + Vector2.left * size / 2f;
        Vector2 topRightPoint = position + Vector2.up * size / 2f + Vector2.right * size / 2f;
        Vector2 bottomLeftPoint = position + Vector2.down * size / 2f + Vector2.left * size / 2f;
        Vector2 bottomRightPoint = position + Vector2.down * size / 2f + Vector2.right * size / 2f;

        Debug.DrawLine(topLeftPoint, topRightPoint, color, duration);
        Debug.DrawLine(topRightPoint, bottomRightPoint, color, duration);
        Debug.DrawLine(bottomRightPoint, bottomLeftPoint, color, duration);
        Debug.DrawLine(bottomLeftPoint, topLeftPoint, color, duration);


    }




}
