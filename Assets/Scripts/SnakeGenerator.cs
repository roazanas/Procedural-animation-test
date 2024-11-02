using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeGenerator : MonoBehaviour
{
    public GameObject snakePartPrefab;
    public int[] bodyShape;
    public int numberOfPartsToGenerate;
    public float distanceBetweenParts;
    public float allowedAngleBetweenParts;

    int BodyWidth(int i)
    {
        switch (i)
        {
            case 0:
                return 76;
            case 1:
                return 80;
            default:
                return 64 - i;
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

            if (i == 0)
                distanceConstraint.target = null;
            else
                distanceConstraint.target = previousPart;

            angleConstraint.target = distanceConstraint.target;
            previousPart = newPart.transform;
        }
    }
}