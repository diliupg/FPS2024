using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
     public static SoundManager Instance {get; set;}


    // Pistol Audio
    public AudioSource ShootingChannel;


    //AudioClips
    public AudioClip PistolShoot;
     public AudioClip RifleShoot;
     public AudioClip PistolReload;
     public AudioClip RifleReload;

    public AudioClip PistolEmpty;
     public AudioClip RifleEmpty;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayShootingSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol: 
                ShootingChannel.PlayOneShot(PistolShoot);
                break;
            case WeaponModel.Rifle:
                ShootingChannel.PlayOneShot(RifleShoot);
                 break;

        }
    }

    public void PlayReloadSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol:
                ShootingChannel.PlayOneShot(PistolReload);
                break;
            case WeaponModel.Rifle:
                ShootingChannel.PlayOneShot(RifleReload);
                 break;

        }
    }

    public void PlayEmptySound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol:
                ShootingChannel.PlayOneShot(PistolEmpty);
                break;
            case WeaponModel.Rifle:
                ShootingChannel.PlayOneShot(RifleEmpty);
                 break;

        }
    }





}
