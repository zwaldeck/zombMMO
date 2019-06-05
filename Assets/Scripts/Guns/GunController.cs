using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guns
{
    public class GunController : MonoBehaviour
    {
        [SerializeField] private Gun[] guns = {};

        private Gun currentGun;
        private int currentIndex;

        public Gun CurrentGun => currentGun;

        public void Start()
        {
            if (guns.Length > 0)
            {
                SelectGun(0);
            }
        }

        public void Update()
        {
            SwitchGun();
        }

        private void SelectGun(int index)
        {
            if (currentGun != null)
            {
                currentGun.gameObject.SetActive(false);
                currentGun = null;
            }

            currentIndex = index;
            currentGun = guns[currentIndex];
            currentGun.gameObject.SetActive(true);
        }

        private void SwitchGun()
        {
            if (Math.Abs(Input.GetAxisRaw("Mouse ScrollWheel")) > Mathf.Epsilon)
            {
                var scrollAmount = Input.GetAxisRaw("Mouse ScrollWheel") > 0 ? 1 : -1;
                var index = currentIndex + scrollAmount;

                if (index >= guns.Length)
                {
                    index = 0;
                }
                else if (index < 0)
                {
                    index = guns.Length - 1;
                }

                if (currentIndex != index)
                {
                    SelectGun(index);
                }
            }
        }

       
    }
}