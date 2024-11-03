using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineHandler : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private SnakeOutline snakeOutline;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        snakeOutline = GetComponent<SnakeOutline>();
    }

    private void Update()
    {
        var controlPoints = snakeOutline.controlPoints;

        if (controlPoints == null || controlPoints.Count < 2) return;

        lineRenderer.positionCount = controlPoints.Count;

        int index = 0;
        foreach (var point in controlPoints)
        {
            lineRenderer.SetPosition(index++, point);
        }
    }
}
