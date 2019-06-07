using System;
using UnityEngine;

namespace Debug
{
    [RequireComponent(typeof(LineRenderer))]
    public class DebugRadius : MonoBehaviour
    {
        [SerializeField] private int vertexCount = 40;
        [SerializeField] private float lineWidth = 0.2f;
        [SerializeField] private float radius = 1.0f;

        private LineRenderer lineRenderer;

        public void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();

            SetupCircle();
        }

        public void Update()
        {
            var deltaTheta = (2f * Mathf.PI) / vertexCount;
            var theta = 0f;

            for (var i = 0; i < lineRenderer.positionCount; i++)
            {
                var pos = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f) + transform.position;
                lineRenderer.SetPosition(i, pos);
                theta += deltaTheta;
            }
        }

        private void SetupCircle()
        {
            lineRenderer.widthMultiplier = lineWidth;
            lineRenderer.positionCount = vertexCount;
        }
    }
}