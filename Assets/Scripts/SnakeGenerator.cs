using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SnakeGenerator : MonoBehaviour
{
    public GameObject snakePartPrefab;
    public int[] bodyShape;
    public int numberOfPartsToGenerate;
    public float distanceBetweenParts;
    public float allowedAngleBetweenParts;

    public List<GameObject> snakeParts = new List<GameObject>();

    public int BodyWidth(int i)
    {
        switch (i)
        {
            case 0:
                return 96;
            case 1:
                return 98;
            case 2:
            case 3:
                return 102;
            default:
                float rnd = Random.Range(1, 6);
                return (int)(((98 - i / 1.25f) / rnd) * rnd);
        }
    }

    void Awake()
    {
        if (bodyShape.Length == 0)
        {
            bodyShape = new int[numberOfPartsToGenerate];
            for (int i = 0; i < bodyShape.Length; i++)
            {
                bodyShape[i] = BodyWidth(i);
            }
        }

        Transform previousPart = null;
        for (int i = 0; i < bodyShape.Length; i++)
        {
            GameObject newPart = Instantiate(snakePartPrefab, transform.position, Quaternion.identity);
            DistanceConstraint distanceConstraint = newPart.GetComponent<DistanceConstraint>();
            AngleConstraint angleConstraint = newPart.GetComponent<AngleConstraint>();

            distanceConstraint.maxDistance = distanceBetweenParts;
            angleConstraint.maxAngle = allowedAngleBetweenParts;
            newPart.transform.localScale = Vector3.one * bodyShape[i] / 100f;
            newPart.transform.SetParent(transform);

            SpriteRenderer sprite = newPart.GetComponent<SpriteRenderer>();
            sprite.color = new Color(sprite.color.r + Random.value / 170f,
                                     sprite.color.g + Random.value / 100f,
                                     sprite.color.b + Random.value / 170f,
                                     sprite.color.a);

            if (i == 0)
                distanceConstraint.target = null;
            else
                distanceConstraint.target = previousPart;

            angleConstraint.target = distanceConstraint.target;
            previousPart = newPart.transform;

            snakeParts.Add(newPart);
        }
    }
}
