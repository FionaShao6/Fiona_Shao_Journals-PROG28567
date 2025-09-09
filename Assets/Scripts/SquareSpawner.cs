using log4net.Util;
using UnityEngine;

public class SquareSpawner : MonoBehaviour
{
    private void Start()
    {
    }

    void Update()
    {
        MathFunctions.DrawSquare(transform.position, 1f, Color.white, 1f);
        
    }
}
