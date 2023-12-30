 using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;

     // shooting
     public bool isShooting, readyToShoot;
       bool allowReset = true;
     public float shootingDelay = 2f;

     // burst
     [Header("Set this to 1 or more to prevent division by 0")]
     public int bulletsPerBurst = 4;
     public int burstBulletsLeft;

     // spread
     public float spreadIntensity;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 500f;
    public float bulletLifetime = 3f;

    public GameObject muzzleEffect;
    internal Animator animator; // internal - other scripts can access it but not from the inspector

    // loading
    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;

    [Header ("Set this for Auto Reload")]
    public bool autoReload;
    
    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

    public enum WeaponModel
    {
        Pistol,
        Rifle
    }
    
    public WeaponModel currentWeaponModel;
    

     public enum ShootingMode
     {
        Single,
        Burst,
        Auto
     }
    
    public ShootingMode currentShootingMode;
     
        private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();

        bulletsLeft = magazineSize;
    }

    void Update()
    {
        if (isActiveWeapon)
        {
            //GetComponent<Outline>().enabled = false;
            
            if (bulletsLeft == 0 && isShooting)
            {
                //SoundManager.Instance.Pistol_MagEmpty.Play();
                SoundManager.Instance.PlayEmptySound(currentWeaponModel);
            }
            if (currentShootingMode == ShootingMode.Auto)
            {
                // holding down Left Mouse Button
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == ShootingMode.Single ||
                    currentShootingMode == ShootingMode.Burst)
            {
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                burstBulletsLeft = bulletsPerBurst;
                FireWeapon();
            }
            // Manual Reload
            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !isReloading)
            {
                Reload();
            }

            // auto Reload if the autoReload bool is set
            if (autoReload && !isShooting && !isReloading && bulletsLeft >= 0)
            {
                Reload();
            }

            if (AmmoManager.Instance.ammoDisplay != null)
            {
                AmmoManager.Instance.ammoDisplay.text = $"{bulletsLeft / bulletsPerBurst}/{magazineSize / bulletsPerBurst}";
            }
        }

    }

    private void FireWeapon()
    {
        bulletsLeft --;

        muzzleEffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("Recoil");

        //SoundManager.Instance.Pistol_ShootingSound.Play();
        SoundManager.Instance.PlayShootingSound(currentWeaponModel);

        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        // instantiate the bullet
         GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        
        // point the bullet to face the shooting direction
         bullet.transform.forward = shootingDirection;

        // shoot the bullet
         bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);
        // Destroy the bullet AFTER SOME TIME
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletLifetime));

        // checking if we are done shooting
        if(allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        // Burst Mode
        if(currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1) // we already shoot once before this check
        {
            burstBulletsLeft --;
            Invoke("FireWeapon", shootingDelay);
        }

    }


    private void Reload()
    {
        SoundManager.Instance.PlayReloadSound(currentWeaponModel);

        animator.SetTrigger("Reload");

        isReloading = true;
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        bulletsLeft = magazineSize;
        isReloading = false;

    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }
     
    public Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
        {
            // hitting something
            targetPoint = hit.point;
        }
        else
        {
            // shooting at the air
            targetPoint = ray.GetPoint(100f);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        // returning the shooting direction and spread
        return direction + new Vector3(x, y, 0);

    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }


}
