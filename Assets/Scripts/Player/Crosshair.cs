using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Crosshair : MonoBehaviour
    {
        [SerializeField] private bool hideMouse = true;

        private Camera cam;

        public void Start()
        {
            cam = Camera.main;

            Cursor.visible = !hideMouse;
        }

        public void FixedUpdate()
        {
            var mouse = cam.ScreenToWorldPoint(Input.mousePosition);
            mouse.z = 0;

            transform.position = mouse;
        }
        
    }
}