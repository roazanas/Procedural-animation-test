using UnityEngine;

public class DistanceConstraint : MonoBehaviour
{
    public Transform target;
    private Vector2 targetPos;
    public float maxDistance;

    void LateUpdate()
    {
        if (target == null)
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        else
            targetPos = target.position;

        float currentDistance = Vector2.Distance(transform.position, targetPos);

        if (currentDistance > maxDistance)
        {
            Vector2 direction = (targetPos - (Vector2)transform.position).normalized;

            transform.position = targetPos - direction * maxDistance;
        }
    }
}
