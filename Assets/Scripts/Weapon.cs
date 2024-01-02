 using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;

    [Header("Shooting")]
     // shooting
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay;

     // burst
    [Header("Set this to 1 or more to prevent division by 0")]
    public int bulletsPerBurst = 4;
    public int burstBulletsLeft;

     // spread
    [Header("Bullet Spread")]
    public float spreadIntensity;
    public float hipSpreadIntensity;
    public float ADSSpreadIntensity;

    [Header("Bullets")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 500f;
    public float bulletLifetime = 3f;

    public GameObject muzzleEffect;
    internal Animator animator; // internal - other scripts can access it but not from the inspector

    [Header("Reloading")]
    // loading
    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;

    [Header ("Set this for Auto Reload")]
    public bool autoReload;
    
    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

    private bool isADS;
    public enum WeaponModel
    {
        Pistol,
        Rifle,
        Laser,
        GrenadeLauncher
    }
    
    public WeaponModel thisWeaponModel;
    
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

        spreadIntensity = hipSpreadIntensity;
    }

    void Update()
    {
        
        if (isActiveWeapon)
        {
            if(Input.GetMouseButtonDown(1))
            {
                EnterADS();
            }
            if(Input.GetMouseButtonUp(1))
            {
                ExitADS();
            }

            GetComponent<Outline>().enabled = false;
            
            if (bulletsLeft == 0 && isShooting)
            {
                //SoundManager.Instance.Pistol_MagEmpty.Play();
                SoundManager.Instance.PlayEmptySound(thisWeaponModel);
            }
            if (currentShootingMode == ShootingMode.Auto)
            {
                // detects holding down Left Mouse Button
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == ShootingMode.Single ||
                    currentShootingMode == ShootingMode.Burst)
            {
                // detect the downward pressing of the mouse button or key, which happens once every press
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                burstBulletsLeft = bulletsPerBurst;
                FireWeapon();
            }
            // Manual Reload
            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !isReloading && 
                                WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > 0)
            {
                Reload();
            }

            // auto Reload if the autoReload bool is set
            if (autoReload && !isShooting && !isReloading && bulletsLeft >= 0)
            {
                Reload();
            }
        }
    }

    private void EnterADS()
    {
        animator.SetTrigger("EnterADS");
        isADS = true;
        HUDManager.Instance.middleDot.SetActive(false);
        spreadIntensity = ADSSpreadIntensity;
    }

    private void ExitADS()
    {
        animator.SetTrigger("ExitADS");
        isADS = false;
        HUDManager.Instance.middleDot.SetActive(true);
        spreadIntensity = hipSpreadIntensity;
    }
    
    private void FireWeapon()
    {
        bulletsLeft --;

        muzzleEffect.GetComponent<ParticleSystem>().Play();

        if(isADS)
        {
            animator.SetTrigger("Recoil_ADS");
        }
        else
        {
            animator.SetTrigger("Recoil");
        }

        //SoundManager.Instance.Pistol_ShootingSound.Play();
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

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
        SoundManager.Instance.PlayReloadSound(thisWeaponModel);

        animator.SetTrigger("Reload"); // run the reload animation 

        isReloading = true;
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        if(WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > magazineSize)
        {
            bulletsLeft = magazineSize;
            WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
        }
        else
        {
            bulletsLeft = WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel);
            WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
        }
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

        float z = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        // returning the shooting direction and spread
        return direction + new Vector3(0, y, z);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }


}
