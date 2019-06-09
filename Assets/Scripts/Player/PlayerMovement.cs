using System.Collections;
using System.Collections.Generic;
using Guns;
using UnityEngine;
using Utils;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] [Range(0.1f, 20)] private float movementSpeed = 5f;

        private Rigidbody2D body;
        private Camera cam;

        public void Start()
        {
            cam = Camera.main;
            body = GetComponent<Rigidbody2D>();
        }

        public void FixedUpdate()
        {
            Move();
            LookAtMouse();
        }

        private void Move()
        {
            var deltaX = Input.GetAxisRaw("Horizontal");
            var deltaY = Input.GetAxisRaw("Vertical");
            var velocity = movementSpeed * Time.deltaTime * new Vector2(deltaX, deltaY).normalized;

            body.MovePosition(body.position + velocity);
        }

        private void LookAtMouse()
        {
            var bodyPosition = body.position;
            var camDistance = cam.transform.position.y - bodyPosition.y;
            var mouse = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camDistance));

            var angle = TransformUtils.LookAt(transform.position, mouse);

            // Fixes a weird issue when its 180 that it does not show anything anymore
            if (Mathf.Abs(angle) > 179.9f && Mathf.Abs(angle) < 180.1f) 
            {
                angle = 180.1f;
            }
            
            body.rotation = angle;
        }
    }
}