using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{
    public List<Transform> starTransforms;

    public float drawingTime;
    public float lineWidth;
    public float drawProgress = 0f;
    public Color lineColor = Color.white;
    public LineRenderer lineRenderer;
    int currentLineIndex = 0;


    // Update is called once per frame
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null )
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
         
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
           
            lineRenderer.startColor = lineColor;
            lineRenderer.endColor = lineColor;

            lineRenderer.positionCount = 2;

        }
    }
    void Update()
    {
        DrawConstellation();
    }
    public void DrawConstellation()
    {
        //Determine the start and end indexes of the current line segment
        int startIndex = currentLineIndex;
        int endIndex = currentLineIndex + 1;
        //If the end point is the last star, the end point index is reset to 0
        if (endIndex >= starTransforms.Count)
        {
            endIndex = 0;
        }
        //If the star list is empty or the number of stars is less than 2,
        //no drawing 
        if (starTransforms == null || starTransforms.Count <2)
        {
            return;
        }

        drawProgress += Time.deltaTime / drawingTime;


        
        Vector3 startPos = starTransforms[startIndex].position;
        Vector3 endPos = starTransforms[endIndex].position;

        //Use Lerp to achieve a gradual effect from the starting point to the end point
        Vector3 currentEndPos = Vector3.Lerp(startPos, endPos, drawProgress);
        //Update the two vertex positions of LineRenderer
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, currentEndPos);
        //Circular logic
        if (drawProgress >= 1f) { 
        drawProgress = 0f;
            currentLineIndex++;
        }
        if(currentLineIndex >= starTransforms.Count - 1)
        {
            currentLineIndex = 0;
        }




    }
}
