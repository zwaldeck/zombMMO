using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils.Pool;

namespace Guns.Bullets
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float damage = 1f;
        [SerializeField] private float moveSpeed = 20f;

        private Rigidbody2D body;
        private Vector2 moveDirection;
        
        public ObjectPool ObjectPool { get; set; }

        public void Start()
        {
            body = GetComponent<Rigidbody2D>();
        }

        public void FixedUpdate()
        {
            var movement = new Vector2(transform.position.x, transform.position.y) + Time.deltaTime * moveSpeed * moveDirection;
            body.MovePosition(movement);
        }

        public void OnBecameInvisible()
        {
            ObjectPool.ReturnInstance(gameObject);
        }

        public void Init()
        {
            var moveDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            moveDir.z = 0;
            moveDir.Normalize();

            moveDirection =  moveDir;
        }
    }
}