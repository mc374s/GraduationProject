using UnityEngine;

static class Utility
{
    // Return hittedBorder's direction
    public static Vector3 BorderCheck(Transform target, Vector3 leftTop, Vector3 rightButtom)
    {
        Vector3 newPosition = target.position;
        Vector3 direction = Vector3.zero;
        if (target.position.x < leftTop.x)
        {
            newPosition.x = leftTop.x;
            direction = Vector3.left;
        }
        else if (target.position.x > rightButtom.x)
        {
            newPosition.x = rightButtom.x;
            direction = Vector3.right;
        }
        if (target.position.y > leftTop.y)
        {
            newPosition.y = leftTop.y;
            direction = Vector3.up;
        }
        else if (target.position.y < rightButtom.y)
        {
            newPosition.y = rightButtom.y;
            direction = Vector3.down;
        }
        target.position = newPosition;
        return direction;
    }
}
