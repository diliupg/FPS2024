using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
     public static SoundManager Instance {get; set;}

    
    public AudioSource Pistol_ShootingSound;
    public AudioSource Pistol_MagEmpty;
    public AudioSource Pistol_Reload;

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
}
