using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Guns.Bullets;
using Utils.Pool;


namespace Guns
{
    [RequireComponent(typeof(Animator))]
    public class Gun : MonoBehaviour
    {
        private static readonly int Reloading = Animator.StringToHash("Reloading");

        [Header("Bullet Settings")] [SerializeField]
        private GameObject bulletPrefab;

        [SerializeField] private int bulletPoolSize = 12;

        [Header("Reload Settings")] [SerializeField]
        private AudioClip reloadSound;

        [Header("Gun Settings")] [SerializeField]
        private int maxAmmoInClip = 12;

        [SerializeField] private int amountOfClips = 6;
        [SerializeField] private FireType fireType;
        [SerializeField] private int fireRate = 10; // Bullets per second
        [SerializeField] private float coolDown = 1f; // Only for action bolt

        private bool canFire = true;
        private int currentAmmoInClip;
        private int currentTotalAmmo;
        private int maxTotalAmmo;
        private Coroutine fireCoroutine = null;
        private bool isReloading = false;

        private Camera cam;
        private ObjectPool bulletPool;
        private Animator animator;

        // Public getters
        public bool CanFire => canFire && currentAmmoInClip > 0 && !isReloading;

        public int CurrentAmmoInClip => currentAmmoInClip;

        public int CurrentTotalAmmo => currentTotalAmmo;

        public void Awake()
        {
            cam = Camera.main;
            bulletPool = new ObjectPool(bulletPrefab, bulletPoolSize, Constants.BULLET_POOL_GAMEOBJECT_NAME);
        }

        public void Start()
        {
            animator = GetComponent<Animator>();

            currentAmmoInClip = maxAmmoInClip;
            maxTotalAmmo = maxAmmoInClip * amountOfClips;
            currentTotalAmmo = maxTotalAmmo;
        }

        public void OnEnable()
        {
            animator = GetComponent<Animator>();
            isReloading = false;
            animator.SetBool(Reloading, false);
        }

        public void Update()
        {
            if (CanFire && fireType == FireType.SEMI_AUTOMATIC && Input.GetButtonDown("Fire1"))
            {
                Fire();
            }
            else if (CanFire && fireType == FireType.AUTOMATIC && Input.GetButton("Fire1") && fireCoroutine == null)
            {
                fireCoroutine = StartCoroutine(FireAutomatic());
            }

            if (fireCoroutine != null && Input.GetButtonUp("Fire1"))
            {
                StopFiringAutomatic();
            }


            Reload();
            UpdateCanFire();
        }

        public void OnDisable()
        {
            StopFiringAutomatic();
            isReloading = false;
            animator.SetBool(Reloading, isReloading);
        }

        private void Fire()
        {
            currentAmmoInClip--;
            canFire = false;

            var bullet = bulletPool.GetInstance();
            bullet.transform.position = CalculateBulletStartPoint();
            bullet.transform.rotation = CalculateBulletRotation();

            var bulletComponent = bullet.GetComponent<Bullet>();
            bulletComponent.ObjectPool = bulletPool;
            bulletComponent.Init();
        }

        public void ReloadDone()
        {
            var ammoToReload = maxAmmoInClip - currentAmmoInClip;
            if (currentTotalAmmo >= ammoToReload)
            {
                currentAmmoInClip = maxAmmoInClip;
                currentTotalAmmo -= ammoToReload;
            }
            else
            {
                currentAmmoInClip = currentTotalAmmo;
                currentTotalAmmo = 0;
            }

            isReloading = false;
            animator.SetBool(Reloading, isReloading);
        }

        private IEnumerator FireAutomatic()
        {
            var waitingPeriod = 1f / fireRate;

            while (currentAmmoInClip > 0)
            {
                Fire();

                yield return new WaitForSeconds(waitingPeriod);
            }

            StopFiringAutomatic();
        }

        private void StopFiringAutomatic()
        {
            if (fireCoroutine != null)
            {
                StopCoroutine(fireCoroutine);
                fireCoroutine = null;
                canFire = true;
            }
        }

        private Vector3 CalculateBulletStartPoint()
        {
            foreach (Transform child in transform)
            {
                if (child.CompareTag(Constants.BULLET_START_POSITION_TAG))
                {
                    return child.position;
                }
            }

            // If we get here crash the game
            throw new ApplicationException("We should have a bullet start position");
        }

        private Quaternion CalculateBulletRotation()
        {
            var bodyPosition = transform.position;
            var camDistance = cam.transform.position.y - bodyPosition.y;
            var mouse = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camDistance));
            var angleInRad = Mathf.Atan2(mouse.y - bodyPosition.y, mouse.x - bodyPosition.x);
            var angle = (180 / Mathf.PI) * angleInRad;

            // Fixes a weird issue when its 180 that it does not show anything anymore
            if (Mathf.Abs(angle) > 179.9f && Mathf.Abs(angle) < 180.1f)
            {
                angle = 180.1f;
            }

            return Quaternion.AngleAxis(angle, Vector3.forward);
        }

        private void UpdateCanFire()
        {
            // Semi auto: button was released 
            if (!canFire && currentAmmoInClip > 0 && fireType == FireType.SEMI_AUTOMATIC && Input.GetButtonUp("Fire1"))
            {
                canFire = true;
            }
        }

        private void Reload()
        {
            if (!isReloading && (Input.GetKeyDown(KeyCode.R) || currentAmmoInClip <= 0)
                             && currentTotalAmmo > 0 && currentAmmoInClip < maxAmmoInClip)
            {
                StopFiringAutomatic();
                isReloading = true;
                animator.SetBool(Reloading, isReloading);
                AudioSource.PlayClipAtPoint(reloadSound, cam.transform.position, 1f);
            }
        }
    }
}