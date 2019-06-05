using System.Collections;
using System.Collections.Generic;
using Guns;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AmmoDisplay : MonoBehaviour
    {
        [SerializeField] private Text ammoInClipText;
        [SerializeField] private Text totalAmmoText;
        [SerializeField] private GunController playerGunController;

        public void Start()
        {
        }

        public void Update()
        {
            var gun = playerGunController.CurrentGun;
            ammoInClipText.text = gun.CurrentAmmoInClip.ToString();
            totalAmmoText.text = gun.CurrentTotalAmmo.ToString();
        }
    }
}