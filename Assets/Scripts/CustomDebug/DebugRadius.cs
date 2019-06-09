using System;
using UnityEngine;

namespace CustomDebug
{
    public class DebugRadius : MonoBehaviour
    {
        [SerializeField] private int vertexCount = 40;
        [SerializeField] private float radius = 1.0f;
        [SerializeField] private Color color = Color.white;

        public void Start()
        {
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = color;
            var deltaTheta = (2f * Mathf.PI) / vertexCount;
            var theta = 0f;
 
            var oldPos = transform.position;
            for (var i=0; i<vertexCount + 1; i++)
            {
                var pos = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f);

                if (i != 0)
                {
                    Gizmos.DrawLine(oldPos, transform.position + pos);
                }

                oldPos = transform.position + pos;
 
                theta += deltaTheta;
            }
            Gizmos.color = Color.white;
        }
    }
}