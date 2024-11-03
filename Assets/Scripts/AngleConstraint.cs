using System.Collections.Generic;
using UnityEngine;

public class AngleConstraint : MonoBehaviour
{
    public Transform target;
    public float maxAngle;

    public float constrainedAngle;

    void LateUpdate()
    {
        if (target == null) return;

        Vector2 directionToTarget = (transform.position - target.position).normalized;

        DistanceConstraint targetConstraint = target.GetComponent<DistanceConstraint>();
        if (targetConstraint == null || targetConstraint.target == null) return;

        Vector2 targetDirection = (target.position - targetConstraint.target.position).normalized;

        float angleDifference = Vector2.SignedAngle(targetDirection, directionToTarget);

        if (Mathf.Abs(angleDifference) > maxAngle)
        {
            constrainedAngle = maxAngle * Mathf.Sign(angleDifference);
            Vector2 constrainedDirection = Quaternion.Euler(0, 0, constrainedAngle) * targetDirection;

            transform.position = (Vector2)target.position + constrainedDirection * targetConstraint.maxDistance;
        }
    }
}
