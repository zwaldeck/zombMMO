using UnityEngine;

namespace Utils
{
    public static class TransformUtils
    {
        public static float LookAt(Vector2 currentPosition, Vector2 lookAtPosition)
        {
            var angleInRad = Mathf.Atan2(lookAtPosition.y - currentPosition.y, lookAtPosition.x - currentPosition.x);
            return angleInRad * Mathf.Rad2Deg;
        }
    }
}