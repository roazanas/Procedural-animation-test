using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CatmullRomSpline : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private LinkedList<Vector2> controlPoints;
    private List<Vector2> splinePoints;
    private SnakeOutline snakeOutline;
    private SnakeGenerator snakeGenerator;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        snakeOutline = GetComponent<SnakeOutline>();
        snakeGenerator = GetComponent<SnakeGenerator>();
    }

    private void Update()
    {
        controlPoints = snakeOutline.controlPoints;

        if (controlPoints == null || controlPoints.Count < 2) return;

        splinePoints = GetCatmullRomSplinePoints(controlPoints, 10);
        SetLineWidthBySegments();
        DrawSpline(splinePoints);
    }

    private void SetLineWidthBySegments()
    {
        if (snakeGenerator == null || snakeGenerator.bodyShape.Length == 0) return;

        // Создаем кривую ширины, где каждое ключевое значение представляет ширину сегмента
        AnimationCurve widthCurve = new AnimationCurve();

        for (int i = 0; i < snakeGenerator.bodyShape.Length; i++)
        {
            // Нормализуем позицию от 0 до 1 для ключевой точки
            float keyPosition = (float)i / (snakeGenerator.bodyShape.Length - 1);
            // Ширина сегмента относительно размера тела
            float width = snakeGenerator.bodyShape[i] / 90f + 0.01f;

            // Добавляем ключевую точку в кривую
            widthCurve.AddKey(keyPosition, width);
        }

        // Назначаем кривую ширины линии
        lineRenderer.widthCurve = widthCurve;
    }

    public List<Vector2> GetCatmullRomSplinePoints(LinkedList<Vector2> controlPointsList, int segmentsPerCurve)
    {
        Vector2[] controlPoints = new Vector2[controlPointsList.Count];

        for (int i = 0; i < controlPointsList.Count; i++)
        {
            controlPoints[i] = controlPointsList.ElementAt(i);
        }

        List<Vector2> splinePoints = new List<Vector2>();

        for (int i = 0; i < controlPoints.Length - 1; i++)
        {
            Vector2 p0 = (i == 0) ? controlPoints[i] : controlPoints[i - 1];
            Vector2 p1 = controlPoints[i];
            Vector2 p2 = controlPoints[i + 1];
            Vector2 p3 = (i + 2 < controlPoints.Length) ? controlPoints[i + 2] : controlPoints[i + 1];

            for (int j = 0; j < segmentsPerCurve; j++)
            {
                float t = (float)j / segmentsPerCurve;
                Vector2 point = CalculateCatmullRomPoint(t, p0, p1, p2, p3);
                splinePoints.Add(point);
            }
        }

        return splinePoints;
    }

    private Vector2 CalculateCatmullRomPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        return 0.5f * (
            (2f * p1) +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2 +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t3
        );
    }

    private void DrawSpline(List<Vector2> splinePoints)
    {
        lineRenderer.positionCount = splinePoints.Count;

        for (int i = 0; i < splinePoints.Count; i++)
        {
            lineRenderer.SetPosition(i, splinePoints[i]);
        }
    }
}
