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

        controlPoints.Clear();  // Очищаем для актуальных данных
        foreach (GameObject part in snakeGenerator.snakeParts)
        {
            controlPoints.AddLast(part.transform.position);
        }
    }
}
