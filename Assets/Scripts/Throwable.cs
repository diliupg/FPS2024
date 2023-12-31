using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] float delay = 3f;
    [SerializeField] float damageRadius = 20f;
    [SerializeField] float explosionForce = 1200f;

    float countdown;

    public bool hasExploded;
    public bool hasBeenThrown;

    public enum ThrowableType
    {
        None,
        Grenade,
        SmokeGrenade
    }

    public ThrowableType throwableType;

    private void Start()
    {
        countdown = delay;
    }

    private void Update()
    {
        if(hasBeenThrown)
        {
            countdown -= Time.deltaTime;
            if(countdown <= 0f && !hasExploded)
            {
                Explode();
                hasExploded = true;
            }
        }
    }

    private void Explode()
    {
        GetThrowableEffect();
        Destroy(gameObject);
    }

    private void GetThrowableEffect()
    {
        switch(throwableType)
        {
            case ThrowableType.Grenade:
                GrenadeEffect();
                break;
                case ThrowableType.SmokeGrenade:
                SmokeGrenadeEffect();
                break;
        }
    }

    private void GrenadeEffect()
    {
        // Visual Effect
        GameObject explosionEffect = GlobalReferences.Instance.grenadeExplosionEffect ;
        var destroyLater = Instantiate(explosionEffect, transform.position, transform.rotation);

        // Play sound
        SoundManager.Instance.throwablesChannel.PlayOneShot(SoundManager.Instance.grenadeSound);
        
        // Physical Effect
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach(Collider objectInRange in colliders)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, damageRadius);
            }

            // also apply damage to enemy over here
        }

        Destroy(destroyLater, 4f);
    }

    private void SmokeGrenadeEffect()
    {
        // Visual Effect
        GameObject smokeEffect = GlobalReferences.Instance.smokeGrenadeExplosionEffect ;
        var destroyLater = Instantiate(smokeEffect, transform.position, transform.rotation);

        // Play sound
        SoundManager.Instance.throwablesChannel.PlayOneShot(SoundManager.Instance.smokeGrenadeSound);
        
        // Physical Effect
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach(Collider objectInRange in colliders)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if(rb != null)
            {
                // apply blindness and stun effect to enemy
            }

            // also apply damage to enemy over here
        }

        Destroy(destroyLater, 2f);
    }
}
 