using System;
using System.Collections;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace AI
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class RandomMovement : MonoBehaviour
    {
        [SerializeField] private float randomMoveRadius = 1f;
        [SerializeField] [Range(0f, 100f)] private float randomChanceToWait = 50f;
        [SerializeField] private float minWaitTime = 1f;
        [SerializeField] private float maxWaitTime = 3f;
        [SerializeField] [Range(0.1f, 20)] private float movementSpeed = 5f;
        [SerializeField] private float turnSpeed = 5f;

        // State
        private Vector2 destination;
        private float lookAngle;
        private bool waiting = false;
        

        // Components
        private Rigidbody2D body;

        public void Start()
        {
            body = GetComponent<Rigidbody2D>();
            SetNewDestination();
        }

        public void Update()
        {
            if (!waiting && Vector2.Distance(transform.position, destination) <= Mathf.Epsilon)
            {
                StartCoroutine(SetNewDestination());
            }

            
        }

        public void FixedUpdate()
        {
            if (!waiting)
            {
                // Movement
                var newPosition = Vector2.MoveTowards(transform.position, destination, Time.deltaTime * movementSpeed);
                body.MovePosition(newPosition);
                
                // Looking
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler (0, 0, lookAngle), turnSpeed * Time.deltaTime);
            }
        }

        public void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Tilemap Collision"))
            {
                StartCoroutine(SetNewDestination(false));
            }
        }

        private IEnumerator SetNewDestination(bool useWait = true)
        {
            if (useWait && ShouldWeWait())
            {
                waiting = true;
                yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
                waiting = false;
            }
            
            var x = Random.Range(-randomMoveRadius, randomMoveRadius);
            var y = Random.Range(-randomMoveRadius, randomMoveRadius);
            destination = transform.position + new Vector3(x, y, 0f);
            lookAngle = TransformUtils.LookAt(transform.position, destination);
        }

        private bool ShouldWeWait()
        {
            return Random.Range(1, 100) <= randomChanceToWait;
        }

        public void OnDrawGizmos()
        {
            /*if (destination != null)
            {
                Gizmos.DrawCube(destination, new Vector3(0.2f, 0.2f, 0.2f));
            }*/
        }
    }
}