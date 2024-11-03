using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeOutline : MonoBehaviour
{
    private SnakeGenerator snakeGenerator;
    public LinkedList<Vector2> controlPoints = new LinkedList<Vector2>();

    private void Start()
    {
        snakeGenerator = GetComponent<SnakeGenerator>();
    }

    private void Update()
    {
        if (snakeGenerator == null) return;

        controlPoints.Clear();

        // Правая сторона тела (идем от головы к хвосту)
        for (int i = 0; i < snakeGenerator.snakeParts.Count; i++)
        {
            Vector2 rightPoint = GetOffsetPosition(i, Mathf.PI / 2, 0);
            controlPoints.AddLast(rightPoint);
        }

        // Хвост
        controlPoints.AddLast(GetOffsetPosition(snakeGenerator.snakeParts.Count - 1, 3 * Mathf.PI / 4, 0));
        controlPoints.AddLast(GetOffsetPosition(snakeGenerator.snakeParts.Count - 1, Mathf.PI, 0));
        controlPoints.AddLast(GetOffsetPosition(snakeGenerator.snakeParts.Count - 1, 5 * Mathf.PI / 4, 0));

        // Левая сторона тела (идем от хвоста к голове)
        for (int i = snakeGenerator.snakeParts.Count - 1; i >= 0; i--)
        {
            Vector2 leftPoint = GetOffsetPosition(i, -Mathf.PI / 2, 0);
            controlPoints.AddLast(leftPoint);
        }

        // Голова
        controlPoints.AddLast(GetOffsetPosition(0, -Mathf.PI / 3, 0));
        controlPoints.AddLast(GetOffsetPosition(0, -Mathf.PI / 4, 0));
        controlPoints.AddLast(GetOffsetPosition(0, -Mathf.PI / 6, 0));
        controlPoints.AddLast(GetOffsetPosition(0, 0, -0.03f));
        controlPoints.AddLast(GetOffsetPosition(0, Mathf.PI / 6, 0));
        controlPoints.AddLast(GetOffsetPosition(0, Mathf.PI / 4, 0));
        controlPoints.AddLast(GetOffsetPosition(0, Mathf.PI / 3, 0));
    }

    private Vector2 GetOffsetPosition(int index, float angleOffset, float lengthOffset)
    {
        if (index >= snakeGenerator.snakeParts.Count) return Vector2.zero;
        GameObject part = snakeGenerator.snakeParts[index];
        GameObject nextPart = index > 0 ? snakeGenerator.snakeParts[index - 1] : null;

        Vector2 direction;
        if (nextPart != null)
        {
            direction = ((Vector2)nextPart.transform.position - (Vector2)part.transform.position).normalized;
        }
        else
        {
            direction = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)part.transform.position).normalized;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) + angleOffset;

        float scaledWidth = part.transform.localScale.x / 2f;
        float finalWidth = scaledWidth + lengthOffset;

        return (Vector2)part.transform.position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * finalWidth;
    }
}
